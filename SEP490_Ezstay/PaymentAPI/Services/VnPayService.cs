// // using System.Security.Cryptography;
// // using System.Text;
// // using PaymentAPI.Services.Interfaces;
// //
// // namespace PaymentAPI.Services;
// //
// // public class VnPayService : IPaymentService
// // {
// //     private readonly string _vnpUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
// //     private readonly string _vnpTmnCode = "PWCW3F77"; 
// //     private readonly string _vnpHashSecret = "FUYB1ZLQ8R1KCU5GB5R4PKZPI9U6GB5N"; 
// //
// //     public Task<string> CreatePaymentUrl(Guid id, decimal amount)
// //     {
// //         var query = new SortedDictionary<string, string>
// //         {
// //             { "vnp_Version", "2.1.0" },
// //             { "vnp_Command", "pay" },
// //             { "vnp_TmnCode", _vnpTmnCode },
// //             { "vnp_Amount", ((long)amount * 100).ToString() },
// //             { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
// //             { "vnp_CurrCode", "VND" },
// //             { "vnp_IpAddr", "127.0.0.1" },
// //             { "vnp_Locale", "vn" },
// //             { "vnp_OrderInfo", $"Thanh toan don hang {id}" },
// //             { "vnp_OrderType", "other" },
// //             { "vnp_ReturnUrl", "https://localhost:7212/api/payment/vnpay/callback" },
// //             { "vnp_TxnRef", id.ToString("N").Substring(0, 10) } // chỉ 10-20 ký tự
// //         };
// //
// //         // rawData để ký (chưa encode value)
// //         string rawData = string.Join("&", query.Select(kv => kv.Key + "=" + kv.Value));
// //
// //         // tính hash
// //         string secureHash = CreateHmac512(_vnpHashSecret, rawData);
// //
// //         // build url (encode value)
// //         string queryUrl = string.Join("&", query.Select(kv => kv.Key + "=" + Uri.EscapeDataString(kv.Value)));
// //         string paymentUrl = _vnpUrl + "?" + queryUrl + "&vnp_SecureHash=" + secureHash;
// //
// //         return Task.FromResult(paymentUrl);
// //     }
// //
// //     public Task<bool> VerifyPayment(IDictionary<string, string> parameters)
// //     {
// //         if (!parameters.TryGetValue("vnp_SecureHash", out string receivedHash))
// //             return Task.FromResult(false);
// //
// //         // bỏ SecureHash
// //         var sorted = parameters
// //             .Where(p => p.Key != "vnp_SecureHash")
// //             .OrderBy(p => p.Key);
// //
// //         string rawData = string.Join("&", sorted.Select(x => $"{x.Key}={x.Value}"));
// //         string checkHash = CreateHmac512(_vnpHashSecret, rawData);
// //
// //         return Task.FromResult(checkHash.Equals(receivedHash, StringComparison.OrdinalIgnoreCase));
// //     }
// //
// //     private string CreateHmac512(string key, string input)
// //     {
// //         var keyBytes = Encoding.UTF8.GetBytes(key);
// //         using var hmac = new HMACSHA512(keyBytes);
// //         var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
// //         return BitConverter.ToString(hashValue).Replace("-", "").Replace(" ", "").ToLower();
// //     }
// // }
// //
// //
// // //
// // // using System.Security.Cryptography;
// // // using System.Text;
// // // using System.Web;
// // // using PaymentAPI.Services.Interfaces;
// // //
// // // namespace PaymentAPI.Services;
// // //
// // // public class VnPayService : IPaymentService
// // // {
// // //     private readonly string _vnpUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
// // //     private readonly string _vnpTmnCode = "PWCW3F77";
// // //     private readonly string _vnpHashSecret = "FUYB1ZLQ8R1KCU5GB5R4PKZPI9U6GB5N";
// // //
// // //     public Task<string> CreatePaymentUrl(Guid id, decimal amount)
// // //     {
// // //         var query = new SortedList<string, string>
// // //         {
// // //             { "vnp_Version", "2.1.0" },
// // //             { "vnp_Command", "pay" },
// // //             { "vnp_TmnCode", _vnpTmnCode },
// // //             { "vnp_Amount", ((long)amount * 100).ToString() },
// // //             { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
// // //             { "vnp_CurrCode", "VND" },
// // //             { "vnp_IpAddr", "127.0.0.1" },
// // //             { "vnp_Locale", "vn" },
// // //             { "vnp_OrderInfo", $"Thanh toan don hang {id}" },
// // //             { "vnp_OrderType", "other" },
// // //             { "vnp_ReturnUrl","https://localhost:7212/api/payment/vnpay/callback" },
// // //             { "vnp_TxnRef", id.ToString("N").Substring(0, 10)  }
// // //         };
// // //
// // //         // 1. Raw data để ký (không encode)
// // //         string rawData = string.Join("&", query.Select(kv => $"{kv.Key}={kv.Value}"));
// // //
// // //         // 2. Hash
// // //         string secureHash = CreateHmac512(_vnpHashSecret, rawData);
// // //
// // //         // 3. Build query string với encode
// // //         string queryUrl = string.Join("&", query
// // //             .Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
// // //
// // //         string paymentUrl = _vnpUrl + "?" + queryUrl + "&vnp_SecureHash=" + secureHash;
// // //
// // //         return Task.FromResult(paymentUrl);
// // //     }
// // //
// // //
// // //     public Task<bool> VerifyPayment(IDictionary<string, string> parameters)
// // //     {
// // //         if (!parameters.TryGetValue("vnp_SecureHash", out string receivedHash))
// // //             return Task.FromResult(false);
// // //
// // //         var sorted = parameters
// // //             .Where(p => p.Key != "vnp_SecureHash")
// // //             .OrderBy(p => p.Key);
// // //
// // //         string rawData = string.Join("&", sorted.Select(x => $"{x.Key}={x.Value}"));
// // //         string checkHash = CreateHmac512(_vnpHashSecret, rawData);
// // //
// // //         return Task.FromResult(checkHash.Equals(receivedHash, StringComparison.OrdinalIgnoreCase));
// // //     }
// // //
// // //     private string CreateHmac512(string key, string input)
// // //     {
// // //         var keyBytes = Encoding.UTF8.GetBytes(key);
// // //         using var hmac = new HMACSHA512(keyBytes);
// // //         var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
// // //         return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
// // //     }
// // // }
//
//
//
// using System.Security.Cryptography;
// using System.Text;
// using PaymentAPI.Services.Interfaces;
//
// namespace PaymentAPI.Services;
//
// public class VnPayService : IPaymentService
// {
//     private readonly string _vnpUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
//     private readonly string _vnpTmnCode = "PWCW3F77";
//     private readonly string _vnpHashSecret = "FUYB1ZLQ8R1KCU5GB5R4PKZPI9U6GB5N";
//
//     public Task<string> CreatePaymentUrl(Guid id, decimal amount)
//     {
//         var query = new SortedList<string, string>
//         {
//             { "vnp_Version", "2.1.0" },
//             { "vnp_Command", "pay" },
//             { "vnp_TmnCode", _vnpTmnCode },
//             { "vnp_Amount", ((long)amount * 100).ToString() },
//             { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
//             { "vnp_CurrCode", "VND" },
//             { "vnp_IpAddr", "127.0.0.1" },
//             { "vnp_Locale", "vn" },
//             { "vnp_OrderInfo", $"Thanh toan don hang {id}" },
//             { "vnp_OrderType", "other" },
//             // 👇 Gán cứng callback giống controller
//             { "vnp_ReturnUrl","https://localhost:7212/api/payment/vnpay/callback" },
//             { "vnp_TxnRef", id.ToString("N").Substring(0, 10)  }
//         };
//
//         // 1. Raw data để ký (không encode)
//         string rawData = string.Join("&", query.Select(kv => $"{kv.Key}={kv.Value}"));
//
//         // 2. Hash
//         string secureHash = CreateHmac512(_vnpHashSecret, rawData);
//
//         // 3. Build query string với encode
//         string queryUrl = string.Join("&", query
//             .Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
//
//         string paymentUrl = _vnpUrl + "?" + queryUrl + "&vnp_SecureHash=" + secureHash;
//
//         return Task.FromResult(paymentUrl);
//     }
//
//     public Task<bool> VerifyPayment(IDictionary<string, string> parameters)
//     {
//         if (!parameters.TryGetValue("vnp_SecureHash", out string receivedHash))
//             return Task.FromResult(false);
//
//         var sorted = parameters
//             .Where(p => p.Key != "vnp_SecureHash")
//             .OrderBy(p => p.Key);
//
//         string rawData = string.Join("&", sorted.Select(x => $"{x.Key}={x.Value}"));
//         string checkHash = CreateHmac512(_vnpHashSecret, rawData);
//
//         return Task.FromResult(checkHash.Equals(receivedHash, StringComparison.OrdinalIgnoreCase));
//     }
//
//     private string CreateHmac512(string key, string input)
//     {
//         var keyBytes = Encoding.UTF8.GetBytes(key);
//         using var hmac = new HMACSHA512(keyBytes);
//         var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
//         return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
//     }
// }


using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using PaymentAPI.Config;
using PaymentAPI.Services.Interfaces;

namespace PaymentAPI.Services
{
    public class VnPayService : IPaymentService
    {
        private readonly VnPayConfig _config;

        public VnPayService(IOptions<VnPayConfig> config)
        {
            _config = config.Value;
        }

        public Task<string> CreatePaymentUrl(Guid id, decimal amount)
        {
            var vnpParams = new SortedDictionary<string, string>
            {
                { "vnp_Version", _config.Version },
                { "vnp_Command", _config.Command },
                { "vnp_TmnCode", _config.TmnCode },
                { "vnp_Amount", ((long)amount * 100).ToString() },
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", id.ToString("N").Substring(0, 10) },
                { "vnp_OrderInfo", $"Thanh toan don hang {id}" },
                { "vnp_OrderType", _config.OrderType },
                { "vnp_Locale", "vn" },
                { "vnp_ReturnUrl", _config.ReturnUrl },
                { "vnp_IpAddr", "127.0.0.1" },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }
            };

            // build query string
            string queryString = string.Join("&", vnpParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

            // raw data để ký
            string rawData = string.Join("&", vnpParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            // hash
            string secureHash = CreateHmac512(_config.SecretKey, rawData);

            string paymentUrl = $"{_config.Url}?{queryString}&vnp_SecureHash={secureHash}";

            return Task.FromResult(paymentUrl);
        }

        public Task<bool> VerifyPayment(IDictionary<string, string> parameters)
        {
            if (!parameters.TryGetValue("vnp_SecureHash", out string receivedHash))
                return Task.FromResult(false);

            var sorted = parameters
                .Where(p => p.Key != "vnp_SecureHash")
                .OrderBy(p => p.Key);

            string rawData = string.Join("&", sorted.Select(x => $"{x.Key}={x.Value}"));
            string checkHash = CreateHmac512(_config.SecretKey, rawData);

            return Task.FromResult(checkHash.Equals(receivedHash, StringComparison.OrdinalIgnoreCase));
        }

        private string CreateHmac512(string key, string input)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            using var hmac = new HMACSHA512(keyBytes);
            var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        }
    }
}


