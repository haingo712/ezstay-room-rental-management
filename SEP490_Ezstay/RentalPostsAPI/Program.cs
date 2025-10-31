

using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using RentalPostsAPI.Data;
using RentalPostsAPI.DTO.Request;
using RentalPostsAPI.Repository.Interface;
using RentalPostsAPI.Repository;
using RentalPostsAPI.Service.Interface;
using RentalPostsAPI.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

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
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.Configure<ExternalServiceSettings>(
    builder.Configuration.GetSection("ExternalServices"));
            builder.Services.AddHttpClient<ExternalService>();

            builder.Services.AddHttpClient<IExternalService, ExternalService>();
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
                    edmBuilder.EntitySet<RentalpostDTO>("RentalPosts");
                    opt.AddRouteComponents("odata", edmBuilder.GetEdmModel());
                });
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddHttpClient("Gateway", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7000");
            });

          

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RentalPostAPI", Version = "v1" });

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
