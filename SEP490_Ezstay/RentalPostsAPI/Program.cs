

using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using RentalPostsAPI.Data;
using RentalPostsAPI.DTO.Request;
using RentalPostsAPI.Repository.Interface;
using RentalPostsAPI.Repository;
using RentalPostsAPI.Service.Interface;
using RentalPostsAPI.Service;

namespace RentalPostsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<MongoSettings>(
            builder.Configuration.GetSection("MongoSettings"));
            builder.Services.AddSingleton<MongoDbService>();
            builder.Services.AddScoped<IRentalPostRepository, RentalPostRepository>();
            builder.Services.AddScoped<IRentalPostService, RentalPostService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
          //  builder.Services.AddControllers();
           // builder.Services.AddControllers()
           //     .AddOData(opt =>
           //     {
           //         opt.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100);
           //         var edmBuilder = new ODataConventionModelBuilder();
           //         edmBuilder.EntitySet<RentalpostDTO>("RentalPostsOdata"); 
           //
           //         opt.AddRouteComponents("odata", edmBuilder.GetEdmModel());
           //     });
           builder.Services.AddControllers()
               .AddOData(opt =>
               {
                   opt.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100);
                   var edmBuilder = new ODataConventionModelBuilder();
                   edmBuilder.EntitySet<RentalpostDTO>("RentalPostsOdata"); 
           
                   opt.AddRouteComponents("odata", edmBuilder.GetEdmModel());
               });
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
