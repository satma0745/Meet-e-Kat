using System;
using System.IO;
using System.Reflection;
using Meetekat.WebApi.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
