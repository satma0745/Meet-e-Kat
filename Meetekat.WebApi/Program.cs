using System;
using System.IO;
using System.Reflection;
using System.Text;
using Meetekat.WebApi.Auth;
using Meetekat.WebApi.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    // XML Documentation Comments integration
    var webApiProjectName = Assembly.GetExecutingAssembly().GetName().Name;
    var pathToXmlCommentsFile = Path.Combine(AppContext.BaseDirectory, $"{webApiProjectName}.xml");
    options.IncludeXmlComments(pathToXmlCommentsFile);

    // Swashbuckle.AspNetCore.Annotations integration
    options.OperationFilter<AnnotationsOperationFilter>();
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Put Your access token here:",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    options.OperationFilter<OpenApiSecurityRequirementFilter>();
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
