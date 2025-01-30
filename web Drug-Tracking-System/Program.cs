using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using DrugSystem.Services;
using DrugSystem.Models;
using DrugSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register custom services
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<DrugService>();
builder.Services.AddScoped<PharmacyService>();

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
