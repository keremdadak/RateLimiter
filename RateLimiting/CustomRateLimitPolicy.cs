using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace RateLimiting
{
    public class CustomRateLimitPolicy : IRateLimiterPolicy<string>
    {
        public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => (context, cancellationToken) =>
        {
            return new();
        };

        public RateLimitPartition<string> GetPartition(HttpContext httpContext)
        {
            return RateLimitPartition.GetFixedWindowLimiter("", _ => new()
            {
                PermitLimit =3,
                Window=TimeSpan.FromSeconds(12),
                QueueLimit=2,
                QueueProcessingOrder=QueueProcessingOrder.OldestFirst
            }); ;
        }
    }
}
