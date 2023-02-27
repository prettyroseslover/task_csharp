using Microsoft.OpenApi.Models;
using TASK.Controllers;
using TASK.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "My API - V1",
            Version = "v1"
        }
    );

    var filePath = Path.Combine(System.AppContext.BaseDirectory, "XmlDoc.xml");
    c.IncludeXmlComments(filePath);
});

// builder.Services.AddSingleton<FileController>(_ => new ApplicationContext());
builder.Services.AddSingleton<ApplicationContext>();

var app = builder.Build();

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