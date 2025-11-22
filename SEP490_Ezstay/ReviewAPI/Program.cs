using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using ReviewAPI.APIs;
using ReviewAPI.APIs.Interfaces;
using ReviewAPI.DTO.Response.ReviewReply;
using ReviewAPI.Profiles;
using ReviewAPI.Repository;
using ReviewAPI.Repository.Interface;
using ReviewAPI.Service;
using ReviewAPI.Service.Interface;
using Shared.DTOs.Reviews.Responses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var mongoClient = new MongoClient(builder.Configuration["ConnectionStrings:ConnectionString"]);
builder.Services.AddSingleton( mongoClient.GetDatabase(builder.Configuration["ConnectionStrings:DatabaseName"]));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IReviewReplyService, ReviewReplyService>();
builder.Services.AddScoped<IReviewReplyRepository, ReviewReplyRepository>();
builder.Services.AddHttpClient<IImageService, ImageService >(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ImageApi"]); 
});
// var serviceUrls = builder.Configuration.GetSection("ServiceUrls");
//
// builder.Services.AddHttpClient<IAmenityClientService, AmenityClientService>(client =>
// {
//     client.BaseAddress = new Uri(serviceUrls["AmenityApi"]);
// });
//
// builder.Services.AddHttpClient<IRoomAmenityClientService, RoomAmenityClientService>(client =>
// {
//     client.BaseAddress = new Uri(serviceUrls["RoomAmenityApi"]);
// });

builder.Services.AddHttpClient<IContractService, ContractService>(client =>
{ 
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ContractApi"]);
});
builder.Services.AddHttpClient<IRentalPostService, RentalPostService>(client =>
{ 
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:RentalPostApi"]);
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var odatabuilder = new ODataConventionModelBuilder();
odatabuilder.EntitySet<ReviewResponse>("Review");
odatabuilder.EntitySet<ReviewReplyResponse>("ReviewReplys");
var odata = odatabuilder.GetEdmModel();
builder.Services.AddControllers().AddOData(options =>
    options.AddRouteComponents("odata", odata)
        .SetMaxTop(100)
        .Count()
        .Filter()
        .OrderBy()
        .Expand()
        .Select());
builder.Services.AddAutoMapper(typeof(ReviewProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ReviewReplyResponse).Assembly);

var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });



            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReviewAPI", Version = "v1" });

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
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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
