using ColorsCommon;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddSingleton<ColorsService>();

builder.EnableMcpToolMetadata();

var app = builder.Build();

var colorsService = app.Services.GetRequiredService<ColorsService>();

app.Run();


