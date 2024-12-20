using Cashflow.Domain;
using Cashflow.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();

builder.Services.AddDomain()
                .AddValidation();

builder.Services.AddRequestPipeline()
                .AddEventReaction()
                .AddDataAccess(builder.Configuration);

builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () =>
{
    return $"Running in Environment: {app.Environment.EnvironmentName}";
});

app.Run();
