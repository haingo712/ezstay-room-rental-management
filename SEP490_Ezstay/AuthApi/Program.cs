using AuthApi.Data;
using AuthApi.Repositories;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services;
using AuthApi.Services.Interfaces;
using AuthApi.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddSingleton<MongoDbService>(); // ? thêm dòng này


builder.Services.AddHttpClient("MailApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5004");
});


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();
builder.Services.AddScoped<IEmailVerificationService, EmailVerificationService>();
builder.Services.AddSingleton<GenerateJwtToken>();


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
