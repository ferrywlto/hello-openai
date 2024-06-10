// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using OpenAI.Images;

var builder = new ConfigurationBuilder();
// builder.AddEnvironmentVariables();
builder.AddUserSecrets<Program>();
var config = builder.Build();
var devKey = config["openai-dev-key"]!;

Console.WriteLine($"Hello, OpenAI! Your dev key is {devKey}");

// ChatClient client = new(model: "gpt-4o", devKey);
// ChatCompletion chatCompletion = await client.CompleteChatAsync(
//     [
//         new UserChatMessage("Try to explain Moe maid culture visually."),
//     ]);

// foreach(var c in chatCompletion.Content)
// {

// Console.WriteLine(
//     $"""
//     {c.Text}
//     """
// );
// }

ImageClient imageClient = new("dall-e-2", new System.ClientModel.ApiKeyCredential(devKey));
// var image = await imageClient.GenerateImageAsync(
//     """
//     On a shinely afternoon, a pretty Hong Kong girl standing on a street in Mong Kok with long black hair wearing black and white Moe style maid uniform, shinely Mary Jane shoes, 
//     while lace over kneel stockings. Facing viewer with warm smile, waving right hands to viewer. Damn this is a concept art for a Maid cafe advertisement.
//     """, new ImageGenerationOptions { 
//         Quality = GeneratedImageQuality.High, 
//         ResponseFormat = GeneratedImageFormat.Uri, 
//         Size = new GeneratedImageSize(1024, 1792), 
//         Style = GeneratedImageStyle.Natural 
//     });
var image = await imageClient.GenerateImageVariationAsync("generated-image.png", new ImageVariationOptions() {
         ResponseFormat = GeneratedImageFormat.Uri,
          Size = new GeneratedImageSize(1024,1024),
           User = "system"
    });
Console.WriteLine(image.Value.ImageUri);