package AccountAPI.DTO.Resquest;

public class UpdateEmailRequestDto {
	public final String _newEmail;
	public final String _otp;

	public String getNewEmail() {
		return this._newEmail;
	}

	public String getOtp() {
		return this._otp;
	}
}