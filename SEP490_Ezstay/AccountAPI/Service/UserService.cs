using AccountAPI.Data;
using AccountAPI.DTO.Reponse;
using AccountAPI.DTO.Request;
using AccountAPI.DTO.Resquest;
using AccountAPI.Repositories.Interfaces;
using AccountAPI.Service.Interfaces;
using APIGateway.Helper.Interfaces;
using AutoMapper;
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

        private readonly IAddressApiClient _addressClient;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IImageService imageService,
            IAuthApiClient authApiClient,
            IUserClaimHelper userClaimHelper,
            IPhoneOtpClient otpClient,
            IAddressApiClient addressClient) // 👈 inject đúng
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _imageService = imageService;
            _authApiClient = authApiClient;
            _userClaimHelper = userClaimHelper;
            _otpClient = otpClient;
            _addressClient = addressClient;
        }


        public async Task<bool> CreateProfileAsync(Guid userId, UserDTO userDto)
        {
            var existingUser = await _userRepository.GetByUserIdAsync(userId);
            if (existingUser != null) return false;

            var user = _mapper.Map<User>(userDto);
            user.UserId = userId;

            await _userRepository.CreateUserAsync(user);
            return true;
        }

        public async Task<UserResponseDTO?> GetProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user == null) return null;

            var userResponse = _mapper.Map<UserResponseDTO>(user);
            return userResponse;
        }

        public async Task<bool> UpdateProfileAsync(Guid userId, UpdateUserDTO dto)
        {
            var userEntity = await _userRepository.GetByUserIdAsync(userId);
            if (userEntity == null) return false;

            // ✅ Cập nhật từng field có giá trị (tránh ghi đè null)
            if (dto.Gender.HasValue)
                userEntity.Gender = dto.Gender.Value;

            if (!string.IsNullOrEmpty(dto.Bio))
                userEntity.Bio = dto.Bio;

            if (dto.DateOfBirth.HasValue)
                userEntity.DateOfBirth = dto.DateOfBirth.Value;

            if (!string.IsNullOrEmpty(dto.FullName))
                userEntity.FullName = dto.FullName;

            if (!string.IsNullOrEmpty(dto.Phone))
                userEntity.Phone = dto.Phone;

            if (!string.IsNullOrEmpty(dto.DetailAddress))
                userEntity.DetailAddress = dto.DetailAddress;

            // ✅ Upload avatar nếu có file mới
            if (dto.Avatar != null)
            {
                var avatarUrl = await _imageService.UploadImageAsync(dto.Avatar);
                userEntity.Avata = avatarUrl;
            }

            // ✅ Update địa chỉ nếu có ProvinceId + CommuneId hợp lệ
            if (!string.IsNullOrEmpty(dto.ProvinceId) && !string.IsNullOrEmpty(dto.CommuneId))
            {
                var provinceName = await _addressClient.GetProvinceNameAsync(dto.ProvinceId);
                var communeName = await _addressClient.GetCommuneNameAsync(dto.ProvinceId, dto.CommuneId);

                if (!string.IsNullOrEmpty(provinceName) && !string.IsNullOrEmpty(communeName))
                {
                    userEntity.Province = provinceName;
                    userEntity.Commune = communeName;
                }
            }

            await _userRepository.UpdateAsync(userEntity);
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

        //private async Task<string?> GetProvinceNameAsync(string provinceId)
        //{
        //    var response = await _http.GetFromJsonAsync<JsonElement>("/api/provinces");
        //    var provinces = response.GetProperty("provinces").EnumerateArray();
        //    return provinces.FirstOrDefault(p => p.GetProperty("code").GetString() == provinceId)
        //                    .GetProperty("name").GetString();
        //}


        //private async Task<string?> GetCommuneNameAsync(string provinceId, string communeId)
        //{
        //    var response = await _http.GetFromJsonAsync<JsonElement>($"/api/provinces/{provinceId}/communes");
        //    var communes = response.GetProperty("communes").EnumerateArray();
        //    return communes.FirstOrDefault(c => c.GetProperty("code").GetString() == communeId)
        //                   .GetProperty("name").GetString();
        //}


        public async Task<bool> UpdateEmailAsync(string currentEmail, string newEmail, string otp)
        {
            var verified = await _authApiClient.ConfirmOtpAsync(newEmail, otp);
            if (!verified) return false;

            var updated = await _authApiClient.UpdateEmailAsync(currentEmail, newEmail);
            return updated;
        }




    }


}