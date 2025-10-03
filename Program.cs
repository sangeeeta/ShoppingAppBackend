using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ShoppingApp.Data;
using ShoppingApp.Interface;
using ShoppingApp.Middlewares.Loggers;
using ShoppingApp.Repository;
using ShoppingApp.RepositoryInterface;
using ShoppingApp.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// 1. Add CORS
// ---------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Angular app URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// ---------------------------
// 2. Add logging
// ---------------------------

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Logs/requests.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ---------------------------
// 3. Add Controllers
// ---------------------------
builder.Services.AddControllers(option => option.ReturnHttpNotAcceptable = true);

// ---------------------------
// 4. Add DbContext (PostgreSQL)
// ---------------------------
builder.Services.AddDbContext<ShoppingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------------
// 5. Add JWT Authentication
// ---------------------------

//  Add JwtService (your custom service)
builder.Services.AddScoped<JwtService>();



var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT Key not found in configuration.");
}

builder.Services.AddAuthentication("Bearer")
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// ---------------------------
// 6. Swagger + JWT Setup
// ---------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer {your token}'"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// 1️⃣ Register services in DI container
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

WebApplication app = builder.Build();

// ---------------------------
// 7. Middleware pipeline
// ---------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");

app.UseMiddleware<ErrorHandlingMiddleware>();   
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseHttpsRedirection();
// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
