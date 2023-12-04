using System.Diagnostics;
using System.Text.Json.Serialization;
using DallENet;
using DallENet.Models;

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
builder.Services.AddSwaggerGen();

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

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = string.Empty;
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "DALL·E API v1");
});

app.MapPost("/api/image", async (Request request, IDallEClient dallEClient) =>
{
    var response = await dallEClient.GenerateImagesAsync(request.Prompt, request.Size, request.Quality, request.Style);
    return TypedResults.Ok(response);
})
.WithOpenApi();

app.MapPost("/api/image-content", async (Request request, IDallEClient dallEClient) =>
{
    var imageStream = await dallEClient.GetImageStreamAsync(request.Prompt, request.Size, request.Quality, request.Style);
    return TypedResults.Stream(imageStream, "image/png");
})
.WithOpenApi();

app.Run();

public record class Request(string Prompt, string? Size = DallEImageSizes._1024x1024, string? Quality = DallEImageQualities.Standard, string? Style = DallEImageStyles.Vivid);
