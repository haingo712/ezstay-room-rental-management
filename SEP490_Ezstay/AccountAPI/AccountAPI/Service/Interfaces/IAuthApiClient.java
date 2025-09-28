package AccountAPI.Service.Interfaces;

public interface IAuthApiClient {

	public Task<boolean> ConfirmOtpAsync(String aEmail, String aOtp);

	public Task<boolean> UpdateEmailAsync(String aOldEmail, String aNewEmail);
}