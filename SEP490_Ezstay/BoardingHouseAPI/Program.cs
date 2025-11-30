
using BoardingHouseAPI.Data;
using BoardingHouseAPI.Repository.Interface;
using BoardingHouseAPI.Repository;
using BoardingHouseAPI.Service.Interface;
using BoardingHouseAPI.Service;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using BoardingHouseAPI.DTO.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using EasyNetQ; 

namespace BoardingHouseAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });
            builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("ConnectionStrings"));
            builder.Services.AddSingleton<MongoDbService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var serviceUrls = builder.Configuration.GetSection("ServiceUrls");
            builder.Services.AddHttpClient("Gateway", client =>
            {
                client.BaseAddress = new Uri(serviceUrls["Gateway"]!);
            });
            builder.Services.AddHttpClient<IImageService, ImageService>(client =>
            {
                client.BaseAddress = new Uri(serviceUrls["ImageAPI"]!);
            });                      
            builder.Services.AddHttpClient<IReviewService, ReviewService>(client =>
            {
                client.BaseAddress = new Uri(serviceUrls["ReviewAPI"]!);
            });
            builder.Services.AddHttpClient<ISentimentAnalysisService, SentimentAnalysisService>(client =>
            {
                client.BaseAddress = new Uri(serviceUrls["SentimentAnalysisAPI"]!);
            });
            builder.Services.AddHttpClient<IRoomService, RoomService>(client =>
            {
                client.BaseAddress = new Uri(serviceUrls["RoomAPI"]!);
            });
            builder.Services.AddHttpClient<IContractService, ContractService>(client =>
            {
                client.BaseAddress = new Uri(serviceUrls["ContractAPI"]!);
            });
            builder.Services.AddHttpClient<IRentalPostService, RentalPostService>(client =>
            {
                client.BaseAddress = new Uri(serviceUrls["RentalPostAPI"]!);
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<IBoardingHouseRepository, BoardingHouseRepository>();            
            builder.Services.AddScoped<IBoardingHouseService, BoardingHouseService>();
            /*builder.Services.AddHttpClient<IBoardingHouseService, BoardingHouseService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7000"); // Gateway
            });*/
            builder.Services.AddScoped<ITokenService, TokenService>();

            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<BoardingHouseDTO>("BoardingHouses");

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

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSettings = builder.Configuration.GetSection("Jwt");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),
                       
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BoardingHouseAPI", Version = "v1" });

                // Thêm JWT Security
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Nhập vào JWT token theo định dạng: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddSingleton(RabbitHutch.CreateBus(
                builder.Configuration["RabbitMQ:ConnectionString"]
            ));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
