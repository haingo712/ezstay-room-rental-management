using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using UtilityReadingAPI.Data;
using UtilityReadingAPI.DTO.Request;
using UtilityReadingAPI.Repository;
using UtilityReadingAPI.Repository.Interface;
using UtilityReadingAPI.Service;
using UtilityReadingAPI.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddSingleton<MongoDbService>();




builder.Services.AddScoped<IUtilityReadingRepository, UtilityReadingRepository>();
builder.Services.AddScoped<IUtilityReadingService, UtilityReadingService>();
var odatabuilder = new ODataConventionModelBuilder();
odatabuilder.EntitySet<UtilityReadingDto>("UtilityReadings");
var odata = odatabuilder.GetEdmModel();
builder.Services.AddControllers().AddOData(options =>
    options.AddRouteComponents("odata", odata)
        .SetMaxTop(100)
        .Count()
        .Filter()
        .OrderBy()
        .Expand()
        .Select());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 
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