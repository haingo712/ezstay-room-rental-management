using AccountAPI.Data;
using AccountAPI.DTO.Reponse;
using AccountAPI.DTO.Request;
using AccountAPI.DTO.Resquest;
using AccountAPI.Repositories.Interfaces;
using AccountAPI.Service.Interfaces;
using APIGateway.Helper.Interfaces;
using AutoMapper;
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
        private readonly IUserClaimHelper _userClaimHelper;
        private readonly IAddressApiClient _addressClient;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IImageService imageService,
            IAuthApiClient authApiClient,
            IUserClaimHelper userClaimHelper,
            IPhoneOtpClient otpClient,
            IAddressApiClient addressClient)
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

            userResponse.ProvinceName = user.ProvinceName;
            userResponse.CommuneName = user.CommuneName;

            return userResponse;
        }

        public async Task<bool> UpdateProfileAsync(Guid userId, UpdateUserDTO dto)
        {
            var userEntity = await _userRepository.GetByUserIdAsync(userId);
            if (userEntity == null) return false;

            _mapper.Map(dto, userEntity);

            if (dto.Avatar != null)
            {
                var avatarUrl = await _imageService.UploadImageAsync(dto.Avatar);
                userEntity.Avatar = avatarUrl;
            }

            // 🔁 Load address cache (chỉ 1 lần nếu chưa load)
            await _addressClient.LoadAsync();

            if (!string.IsNullOrEmpty(dto.ProvinceCode))
            {
                userEntity.ProvinceCode = dto.ProvinceCode;
                userEntity.ProvinceName = _addressClient.GetProvinceName(dto.ProvinceCode) ?? dto.ProvinceCode;
            }

            if (!string.IsNullOrEmpty(dto.ProvinceCode) && !string.IsNullOrEmpty(dto.CommuneCode))
            {
                userEntity.CommuneCode = dto.CommuneCode;
                userEntity.CommuneName = _addressClient.GetCommuneName(dto.ProvinceCode, dto.CommuneCode) ?? dto.CommuneCode;
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

        public async Task<bool> UpdateEmailAsync(string currentEmail, string newEmail, string otp)
        {
            var verified = await _authApiClient.ConfirmOtpAsync(newEmail, otp);
            if (!verified) return false;

            var updated = await _authApiClient.UpdateEmailAsync(currentEmail, newEmail);
            return updated;
        }
    }
}
