using FavoritePostAPI.Data;
using FavoritePostAPI.Repository;
using FavoritePostAPI.Repository.Interface;
using FavoritePostAPI.Service;
using FavoritePostAPI.Service.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FavoritePostAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Bind MongoSettings từ appsettings.json
            builder.Services.Configure<MongoSettings>(
                builder.Configuration.GetSection("MongoSettings"));

            // Đăng ký MongoClient
            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            // Đăng ký IMongoDatabase
            builder.Services.AddSingleton<IMongoDatabase>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.DatabaseName);
            });

            // Các service khác
            builder.Services.AddScoped<IFavoritePostRepository, FavoritePostRepository>();
            builder.Services.AddScoped<IFavoritePostService, FavoritePostService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add controllers & swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
