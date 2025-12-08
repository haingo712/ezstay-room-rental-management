    using AuthApi.Data;
    using AuthApi.Models;
    using AuthApi.Repositories;
    using AuthApi.Repositories.Interfaces;
    using AuthApi.Services;
    using AuthApi.Services.Interfaces;
    using AuthApi.Utils;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using BCrypt.Net;
    using AuthApi.DTO.Request;
    using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.Configure<MongoSettings>(
        builder.Configuration.GetSection("MongoSettings"));
    builder.Services.AddSingleton<MongoDbService>(); // ? th�m d�ng n�y
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder => builder.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader());
    });

    builder.Services.AddHttpClient("MailApi", client =>
    {
        client.BaseAddress = new Uri("http://mailapi:8080");
    });

builder.Services.AddHttpClient("ImageAPI", client =>
{
    client.BaseAddress = new Uri("http://imageapi:8080"); // API của ImageAPI
});


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


    builder.Services.AddScoped<IAuthRepository, AuthRepository>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();
    builder.Services.AddScoped<IEmailVerificationService, EmailVerificationService>();
    builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
    builder.Services.AddScoped<IOwnerRequestService, OwnerRequestService>();
    builder.Services.AddScoped<IOwnerRequestRepository, OwnerRequestRepository>();
    builder.Services.AddScoped<IImageService, ImageService>();
    builder.Services.AddScoped<IFaceLoginService, FaceLoginService>();



builder.Services.AddHttpClient("Gateway", (serviceProvider, client) =>
    {
        var config = serviceProvider.GetRequiredService<IConfiguration>();
        var baseUrl = config["ServiceUrls:Gateway"];  // đọc từ appsettings.json
        client.BaseAddress = new Uri(baseUrl);
    });

    builder.Services.Configure<GoogleAuthSettings>(

        builder.Configuration.GetSection("GoogleAuth"));
    builder.Services.Configure<FacebookAuthSettings>(
        builder.Configuration.GetSection("FacebookAuth"));
    builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("TwilioSettings"));
    builder.Services.AddSingleton<IPhoneVerificationService, TwilioPhoneVerificationService>();




    builder.Services.AddScoped<IFacebookAuthService, FacebookAuthService>();

    builder.Services.AddSingleton<GenerateJwtToken>();

    var jwtKey = builder.Configuration["Jwt:Key"]!;

    builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserManagerAPI", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"Nhập JWT token theo định dạng: Bearer {token}",
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
                    Scheme = "Bearer",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
    });

    builder.Services.AddAuthorization();


    var app = builder.Build();

    // Seed admin user
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var accountRepo = services.GetRequiredService<IAuthRepository>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            var adminUser = await accountRepo.GetByEmailAsync("admin@gmail.com");
            if (adminUser == null)
            {
                logger.LogInformation("Admin user not found, creating one.");
                var newAdmin = new Account
                {
                    FullName = "Adminis",
                    Email = "admin@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Phone = "0799568612",
                    Role = Shared.Enums.RoleEnum.Admin,
                    IsVerified = true,
                    CreateAt = DateTime.UtcNow
                };
                await accountRepo.CreateAsync(newAdmin);
                logger.LogInformation("Admin user created successfully.");
            }
            else
            {
                logger.LogInformation("Admin user already exists.");
            }

            // Seed staff user
            var staffUser = await accountRepo.GetByEmailAsync("staff1@gmail.com");
            if (staffUser == null)
            {
                logger.LogInformation("Staff user 'staff1@gmail.com' not found, creating one.");
                var newStaff = new Account
                {
                    FullName = "Staff One",
                    Email = "staff@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Ezstay@12345"),
                    Phone = "0799568611",
                    Role = Shared.Enums.RoleEnum.Staff,
                    IsVerified = true,
                    CreateAt = DateTime.UtcNow
                };
                await accountRepo.CreateAsync(newStaff);
                logger.LogInformation("Staff user 'staff1@gmail.com' created successfully.");
            }
            else
            {
                logger.LogInformation("Staff user 'staff1@gmail.com' already exists.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowAll");

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
