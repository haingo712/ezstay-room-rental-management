namespace AccountAPI.Service.Interfaces
{
    public interface IAddressApiClient
    {
        
            Task LoadAsync(); // Gọi 1 lần duy nhất
            string? GetProvinceName(string code);
            string? GetCommuneName(string provinceCode, string communeCode);
        

    }
}
