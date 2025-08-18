using AmenityAPI.Data;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using RoomAPI.Data;
using RoomAPI.DTO.Request;
using RoomAPI.Repository;
using RoomAPI.Repository.Interface;
using RoomAPI.Service;
using RoomAPI.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddSingleton<MongoDbService>();

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();
var odatabuilder = new ODataConventionModelBuilder();
odatabuilder.EntitySet<RoomDto>("Rooms");
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