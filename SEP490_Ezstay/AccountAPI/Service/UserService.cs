﻿using AccountAPI.Data;
using AccountAPI.DTO.Reponse;
using AccountAPI.DTO.Request;
using AccountAPI.DTO.Resquest;
using AccountAPI.Repositories.Interfaces;
using AccountAPI.Service.Interfaces;
using APIGateway.Helper;
using APIGateway.Helper.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace AccountAPI.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IAuthApiClient _authApiClient;
        private readonly IPhoneOtpClient _otpClient;
        private readonly HttpClient _http;
        private readonly IUserClaimHelper _userClaimHelper;

    

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IImageService imageService,
            IAuthApiClient authApiClient,
            IUserClaimHelper userClaimHelper,
            IPhoneOtpClient otpClient,
             IHttpClientFactory factory
            ) // 👈 inject đúng
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _imageService = imageService;
            _authApiClient = authApiClient;
            _userClaimHelper = userClaimHelper;
            _otpClient = otpClient;
            _http = factory.CreateClient("Gateway");
        }


        public async Task<bool> CreateProfileAsync(Guid userId, UserDTO userDto)
        {
            var existingPhone = await _authApiClient.GetByIdAsync(userId);
            if (existingPhone != null)
            {
                   var existingUser = await _userRepository.GetByUserIdAsync(userId);
                if (existingUser != null)
                {
                    return false; // User profile already exists
                }
            }
            var user = _mapper.Map<User>(userDto);
            user.UserId = userId;
            user.FullName = existingPhone.FullName;
            user.Phone = existingPhone.Phone;
            user.Email = existingPhone.Email;
            if (userDto.Avatar != null)
            {
                var avatarUrl = await _imageService.UploadImageAsync(userDto.Avatar);
                user.Avatar = avatarUrl;
            }

            user.ProvinceName = await GetProvinceNameAsync(user.ProvinceId) ?? "";
          user.WardName = await GetCommuneNameAsync(user.ProvinceId, user.WardId) ?? "";
          await _userRepository.CreateUserAsync(user);
            return true;
        }

        public async Task<UserResponseDTO?> GetProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            var authenticatedUser = await _authApiClient.GetByIdAsync(userId);

            if (user == null || authenticatedUser == null)
                return null;

            var userResponse = _mapper.Map<UserResponseDTO>(user);

            // ✅ Gán thông tin từ AuthAPI vào DTO trả về
            userResponse.FullName = authenticatedUser.FullName;
            userResponse.Phone = authenticatedUser.Phone;
            userResponse.Email = authenticatedUser.Email;

            return userResponse;
        }

        private async Task<string?> GetProvinceNameAsync(string provinceId)
        {
            var response = await _http.GetFromJsonAsync<JsonElement>("/api/provinces");
            var provinces = response.GetProperty("provinces").EnumerateArray();
            return provinces.FirstOrDefault(p => p.GetProperty("code").GetString() == provinceId)
                            .GetProperty("name").GetString();
        }

        private async Task<string?> GetCommuneNameAsync(string provinceId, string communeId)
        {
            var response = await _http.GetFromJsonAsync<JsonElement>($"/api/provinces/{provinceId}/communes");
            var communes = response.GetProperty("communes").EnumerateArray();
            return communes.FirstOrDefault(c => c.GetProperty("code").GetString() == communeId)
                           .GetProperty("name").GetString();
        }

        public async Task<bool> UpdateProfile(Guid userId, UpdateUserDTO userDto)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user == null)
                return false;

            // ✅ Cập nhật thông tin mở rộng
            _mapper.Map(userDto, user);

            // ✅ Nếu userDto có FullName thì lưu vào DB local
            if (!string.IsNullOrEmpty(userDto.FullName))
            {
                user.FullName = userDto.FullName;

                // ✅ Gọi Auth API để đồng bộ
                await _authApiClient.UpdateFullNameAsync(userId, userDto.FullName);
            }
           
            if (userDto.Avatar != null)
            {
                var avatarUrl = await _imageService.UploadImageAsync(userDto.Avatar);
                user.Avatar = avatarUrl; // ✅ gán đúng URL ảnh vào DB
            }

            if (userDto.FrontImageUrl != null)
            {
                var frontUrl = await _imageService.UploadImageAsync(userDto.FrontImageUrl);
                user.FrontImageUrl = frontUrl;
            }

            if (userDto.BackImageUrl != null)
            {
                var backUrl = await _imageService.UploadImageAsync(userDto.BackImageUrl);
                user.BackImageUrl = backUrl;
            }


            // ✅ Cập nhật tên tỉnh/xã nếu cần
            if (!string.IsNullOrEmpty(user.ProvinceId))
                user.ProvinceName = await GetProvinceNameAsync(user.ProvinceId) ?? "";

            if (!string.IsNullOrEmpty(user.WardId) && !string.IsNullOrEmpty(user.ProvinceId))
                user.WardName = await GetCommuneNameAsync(user.ProvinceId, user.WardId) ?? "";

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> SendOtpToPhoneAsync(string phone)
        {
            return await _otpClient.SendOtpAsync(phone);
        }

        public async Task<bool> VerifyPhoneOtpAsync(string phone, string otp)
        {
            return await _otpClient.VerifyOtpAsync(phone, otp);
        }

        public async Task<bool> UpdatePhoneAsync(Guid userId, string newPhone)
        {
            var userEntity = await _userRepository.GetByUserIdAsync(userId);
            if (userEntity == null) return false;

            userEntity.Phone = newPhone;
            await _userRepository.UpdateAsync(userEntity);
            return true;
        }

        public async Task<bool> UpdateEmailAsync(string currentEmail, string newEmail, string otp)
        {
            var verified = await _authApiClient.ConfirmOtpAsync(newEmail, otp);
            if (!verified) return false;

            var updated = await _authApiClient.UpdateEmailAsync(currentEmail, newEmail);
            return updated;
        }

        public async Task<UserResponseDTO> GetPhone(string phone)
        {
            var Phone = await _userRepository.GetPhone(phone);
            return _mapper.Map<UserResponseDTO>(Phone);
           
        }
    }
}