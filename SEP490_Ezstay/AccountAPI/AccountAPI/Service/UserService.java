package AccountAPI.Service;

import AccountAPI.Repositories.Interfaces.IUserRepository;
import AccountAPI.Service.Interfaces.IImageService;
import AccountAPI.Service.Interfaces.IAuthApiClient;
import AccountAPI.Service.Interfaces.IPhoneOtpClient;
import AccountAPI.Service.Interfaces.IAddressApiClient;
import AccountAPI.DTO.Request.UserDTO;
import AccountAPI.DTO.Reponse.UserResponseDTO;
import AccountAPI.DTO.Request.UpdateUserDTO;
import AccountAPI.Service.Interfaces.IUserService;

public class UserService implements IUserService {
	private IUserRepository _userRepository;
	private IMapper _mapper;
	private IImageService _imageService;
	private IAuthApiClient _authApiClient;
	private IPhoneOtpClient _otpClient;
	private HttpClient _http;
	private IUserClaimHelper _userClaimHelper;
	private IAddressApiClient _addressClient;
	public IUserRepository _unnamed_IUserRepository_;
	public IImageService _unnamed_IImageService_;
	public IAuthApiClient _unnamed_IAuthApiClient_;
	public IPhoneOtpClient _unnamed_IPhoneOtpClient_;
	public IAddressApiClient _unnamed_IAddressApiClient_;

	public UserService(IUserRepository aUserRepository, IMapper aMapper, IImageService aImageService, IAuthApiClient aAuthApiClient, IUserClaimHelper aUserClaimHelper, IPhoneOtpClient aOtpClient, IAddressApiClient aAddressClient) {
		throw new UnsupportedOperationException();
	}

	public async_Task<boolean> CreateProfileAsync(Guid aUserId, UserDTO aUserDto) {
		throw new UnsupportedOperationException();
	}

	public async_Task<?UserResponseDTO> GetProfileAsync(Guid aUserId) {
		throw new UnsupportedOperationException();
	}

	public async_Task<boolean> UpdateProfileAsync(Guid aUserId, UpdateUserDTO aDto) {
		throw new UnsupportedOperationException();
	}

	public async_Task<boolean> SendOtpToPhoneAsync(String aPhone) {
		throw new UnsupportedOperationException();
	}

	public async_Task<boolean> VerifyPhoneOtpAsync(String aPhone, String aOtp) {
		throw new UnsupportedOperationException();
	}

	public async_Task<boolean> UpdatePhoneAsync(Guid aUserId, String aNewPhone) {
		throw new UnsupportedOperationException();
	}

	public async_Task<boolean> UpdateEmailAsync(String aCurrentEmail, String aNewEmail, String aOtp) {
		throw new UnsupportedOperationException();
	}
}