using System;
using System.Text;
using Meetekat.WebApi.Auth;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.UseTypeFullNameForDtoSchemas();
    options.IncludeXmlComments();
    options.IncludeSwaggerAnnotations();
    options.AddJwtSecurity();
});
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseInMemoryDatabase("In-Memory DB");
});
builder.Services.AddSingleton<JwtTokenService>();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // TODO: Refactor Auth configuration.

        const string secretKeyParameterName = "Auth:SecretKey";
        var secretKeyParameter = builder.Configuration[secretKeyParameterName];
        if (string.IsNullOrWhiteSpace(secretKeyParameter))
        {
            throw new Exception($"Configuration parameter \"{secretKeyParameterName}\" is required.");
        }
        var secretKeyBytes = Encoding.UTF8.GetBytes(secretKeyParameter);
        var secretKey = new SymmetricSecurityKey(secretKeyBytes);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = secretKey,
            
            ValidateAudience = false,
            ValidateIssuer = false,
            
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        options.RequireHttpsMetadata = false;
    });

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
