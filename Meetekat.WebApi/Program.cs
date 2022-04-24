using Meetekat.WebApi.Auth;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Seedwork.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
