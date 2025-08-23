using APIGateway.Utils;
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
app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

    if (!string.IsNullOrEmpty(token))
    {
        var jwtUtils = new JwtUtils("thisIsYourSuperSecretKey123456456"); // key gi?ng trong AuthApi appsettings
        var principal = jwtUtils.ValidateToken(token);

        if (principal != null)
        {
            context.User = principal; // ? gán user vào HttpContext ?? ti?p t?c dùng
        }
        else
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid or expired token");
            return;
        }
    }

    await next();
});

await app.UseOcelot();

app.Run();