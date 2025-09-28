package AccountAPI.Service.Interfaces;

public interface IAddressApiClient {

	public Task<?String> GetProvinceNameAsync(String aProvinceId);

	public Task<?String> GetCommuneNameAsync(String aProvinceId, String aCommuneId);
}