//
// using System.Net.Http.Headers;
// using System.Text;
// using System.Text.Json;
// using PaymentAPI.Services.Interfaces;
//
// namespace PaymentAPI.Services;
//
// public class PaypalService : IPaymentService
// {
//     private readonly string _clientId = "YOUR_PAYPAL_CLIENT_ID";
//     private readonly string _secret = "YOUR_PAYPAL_SECRET";
//     private readonly string _baseUrl = "https://api-m.sandbox.paypal.com";
//
//     public async Task<string> CreatePaymentUrl(Guid orderId, decimal amount)
//     {
//         using var client = new HttpClient();
//
//         // Step 1: Get access token
//         var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_secret}"));
//         client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
//
//         var authResponse = await client.PostAsync($"{_baseUrl}/v1/oauth2/token",
//             new FormUrlEncodedContent(new Dictionary<string, string>
//             {
//                 { "grant_type", "client_credentials" }
//             }));
//
//         var authJson = await authResponse.Content.ReadAsStringAsync();
//         var token = JsonDocument.Parse(authJson).RootElement.GetProperty("access_token").GetString();
//
//         // Step 2: Create an order
//         client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//
//         var orderBody = new
//         {
//             intent = "CAPTURE",
//             purchase_units = new[]
//             {
//                 new { amount = new { currency_code = "USD", value = amount.ToString("F2") } }
//             },
//             application_context = new
//             {
//                 return_url = "https://yourdomain.com/payment/paypal/callback",
//                 cancel_url = "https://yourdomain.com/payment/paypal/cancel"
//             }
//         };
//
//         var response = await client.PostAsJsonAsync($"{_baseUrl}/v2/checkout/orders", orderBody);
//         var json = await response.Content.ReadAsStringAsync();
//
//         var links = JsonDocument.Parse(json).RootElement.GetProperty("links");
//         string approvalUrl = links.EnumerateArray()
//             .First(x => x.GetProperty("rel").GetString() == "approve")
//             .GetProperty("href").GetString();
//
//         return approvalUrl;
//     }
//
//     public Task<bool> VerifyPayment(IDictionary<string, string> parameters)
//     {
//         // Demo đơn giản: nếu có token (orderId) thì coi là OK
//         return Task.FromResult(parameters.ContainsKey("token"));
//     }
// }
