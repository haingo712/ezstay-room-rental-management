
using CloudinaryDotNet;
using ImageAPI;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using ImageAPI.Repository.Interface;
using ImageAPI.Repository;
using ImageAPI.Service.Interface;
using ImageAPI.Service;
using RentalPostsAPI.Data;
using Amazon.S3;

namespace ImageAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<MongoSettings>(
           builder.Configuration.GetSection("MongoSettings"));
            builder.Services.AddSingleton<MongoDbService>();
            builder.Services.Configure<FilebaseSettings>(
    builder.Configuration.GetSection("FilebaseSettings"));

            var filebaseSettings = builder.Configuration
     .GetSection("FilebaseSettings")
     .Get<FilebaseSettings>();

          

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            builder.Services.AddSingleton<ImageService>();
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
