using BankingSecureApi.Middleware;
using BankingSecureApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();

// JWT Config
var key = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();

//Add API rate limiting to prevent abuse
builder.Services.AddRateLimiter(Options =>
{
    Options.AddFixedWindowLimiter("api", limiterOptions =>
    {
        //حداکثر درخواست در بازه زمانی
        limiterOptions.PermitLimit = 10;
        //بازه زمانی
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        //اگر تعداد درخواست زیاد شد چندتا در صف بمانند
        limiterOptions.QueueLimit = 10;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

//Add security headers middleware to mitigate common browser-based attacks
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";

    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
