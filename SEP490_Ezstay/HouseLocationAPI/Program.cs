
using HouseLocationAPI.Data;
using HouseLocationAPI.DTO.Request;
using HouseLocationAPI.Repository;
using HouseLocationAPI.Repository.Interface;
using HouseLocationAPI.Service;
using HouseLocationAPI.Service.Interface;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;

namespace HouseLocationAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.Configure<MongoSettings>(
            builder.Configuration.GetSection("ConnectionStrings"));
            builder.Services.AddSingleton<MongoDbService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<IHouseLocationRepository, HouseLocationRepository>();
            builder.Services.AddScoped<IHouseLocationService, HouseLocationService>();

            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<HouseLocationDTO>("HouseLocations");

            builder.Services.AddControllers().AddOData(options => options
                .AddRouteComponents("odata", odataBuilder.GetEdmModel())
                .SetMaxTop(100)
                .Count()
                .Filter()
                .OrderBy()
                .Expand()
                .Select());

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
