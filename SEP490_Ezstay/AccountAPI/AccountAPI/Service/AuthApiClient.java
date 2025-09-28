package AccountAPI.Service;

import AccountAPI.Service.Interfaces.IAuthApiClient;

public class AuthApiClient implements IAuthApiClient {
	private HttpClient _http;

	public AuthApiClient(HttpClient aHttp) {
		throw new UnsupportedOperationException();
	}

	public async_Task<boolean> ConfirmOtpAsync(String aEmail, String aOtp) {
		throw new UnsupportedOperationException();
	}

	public async_Task<boolean> UpdateEmailAsync(String aOldEmail, String aNewEmail) {
		throw new UnsupportedOperationException();
	}
}