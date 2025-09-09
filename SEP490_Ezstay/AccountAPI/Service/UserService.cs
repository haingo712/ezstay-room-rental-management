using AccountAPI.Data;
using AccountAPI.DTO.Reponse;
using AccountAPI.DTO.Request;

using AccountAPI.Repositories.Interfaces;
using AccountAPI.Service.Interfaces;
using AutoMapper;

namespace AccountAPI.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
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
    }
}