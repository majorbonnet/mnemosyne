using MnemosyneDomain;

var builder = WebApplication.CreateBuilder(args);

builder.AddMnemosyneDomainServices();

var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.Run();
