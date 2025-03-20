using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GameInventoryAPI.Data;


var builder = WebApplication.CreateBuilder(args);

// Логируем загруженную конфигурацию
Console.WriteLine("Configuration Sources:");
foreach (var source in builder.Configuration.Sources)
{
    Console.WriteLine(source);
}

Console.WriteLine("JWT Key from Configuration: " + builder.Configuration["Jwt:Key"]);

// Добавление сервисов
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Получаем ключ для JWT
var jwtKey = builder.Configuration["Jwt:Key"];
Console.WriteLine($"JWT Key: {jwtKey}");
Console.WriteLine($"JWT Key Length: {jwtKey.Length * 8} bits"); // Длина ключа в битах
if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
{
    throw new InvalidOperationException("JWT Key is not configured or is too short.");
}

var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

// Настройка аутентификации с JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Позволяет работать без HTTPS в Dev-среде
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };

        // Добавляем события для логирования
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(context.Exception, "JWT authentication failed.");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("JWT token validated for user: {UserName}", context.Principal.Identity.Name);
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwt"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Инициализация БД
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    DbInitializer.Initialize(dbContext, configuration); // Передаём конфигурацию для начальных данных
}

// Настройка middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Логирование ошибок в middleware
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Starting request processing for path: {Path}", context.Request.Path);
        await next();
        logger.LogInformation("Completed request processing for path: {Path}", context.Request.Path);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An unhandled exception occurred while processing the request.");
        throw;
    }

    var headers = context.Request.Headers;
    // Логируем заголовки
    //foreach (var header in headers)
    //{
    //    Console.WriteLine($"{header.Key}: {header.Value}");
    //}
    //await next();

});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();