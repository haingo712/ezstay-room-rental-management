using System.Text;
using System.Text.Json.Serialization;
using ContractAPI.APIs;
using ContractAPI.APIs.Interfaces;
using ContractAPI.DTO.Response;
using ContractAPI.Profiles;
using ContractAPI.Repository;
using ContractAPI.Repository.Interface;
using ContractAPI.Services;
using ContractAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Shared.DTOs.Contracts.Responses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mongoClient = new MongoClient(builder.Configuration["ConnectionStrings:ConnectionString"]);
builder.Services.AddSingleton( mongoClient.GetDatabase(builder.Configuration["ConnectionStrings:DatabaseName"]));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IContractService, ContractService>();

builder.Services.AddHttpClient<IImageAPI, ImageAPI >(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ImageApi"]); 
});
builder.Services.AddHttpClient<IRoomClientService, RoomClientService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:RoomApi"]);
});
builder.Services.AddHttpClient<IAccountAPI, AccountAPI>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:AccountApi"]);
});
builder.Services.AddHttpClient<IUtilityReadingClientService, UtilityReadingClientService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:UtilityReadingApi"]);
});



 builder.Services.AddAutoMapper(typeof(MappingContract).Assembly);
 builder.Services.AddAutoMapper(typeof(MappingIdentityProfile).Assembly);
var odatabuilder = new ODataConventionModelBuilder();
odatabuilder.EntitySet<ContractResponse>("Contract");
odatabuilder.EntitySet<IdentityProfileResponse>("IdentityProfile");
var odata = odatabuilder.GetEdmModel();
builder.Services.AddControllers().AddOData(options =>
    options.AddRouteComponents("odata", odata)
        .SetMaxTop(100)
        .Count()
        .Filter()
        .OrderBy()
        .Expand()
        .Select());

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContractAPI", Version = "v1" });

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
    // Cho phép nhận string -> enum và khi trả ra thì enum thành string
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
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

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();