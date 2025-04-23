using System.Text;
using API.Source.Db;
using API.Source.Db.Repo;
using API.Source.Extensions;
using API.Source.Factory;
using API.Source.Guards;
using FluentValidation;
using FluentValidation.AspNetCore;
using llibrary.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Resend;

namespace API.Source;

public static class Dependencies
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(3);
        });

        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddGoogle(options =>
            {
                options.ClientId =
                    configuration["Google:ClientId"]
                    ?? throw new Exception("GoogleRedirect client id is required");
                options.ClientSecret =
                    configuration["Google:ClientSecret"]
                    ?? throw new Exception("GoogleRedirect client secret is required");
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            configuration["Jwt:Key"] ?? throw new Exception("Jwt key is required")
                        )
                    ),
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthHandler>(
                "ApiKey",
                configureOptions: null
            )
            .AddIdentityCookies();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo() { Title = "ChitChat API", Version = "v1" });

            options.AddSecurityDefinition(
                "ApiKey",
                new OpenApiSecurityScheme()
                {
                    Name = "X-API-KEY",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                }
            );

            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
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
                                Id = "ApiKey",
                            },
                        },
                        new List<string>()
                    },
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

        services.AddSingleton<TokenManager>(sp => new TokenManager(configuration));

        services.AddTransient<ILogger<IWarningLogger>>(sp =>
        {
            using ILoggerFactory factory = LoggerFactory.Create(c => c.AddDebug().AddConsole());

            return factory.CreateLogger<IWarningLogger>();
        });

        services.AddTransient<ILogger<IInformationLogger>>(sp =>
        {
            using ILoggerFactory factory = LoggerFactory.Create(c => c.AddDebug());
            return factory.CreateLogger<IInformationLogger>();
        });

        //services.AddTransient<ILogger<ICriticalLogger>>(sp =>
        //{
        //    Log.Logger = new LoggerConfiguration()
        //        .WriteTo.File(
        //            "critical_logs.txt",
        //            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Fatal
        //        )
        //        .CreateLogger();
        //    using ILoggerFactory criticalFactory = LoggerFactory.Create(builder =>
        //        builder.AddSerilog().AddDebug().AddConsole()
        //    );
        //    return criticalFactory.CreateLogger<ICriticalLogger>();
        //});

        services
            .AddAuthorizationBuilder()
            .AddPolicy(name: Policies.Admin, configurePolicy: Policies.AdminPolicyConfig)
            .AddPolicy(name: Policies.Elevated, configurePolicy: Policies.ElevatedPolicyConfig)
            .AddPolicy(name: Policies.Moderator, configurePolicy: Policies.ModeratorPolicyConfig)
            .AddPolicy(name: Policies.Public, configurePolicy: Policies.PublicPolicyConfig);

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Program>();

        services
            .AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
                    .Json
                    .ReferenceLoopHandling
                    .Ignore
            );
        ;
        services.AddEndpointsApiExplorer();

        services.AddMemoryCache();

        services.AddOptions();
        services.AddHttpClient<ResendClient>();
        services.Configure<ResendClientOptions>(o =>
        {
            o.ApiToken =
                configuration["ResendApiKey"] ?? throw new Exception("Resend API key is required");
        });
        services.AddTransient<IResend, ResendClient>();

        services.RegisterHandlersFromAssembly(typeof(Program).Assembly);
        services.AddScoped<IRequestHandlerFactory, RequestHandlerFactory>();

        return services;
    }
}
