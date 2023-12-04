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

////Adds DALL·E service and configure options via code.
//builder.Services.AddDallE(options =>
//{
//    // Azure OpenAI Service.
//    options.UseAzure(resourceName: "", apiKey: "", authenticationType: AzureAuthenticationType.ApiKey);

//    options.DefaultSize = DallEImageSizes._1792x1024;              // Default: 1024x1024
//    options.DefaultQuality = DallEImageQualities.HD;               // Default: Standard
//    options.DefaultStyle = DallEImageStyles.Natural;               // Default: Vivid
//    options.DefaultResponseFormat = DallEImageResponseFormats.Url; // Default: Url
//});

////Adds DALL·E service using settings from IConfiguration and code.
//builder.Services.AddDallE(options =>
//{
//    options.UseConfiguration(builder.Configuration);

//    options.UseAzure(resourceName: "", apiKey: "", authenticationType: AzureAuthenticationType.ApiKey);
//    options.DefaultSize = DallEImageSizes._1792x1024;     // Default: 1024x1024
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

app.MapPost("/api/image", async (Request request, IDallEClient dallEClient, string? size = DallEImageSizes._1024x1024, string? quality = DallEImageQualities.Standard, string? style = DallEImageStyles.Vivid) =>
{
    var response = await dallEClient.GenerateImagesAsync(request.Prompt, size, quality, style);
    return TypedResults.Ok(response);
})
.WithOpenApi();

app.MapPost("/api/image-content", async (Request request, IDallEClient dallEClient, string? size = DallEImageSizes._1024x1024, string? quality = DallEImageQualities.Standard, string? style = DallEImageStyles.Vivid) =>
{
    var imageStream = await dallEClient.GetImageStreamAsync(request.Prompt, size, quality, style);
    return TypedResults.Stream(imageStream, "image/png");
})
.WithOpenApi();

app.Run();

public record class Request(string Prompt);
