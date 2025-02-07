using System.Text;
using API.Source.Db;
using API.Source.Factory;
using API.Source.Guard;
using API.Source.Strategy;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API.Source;

public static class Dependencies
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            configuration["Jwt:Key"] ?? throw new Exception("Jwt key is required")
                        )
                    ),
                };
            });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo() { Title = "API", Version = "v1" });

            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                }
            );

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        new List<string>()
                    },
                }
            );
        });

        services.AddScoped<
            IRequestHandlerStrategy<AddNewUserRequest, ResponseModelBase<object>>,
            AddUserStrategy
        >();
        services.AddScoped<
            IRequestHandlerStrategy<LoginRequest, ResponseModelBase<string>>,
            LoginStrategy
        >();
        services.AddScoped<IRequestHandlerFactory, RequestHandlerFactory>();

        services.AddSingleton<TokenManager>(sp => new TokenManager(configuration));

        services
            .AddAuthorizationBuilder()
            .AddPolicy(
                name: Policies.AdminRolePolicy,
                configurePolicy: p => p.RequireRole("AdminRole")
            )
            .AddPolicy(
                name: Policies.ElevatedRolePolicy,
                configurePolicy: p => p.RequireClaim("EmailClaim")
            );

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Program>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        return services;
    }
}
