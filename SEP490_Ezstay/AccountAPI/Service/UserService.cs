using AccountAPI.Data;
using AccountAPI.DTO.Reponse;
using AccountAPI.DTO.Request;
using AccountAPI.DTO.Resquest;
using AccountAPI.Repositories.Interfaces;
using AccountAPI.Service.Interfaces;
using AutoMapper;

namespace AccountAPI.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public UserService(IUserRepository userRepository, IMapper mapper, IImageService imageService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<bool> CreateProfileAsync(Guid userId, UserDTO userDto)
        {
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

            // Không cần gọi Account nữa
            // Giữ nguyên logic nếu cần thêm sau này
            return userResponse;
        }

        public async Task<bool> UpdateProfileAsync(Guid userId, UpdateUserDTO dto)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user == null) return false;

            // map các field cơ bản
            _mapper.Map(dto, user);

            // xử lý avatar (nếu có upload)
            if (dto.Avatar != null)
            {
                var avatarUrl = await _imageService.UploadImageAsync(dto.Avatar);
                user.Avata = avatarUrl;
            }

            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}