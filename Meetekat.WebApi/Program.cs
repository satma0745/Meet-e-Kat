using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    var webApiAssembly = Assembly.GetExecutingAssembly();
    var webApiProjectName = webApiAssembly.GetName().Name;

    var xmlCommentsFileName = $"{webApiProjectName}.xml";
    var xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFileName);
    
    options.IncludeXmlComments(xmlCommentsFilePath);
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
