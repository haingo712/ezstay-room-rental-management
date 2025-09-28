package AccountAPI.Service;

import AccountAPI.Service.Interfaces.IPhoneOtpClient;

public class PhoneOtpClient implements IPhoneOtpClient {
	private HttpClient _http;

	public PhoneOtpClient(HttpClient aHttp) {
		throw new UnsupportedOperationException();
	}

	public async_Task<boolean> SendOtpAsync(String aPhone) {
		throw new UnsupportedOperationException();
	}

	public async_Task<boolean> VerifyOtpAsync(String aPhone, String aOtp) {
		throw new UnsupportedOperationException();
	}
}