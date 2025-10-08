using SignalRHubAPI.Hub;
using EasyNetQ;
using SignalRHubAPI.Listener;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyHeader()
                  .AllowAnyMethod()
                  .SetIsOriginAllowed(_ => true) // Cho phép mọi domain
                  .AllowCredentials(); // Cho phép gửi cookie, signalR credentials
        });
});

builder.Services.AddSingleton(RabbitHutch.CreateBus(
    builder.Configuration["RabbitMQ:ConnectionString"]
));

builder.Services.AddHostedService<RabbitMqListener>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapHub<NotificationHub>("/hubs/notification");

app.MapControllers();

app.Run();