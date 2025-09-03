using MongoDB.Driver;
using TenantAPI.Repository.Interface;
using TenantAPI.Repository;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoClient = new MongoClient(builder.Configuration["ConnectionStrings:ConnectionString"]);
builder.Services.AddSingleton( mongoClient.GetDatabase(builder.Configuration["ConnectionStrings:DatabaseName"]));

// builder.Services.AddScoped<ITenantRepository, TenantRepository>();
// builder.Services.AddScoped<ITenantService, TenantService>();
// var odatabuilder = new ODataConventionModelBuilder();
// odatabuilder.EntitySet<TenantDto>("Tenants");
// var odata = odatabuilder.GetEdmModel();
// builder.Services.AddControllers().AddOData(options =>
//     options.AddRouteComponents("odata", odata)
//         .SetMaxTop(100)
//         .Count()
//         .Filter()
//         .OrderBy()
//         .Expand()
//         .Select());


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
