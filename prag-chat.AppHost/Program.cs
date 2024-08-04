var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.prag_chat_ApiService>("apiservice");

builder.AddProject<Projects.prag_chat_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
