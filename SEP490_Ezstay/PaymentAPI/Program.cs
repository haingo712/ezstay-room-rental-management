using PaymentAPI.Config;
using PaymentAPI.Services;
using PaymentAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<VnPayConfig>(builder.Configuration.GetSection("VnPay"));
builder.Services.AddScoped<IPaymentService, VnPayService>();

// builder.Services.AddScoped<IPaymentService, PaypalService>();
// builder.Services.AddScoped<IPaymentService, MoMoService>();

// builder.Services.AddSingleton<IPaymentService,VnPayService>();
// builder.Services.AddSingleton<IPaymentService,MoMoService>();

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