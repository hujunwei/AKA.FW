using System.Text;
using EFCoreApi.DTOs;
using EFCoreApi.Infra.Extensions;
using EFCoreApi.Infra.Handlers;
using EFCoreApi.Infra.Middlewares;
using EFDataAccess;
using EFIdentityFramework;
using EFIdentityFramework.Model;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Utilities.Authentication;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        builder.Services.AddControllers();

        // EFCore
        builder.Services.AddDbContext<EFCoreContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
        });

        // FluentValidation
        builder.Services.AddValidatorsFromAssemblyContaining<PersonDtoValidator>();
        
        // AutoMapper
        builder.Services.AddAutoMapper(typeof(Program));
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var scheme = new OpenApiSecurityScheme()
            {
                Description = "Authorization header.\r\nExample: 'Bearer eyftubsaceweacsa...'",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Authorization"
                },
                Scheme = "oauth2",
                Name = "Authorization",
                In = ParameterLocation.Header, Type = SecuritySchemeType.ApiKey
            };
            
            options.AddSecurityDefinition("Authorization", scheme);
            var requirement = new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            };
            options.AddSecurityRequirement(requirement);
;        });

        builder.Services.AddHttpContextAccessor();
        
        // Custom Types
        builder.Services.AddCustomTypes();

        // See https://stackoverflow.com/questions/47735133/asp-net-core-synchronous-operations-are-disallowed-call-writeasync-or-set-all
        builder.Services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });

        #region EF Core Identity Framework
        
        builder.Services.AddDbContext<IdDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("Identity"));
        });

        builder.Services.AddDataProtection();
        builder.Services.AddIdentityCore<User>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
        });

        var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
        idBuilder.AddEntityFrameworkStores<IdDbContext>()
            .AddDefaultTokenProviders()
            .AddRoleManager<RoleManager<Role>>()
            .AddUserManager<UserManager<User>>();
        
        #endregion

        #region AuthN

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = builder.Configuration.GetSection("JWT").Get<JwtOptions>();
                Console.WriteLine(jwtOptions.SigningKey);
                var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
                var signingKey = new SymmetricSecurityKey(keyBytes);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey
                };
            });
        

        #endregion
        
        var app = builder.Build();
        
        // Global Exception Handler
        app.UseExceptionHandler(a => a.Run(async context =>
        {
            await GeneralExceptionHandler.HandleException(context);
        }));
        
        // CorrelationId Preserver, used to intercept request and add correlationId in request header
        app.UseMiddleware<CorrelationPreserverMiddleware>();
        app.UseMiddleware<ApiRequestTrackingMiddleware>();
        // API logger to record generic information of an API
        app.UseMiddleware<ApiLoggerMiddleware>();

        /*
            [AllowedHosts] is used for host filtering to bind your app to specific hostnames.
            For example, if you replace following:
            "AllowedHosts": "*"
                with
            "AllowedHosts": "example.com"
            and you try to access your app using http://localhost:1234/ address you will get default bad request (400) response.

            [AllowedOrigins]
            CORS, on the other hand, is to control which hosts try accessing a resource (API) on your app, e.g. the client UI server. 
        */
        var data = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
        
        app.UseCors(corsBuilder => corsBuilder
            .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
        
        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();
        

        // This line must be after logger registration.
        app.UseStaticApiLogger();
        // Register Http Context Accessor
        app.UseStaticHttpContext();
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    
        app.Run();
    }
}