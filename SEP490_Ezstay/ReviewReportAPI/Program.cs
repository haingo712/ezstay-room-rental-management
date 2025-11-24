using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using ReviewReportAPI.DTO.Response;
using ReviewReportAPI.Profiles;
using ReviewReportAPI.Repository;
using ReviewReportAPI.Repository.Interface;
using ReviewReportAPI.Service;
using ReviewReportAPI.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var mongoClient = new MongoClient(builder.Configuration["ConnectionStrings:ConnectionString"]);
builder.Services.AddSingleton( mongoClient.GetDatabase(builder.Configuration["ConnectionStrings:DatabaseName"]));
builder.Services.AddScoped<IReviewReportRepository, ReviewReportRepository>();
builder.Services.AddScoped<IReviewReportService, ReviewReportService>();
builder.Services.AddHttpClient<IReviewService, ReviewService>(client =>
{ 
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ReviewApi"]);
});
builder.Services.AddHttpClient<IImageService, ImageService>(client =>
{ 
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ImageApi"]);
});
var odatabuilder = new ODataConventionModelBuilder();
odatabuilder.EntitySet<ReviewReportResponse>("ReviewReports");
var odata = odatabuilder.GetEdmModel();
builder.Services.AddControllers().AddOData(options =>
    options.AddRouteComponents("odata", odata)
        .SetMaxTop(100)
        .Count()
        .Filter()
        .OrderBy()
        .Expand()
        .Select());
builder.Services.AddAutoMapper(typeof(ReviewReportProfile).Assembly);

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReviewReportAPI", Version = "v1" });

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
// builder.Services.AddHttpClient<IContractClientService, ContractClientService>(client =>
// { 
//     client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ContractApi"]);
// });
// builder.Services.AddHttpClient<IPostClientService, PostClientService>(client =>
// { 
//     client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:RentalPostApi"]);
// });


            builder.Services.AddControllers().AddJsonOptions(options =>
            {
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
