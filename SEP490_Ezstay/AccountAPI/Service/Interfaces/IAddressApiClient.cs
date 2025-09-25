namespace AccountAPI.Service.Interfaces
{
    public interface IAddressApiClient
    {
        Task<string?> GetProvinceNameAsync(string provinceId);
        Task<string?> GetCommuneNameAsync(string provinceId, string communeId);
    }
}
