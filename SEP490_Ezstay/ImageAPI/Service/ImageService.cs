using System.Net.Http.Headers;
using System.Text.Json;

public class ImageService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ImageService(IConfiguration config)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://rpc.filebase.io/");
        _apiKey = config["FilebaseSettings:IpfsKey"];

        // Thêm header mặc định
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        using var form = new MultipartFormDataContent();
        using var stream = file.OpenReadStream();

        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

        form.Add(fileContent, "file", file.FileName);

        // Gọi endpoint IPFS
        var response = await _httpClient.PostAsync("api/v0/add", form);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var obj = JsonDocument.Parse(json);
        var cid = obj.RootElement.GetProperty("Hash").GetString();

        // Trả về link IPFS Gateway
        return $"https://ipfs.filebase.io/ipfs/{cid}";
    }
    public async Task<List<string>> UploadMultipleAsync(IFormFileCollection files)
    {
        var urls = new List<string>();
        foreach (var file in files)
        {
            var url = await UploadAsync(file);
            urls.Add(url);
        }
        return urls;
    }
}
