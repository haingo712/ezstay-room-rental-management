    using AccountAPI.Data;
    using AccountAPI.DTO.Reponse;
    using AccountAPI.DTO.Request;
    using AccountAPI.DTO.Resquest;
    using AccountAPI.Repositories.Interfaces;
    using AccountAPI.Service.Interfaces;
    using APIGateway.Helper.Interfaces;
    using AutoMapper;
    using System.Security.Claims;

    namespace AccountAPI.Service
    {
        public class UserService : IUserService
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            private readonly IImageService _imageService;
            private readonly IAuthApiClient _authApiClient;

            private readonly IUserClaimHelper _userClaimHelper;

            public UserService(
                IUserRepository userRepository,
                IMapper mapper,
                IImageService imageService,
                IAuthApiClient authApiClient,
                IUserClaimHelper userClaimHelper) // 👈 thêm dòng này
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _imageService = imageService;
                _authApiClient = authApiClient;
                _userClaimHelper = userClaimHelper; // 👈 gán vào
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
            var currentEmail = _userClaimHelper.GetEmail(user); // ✅ Dùng ClaimsPrincipal

            // ✅ Nếu có yêu cầu đổi email
            if (!string.IsNullOrEmpty(dto.NewEmail))
            {
                if (string.IsNullOrEmpty(dto.Otp))
                    return false; // Thiếu OTP

                // ✅ Xác thực OTP từ AuthAPI
                var verified = await _authApiClient.ConfirmOtpAsync(dto.NewEmail, dto.Otp);
                if (!verified)
                    return false; // OTP sai hoặc hết hạn

                // ✅ Cập nhật email ở AuthAPI
                var updateSuccess = await _authApiClient.UpdateEmailAsync(currentEmail, dto.NewEmail);
                if (!updateSuccess)
                    return false; // Gọi AuthAPI thất bại
            }

            // ✅ Cập nhật các field còn lại trong bảng User
            _mapper.Map(dto, userEntity);

            if (dto.Avatar != null)
            {
                var avatarUrl = await _imageService.UploadImageAsync(dto.Avatar);
                userEntity.Avata = avatarUrl;
            }

            await _userRepository.UpdateAsync(userEntity);
            return true;
        }


    }


}