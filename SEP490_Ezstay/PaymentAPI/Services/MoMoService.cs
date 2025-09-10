// using PaymentAPI.Services.Interfaces;
// using System.Text;
// using System.Security.Cryptography;
//
// namespace PaymentAPI.Services;
//
// public class MoMoService : IPaymentService
// {
//     private readonly string _endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
//     private readonly string _partnerCode = "MOMO_PARTNER_CODE";
//     private readonly string _accessKey = "MOMO_ACCESS_KEY";
//     private readonly string _secretKey = "MOMO_SECRET_KEY";
//
//     public async Task<string> CreatePaymentUrl(Guid orderId, decimal amount)
//     {
//         var requestId = Guid.NewGuid().ToString();
//         var orderIdStr = orderId.ToString();
//         var rawHash = $"accessKey={_accessKey}&amount={amount}&extraData=&ipnUrl=https://yourdomain.com/payment/momo/ipn&orderId={orderIdStr}&orderInfo=Thanh toan don hang {orderId}&partnerCode={_partnerCode}&redirectUrl=https://yourdomain.com/payment/momo/callback&requestId={requestId}&requestType=captureWallet";
//         var signature = SignSHA256(rawHash, _secretKey);
//
//         var body = new
//         {
//             partnerCode = _partnerCode,
//             requestId = requestId,
//             amount = amount.ToString(),
//             orderId = orderIdStr,
//             orderInfo = $"Thanh toan don hang {orderId}",
//             redirectUrl = "https://yourdomain.com/payment/momo/callback",
//             ipnUrl = "https://yourdomain.com/payment/momo/ipn",
//             requestType = "captureWallet",
//             extraData = "",
//             signature = signature
//         };
//
//         using var client = new HttpClient();
//         var response = await client.PostAsJsonAsync(_endpoint, body);
//         var json = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
//         return json!["payUrl"].ToString()!;
//     }
//
//     public Task<bool> VerifyPayment(IDictionary<string, string> parameters)
//     {
//         // Demo: chỉ kiểm tra có kết quả trả về
//         return Task.FromResult(parameters.ContainsKey("orderId") && parameters.ContainsKey("resultCode"));
//     }
//
//     private string SignSHA256(string data, string key)
//     {
//         var encoding = new UTF8Encoding();
//         byte[] keyByte = encoding.GetBytes(key);
//         byte[] messageBytes = encoding.GetBytes(data);
//         using var hmacsha256 = new HMACSHA256(keyByte);
//         byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
//         return BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
//     }
// }
