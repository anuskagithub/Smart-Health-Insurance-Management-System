using HealthInsuranceApi.Data;
using HealthInsuranceApi.Models;
using HealthInsuranceApi.Services.Implementations;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// DATABASE
// =====================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// =====================================================
// IDENTITY
// =====================================================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// =====================================================
// JWT AUTHENTICATION
// =====================================================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var jwtKey = jwtSettings["SecretKey"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("JWT SecretKey is missing in appsettings.json");
}

builder.Services.AddAuthentication(options =>
{
    // Force JWT to be the default for everything
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)
            ),

            RoleClaimType = ClaimTypes.Role
        };
    });

// =====================================================
// CONTROLLERS (IMPORTANT)
// =====================================================
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// =====================================================
// APPLICATION SERVICES (DI)
// =====================================================

// Auth & Admin
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();

// Role-based services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<IClaimsOfficerService, ClaimsOfficerService>();
builder.Services.AddScoped<IHospitalProviderService, HospitalProviderService>();

// Core domain services
builder.Services.AddScoped<IPolicyService, PolicyService>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IInsurancePlanService, InsurancePlanService>();


// Reporting & notifications
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// =====================================================
// SWAGGER
// =====================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Smart Health Insurance API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token as: Bearer {your_token}"
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
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// =====================================================
// BUILD APP (ONLY ONCE)
// =====================================================
var app = builder.Build();

// =====================================================
// SEED ROLES & ADMIN
// =====================================================
using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
}

// =====================================================
// MIDDLEWARE PIPELINE
// =====================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Server is up and running!");
});

app.Run();
