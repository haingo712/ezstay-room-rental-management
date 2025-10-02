﻿
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using UtilityBillAPI.Data;
using UtilityBillAPI.DTO.Request;
using UtilityBillAPI.HostedService; 
using UtilityBillAPI.Repository;
using UtilityBillAPI.Repository.Interface;
using UtilityBillAPI.Service;
using UtilityBillAPI.Service.Interface;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;

namespace UtilityBillAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("ConnectionStrings"));
            builder.Services.AddSingleton<MongoDbService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<IUtilityBillRepository, UtilityBillRepository>();
            builder.Services.AddScoped<IUtilityBillService, UtilityBillService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            var serviceUrls = builder.Configuration.GetSection("ServiceUrls");
            builder.Services.AddHttpClient<IRoomClient, RoomClient>(client =>
            {
                client.BaseAddress = new Uri(serviceUrls["RoomAPI"]!);
            });
            builder.Services.AddHttpClient<IUtilityReadingClient, UtilityReadingClient>(client =>
            {
                client.BaseAddress = new Uri(serviceUrls["UtilityReadingAPI"]!);
            });
           
            //builder.Services.AddHostedService<BillHostedService>();

            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<UtilityBillDTO>("UtilityBills");

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UtilityBillAPI", Version = "v1" });

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
