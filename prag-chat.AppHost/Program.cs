var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo");
var mongodb = mongo.AddDatabase("mongodb");

var qdrant = builder.AddQdrant("qdrant");

var apiService = builder
    .AddProject<Projects.prag_chat_ApiService>("apiservice")
    .WithReference(qdrant)
    .WithReference(mongo);

var frontend = builder.AddProject<Projects.prag_chat_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
