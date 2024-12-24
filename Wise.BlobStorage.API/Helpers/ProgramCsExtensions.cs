using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Wise.BlobStorage.Application;
using Wise.BlobStorage.Domain.Models;
using Wise.BlobStorage.Infrastructure;
using Wise.BlobStorage.Infrastructure.Context;

namespace Wise.BlobStorage.API.Helpers
{
    public static class ProgramCsExtensions
    {
        public static IServiceCollection AddBlobStorageApi(this IServiceCollection services , IConfiguration configuration)
        {
            services.ConfigureBlobStorageOption(configuration);
            services.AddProjectServices(configuration);
            services.AddDbContext(configuration);
            services.ConfigureSwaggerGen();
            services.AddAuthentication(configuration);
            return services;
        }

        public static void AddDbContext(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<WiseDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), providerOptions =>
                {
                    providerOptions.CommandTimeout(180);
                });
            });
        }

        public static void ConfigureSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Wise Dynamics API",
                    Description = "Wise Dynamics API for Clients",
                    TermsOfService = new Uri("https://appv2.wise-dynamic.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "Admin",
                        Email = "admin@wise-dynamic.com",
                        Url = new Uri("https://appv2.wise-dynamic.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://appv2.wise-dynamic.com"),
                    }
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Security:Jwt");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,

                    //ValidateIssuer = true, 
                    //ValidIssuer = jwtSettings["Issuer"],

                    ValidateAudience = false,

                    //ValidateAudience = true,
                    //ValidAudience = jwtSettings["Audience"],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine($"Token successfully validated: {context.SecurityToken}");
                        return Task.CompletedTask;
                    }
                };
            });
        }

        public static void ConfigureBlobStorageOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BlobStorageOption>(configuration.GetSection("BlobStorage"));
        }

        public static void AddProjectServices(this IServiceCollection services , IConfiguration configuration)
        {
            services
                .AddInfrastructureServices(configuration)
                .AddApplicationServices();
        }
    }
}
