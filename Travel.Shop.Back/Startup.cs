using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Travel.Shop.Back.Common.Domain.Managers;
using Travel.Shop.Back.Data;
using Travel.Shop.Back.Mapping;
using Travel.Shop.Back.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Travel.Shop.Back
{
    public class Startup
    {
        readonly string _corsOrigins = "_corsOrigins";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            AddAutoMapper(services);

            AddCors(services);

            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<BaseDbContext>(options =>
                options.UseSqlServer(connection));

            #region Identity

            services.AddIdentity<Manager, IdentityRole>(options =>
            {
                options.User = new UserOptions
                {
                    RequireUniqueEmail = true
                };

                // упростил для тестирования
                options.Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false,
                    RequireLowercase = false,
                    RequiredLength = 3,
                };
            }).AddEntityFrameworkStores<BaseDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["AuthOptions:ISSUER"],

                    ValidateAudience = true,
                    ValidAudience = Configuration["AuthOptions:AUDIENCE"],
                    ValidateLifetime = true,

                    IssuerSigningKey = Configuration["AuthOptions:KEY"].GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });

            services.AddAuthorization();

            #endregion

            services.AddSingleton(Configuration);
            services.AddScoped<DataScope>();
            services.AddScoped<IEntityManager, EntityManager>();
            services.AddScoped<IAutorizationService, AutorizationService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Test API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Git Hub",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/MrEveKS/Travel.Shop.Back"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

                c.AddSecurityRequirement(securityRequirement);
            });

            services.AddMvcCore()
                .AddNewtonsoftJson()
                .AddApiExplorer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
                c.RoutePrefix = string.Empty;

                c.OAuthClientId("swagger-ui");
                c.OAuthClientSecret("swagger-ui-secret");
                c.OAuthRealm("swagger-ui-realm");
                c.OAuthAppName("Swagger UI");
                c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "foo", "bar" } });
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });

            app.UseCors(_corsOrigins);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        private void AddAutoMapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }

        private void AddCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(_corsOrigins,
                builder =>
                {
                // настройки корс для фронта
                builder.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                });
            });
        }
    }
}
