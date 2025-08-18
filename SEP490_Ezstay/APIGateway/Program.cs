using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.          

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add Ocelot service
builder.Services.AddOcelot();

var app = builder.Build();            

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();                        

await app.UseOcelot();

app.Run();