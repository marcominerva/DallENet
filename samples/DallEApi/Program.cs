using System.Diagnostics;
using System.Text.Json.Serialization;
using DallENet;
using Microsoft.AspNetCore.Diagnostics;
using MinimalHelpers.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Adds DALL·E service using settings from IConfiguration.
builder.Services.AddDallE(builder.Configuration);

// Adds DALL·E service and configure options via code.
//services.AddDallE(options =>
//{
//    // Azure OpenAI Service.
//    //options.UseAzure(resourceName: "", apiKey: "", authenticationType: AzureAuthenticationType.ApiKey);

//    options.DefaultResolution = DallEImageResolutions.Medium;     // Default: Large (1024x1024)
//    options.DefaultImageCount = 2;  // Default: 1
//});

// Adds DALL·E service using settings from IConfiguration and code.
//services.AddDallE(options =>
//{
//    options.UseConfiguration(context.Configuration);

//    options.UseAzure(resourceName: "", apiKey: "", authenticationType: AzureAuthenticationType.ApiKey);
//    options.DefaultResolution = DallEImageResolutions.Medium; // Default: Large (1024x1024)
//    options.DefaultImageCount = 2;  // Default: 1
//});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddMissingSchemas();
});

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions["traceId"] = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
    };
});

var app = builder.Build();

// Configures the HTTP request pipeline.
app.UseHttpsRedirection();

if (!app.Environment.IsDevelopment())
{
    // Error handling
    app.UseExceptionHandler(new ExceptionHandlerOptions
    {
        AllowStatusCode404Response = true,
        ExceptionHandler = async (HttpContext context) =>
        {
            var problemDetailsService = context.RequestServices.GetRequiredService<IProblemDetailsService>();
            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
            var error = exceptionHandlerFeature?.Error;

            // Writes as JSON problem details
            await problemDetailsService.WriteAsync(new()
            {
                HttpContext = context,
                AdditionalMetadata = exceptionHandlerFeature?.Endpoint?.Metadata,
                ProblemDetails =
                {
                    Status = context.Response.StatusCode,
                    Title = error?.GetType().FullName ?? "An error occurred while processing your request",
                    Detail = error?.Message
                }
            });
        }
    });
}

app.UseStatusCodePages();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = string.Empty;
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "DALL·E API v1");
});

app.MapPost("/api/image", async (Request request, IDallEClient dallEClient) =>
{
    var response = await dallEClient.GenerateImagesAsync(request.Prompt);
    return TypedResults.Ok(response);
})
.WithOpenApi();

app.MapPost("/api/image-content", async (Request request, IDallEClient dallEClient) =>
{
    var imageStream = await dallEClient.GetImageStreamAsync(request.Prompt);
    return TypedResults.Stream(imageStream, "image/png");
})
.WithOpenApi();

app.Run();

public record class Request(string Prompt);
