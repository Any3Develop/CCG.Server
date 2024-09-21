using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using CCG.Application.Contracts.Identity;
using CCG.Application.Services;
using CCG.Infrastructure.Configurations;
using CCG.WebApi.Infrastructure.ActionFilter;
using CCG.WebApi.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace CCG.WebApi.Infrastructure.DI
{
    public static class DiWebApi
    {
        public static void InstallWebApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            var jwtTokenConfig = configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
            services.AddSingleton(jwtTokenConfig);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    SaveSigninToken = true,
                    ClockSkew = TimeSpan.FromMinutes(1),
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.NameIdentifier
                };
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var tokenFromQuery = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(tokenFromQuery) && path.StartsWithSegments("/hubs/"))
                        {
                            context.Token = tokenFromQuery;
                            return Task.CompletedTask;
                        }

                        var tokenFromHeader = context.Request.Headers["access_token"];
                        if (!string.IsNullOrEmpty(tokenFromHeader))
                        {
                            context.Token = tokenFromHeader;
                            return Task.CompletedTask;
                        }

                        var tokenFromCookie = context.Request.Cookies["access_token"];
                        context.Token = tokenFromCookie;

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorizationBuilder().AddPolicy("RequireAdministratorRole", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireRole("Admin");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = VersionInfo.SolutionName,
                    Version = VersionInfo.ApiVersion
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = VersionInfo.SolutionName,
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                var requirement = new OpenApiSecurityRequirement
                {
                    {securityScheme, Array.Empty<string>()}
                };

                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(requirement);
                c.OperationFilter<ReqTokenOperationFilter>();

                var filePath = Path.Combine(AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                if (File.Exists(filePath))
                    c.IncludeXmlComments(filePath, true);
            });

            services.AddCors(o => o.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddResponseCaching();
            services.AddResponseCompression();


            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IIdentityProviderService, IdentityProviderService>();
            services.AddSingleton<IApplicationEnvironment, ApplicationEnvironment>();
        }
    }
}