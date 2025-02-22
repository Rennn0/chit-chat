using System.Text;
using API.Source.Db;
using API.Source.Db.Repo;
using API.Source.Factory;
using API.Source.Guards;
using API.Source.Handlers;
using API.Source.Handlers.AddNewTenant;
using API.Source.Handlers.AddNewUser;
using API.Source.Handlers.ApiKey;
using API.Source.Handlers.ListTenants;
using API.Source.Handlers.ListUsers;
using API.Source.Handlers.Login;
using FluentValidation;
using FluentValidation.AspNetCore;
using llibrary.Logging;
using Microsoft.AspNetCore.Authentication;
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
            .AddIdentityCore<ApplicationUser>(options =>
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

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
            );

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

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

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
                    }
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
            .AddPolicy(name: Policies.Elevated, configurePolicy: Policies.ElevatedPolicyConfig);

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Program>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddMemoryCache();

        services.AddScoped<
            IRequestHandler<AddNewUserRequest, ResponseModelBase<object>>,
            AddUserHandler
        >();

        services.AddScoped<
            IRequestHandler<LoginRequest, ResponseModelBase<string>>,
            ApiKeyHandler
        >();

        services.AddScoped<
            IRequestHandler<LoginRequest, ResponseModelBase<string>>,
            TokenHandler
        >();

        services.AddScoped<
            IRequestHandler<ListUsersRequest, ResponseModelBase<IEnumerable<ApplicationUser>>>,
            ListUsersHandler
        >();
        services.AddScoped<
            IRequestHandler<ListUsersRequest, ResponseModelBase<IEnumerable<ApplicationUser>>>,
            FilterUsersHandler
        >();

        services.AddScoped<
            IRequestHandler<AddNewTenantRequest, ResponseModelBase<AddNewTenantResponse>>,
            AddNewTenantHandler
        >();

        services.AddScoped<
            IRequestHandler<
                ListTenantsRequest,
                ResponseModelBase<IEnumerable<ListTenantsResponse>>
            >,
            ListTenantsHandler
        >();

        services.AddScoped<
            IRequestPipeline<ListUsersRequest, ResponseModelBase<IEnumerable<ApplicationUser>>>,
            RequestPipeline<ListUsersRequest, ResponseModelBase<IEnumerable<ApplicationUser>>>
        >();

        services.AddScoped<
            IRequestPipeline<AddNewTenantRequest, ResponseModelBase<AddNewTenantResponse>>,
            RequestPipeline<AddNewTenantRequest, ResponseModelBase<AddNewTenantResponse>>
        >();

        services.AddScoped<
            IRequestPipeline<
                ListTenantsRequest,
                ResponseModelBase<IEnumerable<ListTenantsResponse>>
            >,
            RequestPipeline<ListTenantsRequest, ResponseModelBase<IEnumerable<ListTenantsResponse>>>
        >();

        services.AddScoped<
            IRequestPipeline<LoginRequest, ResponseModelBase<string>>,
            RequestPipeline<LoginRequest, ResponseModelBase<string>>
        >();

        services.AddScoped<IRequestHandlerFactory, RequestHandlerFactory>();

        return services;
    }
}
