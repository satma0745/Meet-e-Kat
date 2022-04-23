using Meetekat.WebApi.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<AnnotationsOperationFilter>();
});
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseInMemoryDatabase("In-Memory DB");
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
