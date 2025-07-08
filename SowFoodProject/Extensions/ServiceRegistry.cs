using System;
using System.Text;
using ASS.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;
using SowFoodProject.Data;
using SowFoodProject.Infrastructure.Implementations.Services;
using SowFoodProject.Infrastructure.Utilities;
using SowFoodProject.Models.Entities;

namespace SowFoodProject.Extensions;

public static class ServiceRegistry
{
    public static IServiceCollection MyCustomeExtensionService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        //services.AddAuthorization(options =>
        //{
        //    AuthorizationConfiguration.ConfigureAuthorization(options);
        //});

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        //services.AddAuthentication(options =>
        //{
        //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //}).AddJwtBearer(options =>
        //{
        //    options.SaveToken = true;
        //    options.RequireHttpsMetadata = false; // Set to true in production
        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = configuration["JwtSettings:ValidIssuer"],
        //        ValidAudience = configuration["JwtSettings:ValidAudience"],
        //        IssuerSigningKey = new SymmetricSecurityKey(
        //            Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"])),
        //        ClockSkew = TimeSpan.Zero
        //    };

        //    // Add events for debugging
        //    options.Events = new JwtBearerEvents
        //    {
        //        OnAuthenticationFailed = context =>
        //        {
        //            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
        //            logger.LogError("Authentication failed: {Error}", context.Exception);
        //            return Task.CompletedTask;
        //        },
        //        OnTokenValidated = context =>
        //        {
        //            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
        //            logger.LogInformation("Token validated successfully");
        //            return Task.CompletedTask;
        //        }
        //    };
        //});

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SowFoodProject API",
                Version = "v1"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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
                    }
                },
                Array.Empty<string>()
            }
            });
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        services.AddHttpContextAccessor();

        // services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        services.AddScoped<IServiceManager, ServiceManager>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IdGenerator>();

        return services;
    }
}
