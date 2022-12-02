using Microsoft.AspNetCore.RateLimiting;
using RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy<string, CustomRateLimitPolicy>("Custom");


    options.AddFixedWindowLimiter("Fixed", _options =>
    {
        _options.PermitLimit = 3;
        _options.Window = TimeSpan.FromSeconds(20);
        _options.QueueLimit = 2;
        _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
    options.OnRejected = (context, cancellationToken) =>
    {
        return new();
    };

    options.AddSlidingWindowLimiter("Sliding", _options =>
    {
        _options.Window = TimeSpan.FromSeconds(10);
        _options.PermitLimit = 4;
        _options.QueueLimit = 1;
        _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        _options.SegmentsPerWindow = 2;
    });
    options.AddTokenBucketLimiter("TokenBucket", _options =>
    {
        _options.TokenLimit = 5;
        _options.TokensPerPeriod = 5;
        _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        _options.QueueLimit = 3;
        _options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);


    });
    options.AddConcurrencyLimiter("Concurrency", _options =>
    {
        _options.PermitLimit = 3;
        _options.QueueLimit = 1;
        _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});
var app = builder.Build();

app.UseRateLimiter();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
