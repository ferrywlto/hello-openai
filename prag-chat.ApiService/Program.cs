using Microsoft.AspNetCore.Mvc;
using Qdrant.Client;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.AddMongoDBClient("mongodb");
// Add services to the container.
builder.Services.AddProblemDetails();

builder.AddQdrantClient("qdrant");
builder.AddMongoDBClient("mongo");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});


app.MapGet("/q/collections", async (QdrantClient qdrant) =>
{
    var r = await qdrant.ListCollectionsAsync();
    foreach(var c in r)
    {
        Console.WriteLine($"collection: {c}");
    }
    return r;
});

app.MapPost("/q/collection/{name}", async (QdrantClient qdrant, string name) => {
    try
    {
        await qdrant.CreateCollectionAsync(name);
        return Results.Ok();
    }
    catch(Exception e)
    {
        return Results.Problem(e.Message);
    }
});
app.MapPost("/q/collection", async (QdrantClient qdrant, HttpRequest req) => {
    try
    {
        var body = await req.ReadFromJsonAsync<CreateCollectionRequest>();
        if(body is not null && !string.IsNullOrEmpty(body.Name))
        {
            await qdrant.CreateCollectionAsync(body.Name);
            return Results.Ok();
        }
        return Results.BadRequest("name missing");
    }
    catch(Exception e)
    {
        return Results.Problem(e.Message);
    }
});
app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
record CreateCollectionRequest(string Name);