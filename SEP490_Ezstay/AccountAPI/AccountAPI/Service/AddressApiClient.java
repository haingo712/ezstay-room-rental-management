package AccountAPI.Service;

import AccountAPI.Service.Interfaces.IAddressApiClient;

public class AddressApiClient implements IAddressApiClient {
	private HttpClient _http;

	public AddressApiClient(HttpClient aHttp) {
		throw new UnsupportedOperationException();
	}

	public async_Task<?String> GetProvinceNameAsync(String aProvinceId) {
		throw new UnsupportedOperationException();
	}

	public async_Task<?String> GetCommuneNameAsync(String aProvinceId, String aCommuneId) {
		throw new UnsupportedOperationException();
	}
}