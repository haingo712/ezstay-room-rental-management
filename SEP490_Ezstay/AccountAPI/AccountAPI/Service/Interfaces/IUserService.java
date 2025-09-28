package AccountAPI.Service.Interfaces;

import AccountAPI.DTO.Request.UserDTO;
import AccountAPI.DTO.Reponse.UserResponseDTO;
import AccountAPI.DTO.Request.UpdateUserDTO;

public interface IUserService {

	public Task<boolean> CreateProfileAsync(Guid aUserId, UserDTO aUserDto);

	public Task<?UserResponseDTO> GetProfileAsync(Guid aUserId);

	public Task<boolean> UpdateProfileAsync(Guid aUserId, UpdateUserDTO aDto);

	public Task<boolean> UpdatePhoneAsync(Guid aUserId, String aNewPhone);

	public Task<boolean> VerifyPhoneOtpAsync(String aPhone, String aOtp);

	public Task<boolean> SendOtpToPhoneAsync(String aPhone);

	public Task<boolean> UpdateEmailAsync(String aCurrentEmail, String aNewEmail, String aOtp);
}