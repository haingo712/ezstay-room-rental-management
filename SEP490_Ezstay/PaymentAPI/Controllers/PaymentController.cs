// // // using Microsoft.AspNetCore.Mvc;
// // // using PaymentAPI.Services.Interfaces;
// // //
// // // namespace PaymentAPI.Controllers;
// // //
// // // [ApiController]
// // // [Route("api/[controller]")]
// // // public class PaymentController : ControllerBase
// // // {
// // //     private readonly Dictionary<string, IPaymentService> _services;
// // //
// // //     public PaymentController(IEnumerable<IPaymentService> paymentServices)
// // //     {
// // //         _services = new Dictionary<string, IPaymentService>();
// // //
// // //         foreach (var service in paymentServices)
// // //         {
// // //             // key = tên provider (vd: vnpay, momo, paypal)
// // //             var key = service.GetType().Name.Replace("Service", "").ToLower();
// // //             _services[key] = service;
// // //         }
// // //     }
// // //     [HttpGet("callback")]
// // //     public async Task<IActionResult> Callback()
// // //     {
// // //         if (!_services.ContainsKey("vnpay"))
// // //             return BadRequest("Provider not supported");
// // //
// // //         var parameters = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());
// // //         bool isValid = await _services["vnpay"].VerifyPayment(parameters);
// // //
// // //         return isValid ? Ok("Payment success") : BadRequest("Payment failed");
// // //     }
// // //     [HttpPost("method/{provider}")]
// // //     public async Task<IActionResult> CreatePayment(string provider, [FromBody] decimal amount)
// // //     {
// // //         if (!_services.ContainsKey(provider))
// // //             return BadRequest("Provider not supported");
// // //     
// // //         var orderId = Guid.NewGuid();
// // //         var url = await _services[provider].CreatePaymentUrl(orderId, amount);
// // //     
// // //         return Ok(new { orderId, url });
// // //     }
// // //     // [HttpPost("method/{provider}/amount/{amount}")]
// // //     // public async Task<IActionResult> CreatePayment(string provider, decimal amount)
// // //     // {
// // //     //     if (!_services.ContainsKey(provider))
// // //     //         return BadRequest("Provider not supported");
// // //     //
// // //     //     var id = Guid.NewGuid();
// // //     //     var url = await _services[provider].CreatePaymentUrl(id, amount);
// // //     //
// // //     //     return Ok(new { id, url });
// // //     // }
// // //
// // //     // [HttpGet("{provider}/callback")]
// // //     // public async Task<IActionResult> Callback(string provider)
// // //     // {
// // //     //     if (!_services.ContainsKey(provider))
// // //     //         return BadRequest("Provider not supported");
// // //     //
// // //     //     var parameters = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());
// // //     //     bool isValid = await _services[provider].VerifyPayment(parameters);
// // //     //
// // //     //     return isValid ? Ok("Payment success") : BadRequest("Payment failed");
// // //     // }
// // // }
// //
// // using Microsoft.AspNetCore.Mvc;
// // using PaymentAPI.Services.Interfaces;
// //
// // namespace PaymentAPI.Controllers;
// //
// // [ApiController]
// // [Route("api/[controller]")]
// // public class PaymentController : ControllerBase
// // {
// //     private readonly Dictionary<string, IPaymentService> _services;
// //
// //     public PaymentController(IEnumerable<IPaymentService> paymentServices)
// //     {
// //         _services = new Dictionary<string, IPaymentService>();
// //
// //         foreach (var service in paymentServices)
// //         {
// //             // key = tên provider (vd: vnpay, momo, paypal)
// //             var key = service.GetType().Name.Replace("Service", "").ToLower();
// //             _services[key] = service;
// //         }
// //     }
// //
// //     /// <summary>
// //     /// Tạo URL thanh toán
// //     /// </summary>
// //     [HttpPost("method/{provider}")]
// //     public async Task<IActionResult> CreatePayment(string provider, [FromBody] decimal amount)
// //     {
// //         if (!_services.ContainsKey(provider))
// //             return BadRequest("Provider not supported");
// //
// //         var orderId = Guid.NewGuid();
// //         var url = await _services[provider].CreatePaymentUrl(orderId, amount);
// //
// //         return Ok(new
// //         {
// //             orderId,
// //             amount,
// //             paymentUrl = url
// //         });
// //     }
// //
// //     /// <summary>
// //     /// Callback từ cổng thanh toán
// //     /// </summary>
// //     [HttpGet("{provider}/callback")]
// //     public async Task<IActionResult> Callback(string provider)
// //     {
// //         if (!_services.ContainsKey(provider))
// //             return BadRequest("Provider not supported");
// //
// //         var parameters = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());
// //
// //         bool isValid = await _services[provider].VerifyPayment(parameters);
// //
// //         if (isValid)
// //         {
// //             // log ra để debug
// //             Console.WriteLine("✅ Thanh toán thành công");
// //             return Ok(new
// //             {
// //                 status = "success",
// //                 message = "Thanh toán thành công",
// //                 query = parameters
// //             });
// //         }
// //         else
// //         {
// //             Console.WriteLine("❌ Sai chữ ký");
// //             return BadRequest(new
// //             {
// //                 status = "failed",
// //                 message = "Sai chữ ký hoặc thanh toán thất bại",
// //                 query = parameters
// //             });
// //         }
// //     }
// // }
//
//
// using Microsoft.AspNetCore.Mvc;
// using PaymentAPI.Services;
// using PaymentAPI.Services.Interfaces;
//
// namespace PaymentAPI.Controllers;
//
// [ApiController]
// [Route("api/[controller]")]
// public class PaymentController : ControllerBase
// {
//     private readonly IPaymentService _vnPayService;
//
//     public PaymentController(IPaymentService vnPayService)
//     {
//         _vnPayService = vnPayService;
//     }
//
//     // Tạo payment cho VNPAY
//     [HttpPost("vnpay")]
//     public async Task<IActionResult> CreatePayment([FromBody] decimal amount)
//     {
//         var orderId = Guid.NewGuid();
//         var url = await _vnPayService.CreatePaymentUrl(orderId, amount);
//
//         return Ok(new { orderId, url });
//     }
//
//     // Callback cứng cho VNPAY
//     [HttpGet("vnpay/callback")]
//     public async Task<IActionResult> VnPayCallback()
//     {
//         var parameters = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());
//         bool isValid = await _vnPayService.VerifyPayment(parameters);
//
//         return isValid ? Ok("Payment success") : BadRequest("Payment failed");
//     }
// }



using Microsoft.AspNetCore.Mvc;
using PaymentAPI.Services.Interfaces;

namespace PaymentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _vnPayService;

        public PaymentController(IPaymentService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("vnpay")]
        public async Task<IActionResult> CreatePayment([FromQuery] decimal amount)
        {
            var orderId = Guid.NewGuid();
            var url = await _vnPayService.CreatePaymentUrl(orderId, amount);
            return Ok(new { orderId, url });
        }

        [HttpGet("vnpay/callback")]
        public async Task<IActionResult> Callback()
        {
            var parameters = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());
            bool isValid = await _vnPayService.VerifyPayment(parameters);

            if (!parameters.TryGetValue("vnp_ResponseCode", out string responseCode))
                return BadRequest("Missing response code");

            if (isValid && responseCode == "00")
            {
                return Ok(new { status = "success", orderId = parameters["vnp_TxnRef"] });
            }
            return BadRequest(new { status = "failed", orderId = parameters["vnp_TxnRef"] });
        }
    }
}
