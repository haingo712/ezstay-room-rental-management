package AccountAPI.Service.Interfaces;

public interface IPhoneOtpClient {

	public Task<boolean> SendOtpAsync(String aPhone);

	public Task<boolean> VerifyOtpAsync(String aPhone, String aOtp);
}