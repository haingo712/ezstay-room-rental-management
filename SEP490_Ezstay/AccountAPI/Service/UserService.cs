using AccountAPI.Model;
using AccountAPI.DTO.Reponse;
using AccountAPI.DTO.Request;
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

        public async Task<bool> CreateProfileAsync(Guid id, CreateUserDTO createUserDto)
        {
            var existingPhone = await _authApiClient.GetByIdAsync(id);

            var existingUser = await _userRepository.GetByUserIdAsync(id);
            if (existingUser != null)
            {
                return false;
            } 
             var existingCitizenIdNumber = await _userRepository.GetCitizenIdNumber(createUserDto.CitizenIdNumber);
            if (existingCitizenIdNumber != null)
            {
                Console.WriteLine($"CitizenIdNumber {createUserDto.CitizenIdNumber} đã tồn tại");
                return false;
            }
            var user = _mapper.Map<User>(createUserDto);
            user.Id = id;
            if (createUserDto.Avatar != null)
            {
                user.Avatar = await _imageService.UploadImageAsync(createUserDto.Avatar);
            }
          
            user.FullName = existingPhone.FullName;
            user.Phone = existingPhone.Phone;
            user.Email = existingPhone.Email;
            user.FrontImageUrl = await _imageService.UploadImageAsync(createUserDto.FrontImageUrl);

            user.BackImageUrl = await _imageService.UploadImageAsync(createUserDto.BackImageUrl);

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
           
            _mapper.Map(userDto, user);
            // user.ProvinceName = await GetProvinceNameAsync(user.ProvinceId) ?? "";
            // user.WardName = await GetCommuneNameAsync(user.ProvinceId, user.WardId) ?? "";
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
        public async Task<UserResponseDTO> GetCitizenIdNumber(string citizenIdNumber)
        {
            var result = await _userRepository.GetCitizenIdNumber(citizenIdNumber);
            return _mapper.Map<UserResponseDTO>(result);

        }

        public async Task<bool> CheckProfileAsync(Guid id)
        {
            var user = await _userRepository.GetByUserIdAsync(id);
            if (user == null) return false;

            bool isValid =
                !string.IsNullOrWhiteSpace(user.FullName) &&
                !string.IsNullOrWhiteSpace(user.Phone) &&
                !string.IsNullOrWhiteSpace(user.Avatar) &&
                user.DateOfBirth != default &&
                !string.IsNullOrWhiteSpace(user.CitizenIdNumber) &&
                user.CitizenIdIssuedDate != default &&
                !string.IsNullOrWhiteSpace(user.CitizenIdIssuedPlace) &&
                !string.IsNullOrWhiteSpace(user.FrontImageUrl) &&
                !string.IsNullOrWhiteSpace(user.BackImageUrl);

            return isValid;
        }
    }
}