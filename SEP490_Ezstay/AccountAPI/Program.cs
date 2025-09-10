using AccountAPI.Mapping;
using AccountAPI.Repositories.Interfaces;
using AccountAPI.Repositories;
using AccountAPI.Service.Interfaces;
using AccountAPI.Service;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using APIGateway.Helper;
using APIGateway.Helper.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoClient = new MongoClient(builder.Configuration["ConnectionStrings:ConnectionString"]);
builder.Services.AddSingleton(mongoClient.GetDatabase(builder.Configuration["ConnectionStrings:DatabaseName"]));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IUserClaimHelper, UserClaimHelper>();

builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(typeof(UserMappingProfile));

// ✅ Không cần jwtSettings và AddJwtBearer nữa
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = config["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();



// Swagger config (vẫn giữ để test token trong UI)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AccountAPI", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Nhập JWT token dạng: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Cần để nhận Claims từ Gateway
app.UseAuthorization();

app.MapControllers();
app.Run();
