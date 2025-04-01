using System.Text;
using System.Text.Json;
using Serilog;
using Microsoft.AspNetCore.Builder; // Ensure middleware is recognized
using Microsoft.Extensions.DependencyInjection; // Ensure services are recognized

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog as the logging provider

// Add services to the container.
builder.Services.AddControllers(); // Add support for controllers
builder.Services.AddEndpointsApiExplorer(); // Enable API explorer for Swagger/OpenAPI
builder.Services.AddSwaggerGen(); // Add Swagger for API documentation
builder.Services.AddSingleton<UserManagementAPI.Services.InputValidationService>();

builder.Services.AddAuthorization(); // Add authorization services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger in development
    app.UseSwaggerUI(); // Enable Swagger UI
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization(); // Add authorization middleware

//? Custom middleware for API key validation
/* app.Use(async (context, next) =>
{
    const string apiKey = "TestKey123"; // Hardcoded API key
    var queryApiKey = context.Request.Query["apiKey"].ToString();

    if (string.IsNullOrWhiteSpace(queryApiKey)) // Handle null or empty API key
    {
        context.Response.StatusCode = 400; // Bad Request
        context.Response.ContentType = "application/json";
        var errorResponse = new { error = "Missing API key." };
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        return;
    }

    if (!string.Equals(queryApiKey, apiKey, StringComparison.Ordinal)) // Case-sensitive comparison
    {
        context.Response.StatusCode = 401; // Unauthorized
        context.Response.ContentType = "application/json";
        var errorResponse = new { error = "Unauthorized. Invalid API key." };
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        return;
    }

    await next(); // Proceed to the next middleware
}); */

// Custom middleware to log request and response details
app.Use(async (context, next) =>
{
    try
    {
        // Log the incoming request
        Log.Information("Incoming Request: {Method} {Path} {QueryString}", 
            context.Request.Method, 
            context.Request.Path, 
            context.Request.QueryString);

        await next(); // Proceed to the next middleware

        // Log the outgoing response
        Log.Information("Outgoing Response: {StatusCode}", context.Response.StatusCode);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred during request logging.");
        throw; // Re-throw the exception to be handled by the global exception handler
    }
});

// Global exception handling middleware
app.Use(async (context, next) =>
{
    try
    {
        await next(); // Proceed to the next middleware
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An unhandled exception occurred.");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500; // Internal Server Error

        var errorResponse = new ErrorResponse
        {
            Error = "Internal server error.",
            Details = app.Environment.IsDevelopment() ? ex.Message : null
        };

        var errorJson = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(errorJson);
    }
});

app.MapControllers(); // Map controller routes

app.Run();
