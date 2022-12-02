using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace RateLimiting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Concurrency")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        
        public IActionResult Get()
        {
            string message = "İşlem başarılı ";
            return Ok(message);
        }

        [HttpGet("[action]")]

        public async Task<IActionResult> GetAsync()
        {
            await Task.Delay(20000);
            string message = "İşlem başarılı Concurrency";
            return Ok(message);
        }
    }
}

//class CustomRateLimitPolicy : IRateLimiterPolicy<string>
//{
//    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => (context, cancellationToken) =>
//    {
//        return new();
//    };

//    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
//    {
//        return RateLimitPartition.GetFixedWindowLimiter("", _ => new()
//        {
//            PermitLimit=4,
//            Window=TimeSpan.FromSeconds(15),
//            QueueProcessingOrder=QueueProcessingOrder.OldestFirst,
//            QueueLimit=2
//        });
//    }
//}
