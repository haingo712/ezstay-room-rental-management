using MailApi.Config;
using MailApi.Repository;
using MailApi.Repository.Interface;
using MailApi.Services;
using MailApi.Services.Interfaces;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mongoClient = new MongoClient(builder.Configuration["ConnectionStrings:ConnectionString"]);
builder.Services.AddSingleton(mongoClient.GetDatabase(builder.Configuration["ConnectionStrings:DatabaseName"]));

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddScoped<IMailRepository, MailRepository>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();
