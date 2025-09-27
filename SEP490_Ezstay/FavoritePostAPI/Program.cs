
using FavoritePostAPI.Data;
using FavoritePostAPI.Repository;
using FavoritePostAPI.Repository.Interface;
using FavoritePostAPI.Service;
using FavoritePostAPI.Service.Interface;

namespace FavoritePostAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<MongoSettings>(
           builder.Configuration.GetSection("MongoSettings"));
            builder.Services.AddSingleton<MongoDbService>();
            builder.Services.AddScoped<IFavoritePostRepository, FavoritePostRepository>();
            builder.Services.AddScoped<IFavoritePostService, FavoritePostService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
