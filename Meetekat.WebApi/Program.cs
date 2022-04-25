using Meetekat.WebApi.Auth.Configuration;
using Meetekat.WebApi.Persistence;
using Meetekat.WebApi.Swagger;
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
    var persistenceConfiguration = PersistenceConfiguration.FromApplicationConfiguration(builder.Configuration);
    options.UseNpgsql(persistenceConfiguration.ConnectionString);
});
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();
app.UseSwaggerDocs();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
