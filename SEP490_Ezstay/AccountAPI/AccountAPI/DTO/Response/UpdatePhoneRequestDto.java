package AccountAPI.DTO.Response;

public class UpdatePhoneRequestDto {
	public final String _phone;
	public final String _otp = string.Empty;

	public String getPhone() {
		return this._phone;
	}

	public String getOtp() {
		return this._otp;
	}
}