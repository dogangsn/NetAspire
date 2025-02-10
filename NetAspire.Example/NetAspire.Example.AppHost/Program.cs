var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ServiceA>("servicea");
 
builder.AddProject<Projects.ServiceB>("serviceb");
 
builder.Build().Run();
