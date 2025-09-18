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

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IImageService imageService,
            IAuthApiClient authApiClient,
            IUserClaimHelper userClaimHelper,
            IPhoneOtpClient otpClient,
            HttpClient http) // 👈 thêm dòng này
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _imageService = imageService;
            _authApiClient = authApiClient;
            _userClaimHelper = userClaimHelper; // 👈 gán vào
            _otpClient = otpClient;
            _http = http;
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

        public async Task<bool> UpdateProfileAsync(Guid userId, UpdateUserDTO dto, ClaimsPrincipal user)
        {
            var userEntity = await _userRepository.GetByUserIdAsync(userId);
            if (userEntity == null) return false;

            // 🟡 Lấy email hiện tại từ token
            var currentEmail = _userClaimHelper.GetEmail(user);

            // ✅ Xác thực & cập nhật Email nếu có yêu cầu đổi
            if (!string.IsNullOrEmpty(dto.NewEmail))
            {
                if (string.IsNullOrEmpty(dto.Otp))
                    return false; // Thiếu OTP

                var verified = await _authApiClient.ConfirmOtpAsync(dto.NewEmail, dto.Otp);
                if (!verified)
                    return false; // OTP sai hoặc hết hạn

                var updateSuccess = await _authApiClient.UpdateEmailAsync(currentEmail, dto.NewEmail);
                if (!updateSuccess)
                    return false; // Gọi AuthAPI thất bại
            }

            // ✅ Cập nhật các field khác từ DTO
            _mapper.Map(dto, userEntity);

            // ✅ Cập nhật avatar nếu có
            if (dto.Avatar != null)
            {
                var avatarUrl = await _imageService.UploadImageAsync(dto.Avatar);
                userEntity.Avata = avatarUrl;
            }

            // ✅ Cập nhật địa chỉ nếu có ProvinceId & CommuneId
            if (!string.IsNullOrEmpty(dto.ProvinceId) && !string.IsNullOrEmpty(dto.CommuneId))
            {
                var provinceName = await GetProvinceNameAsync(dto.ProvinceId);
                var communeName = await GetCommuneNameAsync(dto.ProvinceId, dto.CommuneId);

                if (provinceName != null && communeName != null)
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




    }


}