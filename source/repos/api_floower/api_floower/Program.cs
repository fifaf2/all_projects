using api_floower.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
       
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddOpenApi();
builder.Services.AddDbContext<FlooresContext>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Цветочный магазин");
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
