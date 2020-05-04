using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApp.Api.Configuration;
using WebApp.Api.Services;
using WebApp.Api.Utillits;

namespace WebApp.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //DI
            // �������� , ������ ������ ����� ������ ���� ��� � ���� �� ����� ������ �������(�������� ��)
            services.AddSingleton<IUserRepository, FakeUserRepository>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();

            // �������� ����� � ����� �������� , ��� �� ��������� ������  )
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));

            services.AddControllersWithViews();
            #region //JWT
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //��������� ���� �� �������������
                options.SaveToken = false;
                options.RequireHttpsMetadata = false;
                // ��������� ���������
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true, //���������� ��������
                    ValidateAudience = true, //���������� �����������
                    ValidateLifetime = true, //��������� ������� �����
                    ValidIssuer = Configuration["Jwt:Issuer"],  //�������� ��������
                    ValidAudience = Configuration["Jwt:Audience"], // �������� �����������
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"])),
                    ClockSkew = TimeSpan.FromSeconds(15)  // ������� �� ������� �� ��� ����� �������� 
                };


            });
            #endregion

            //swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Test API",
                    Version = "1.0.0",
                    Description = "Just for test",
                    Contact = new OpenApiContact
                    {
                        Email = "TengriBizMenen@nomad.az",
                        Name = "Oguz Khan",
                        Url = new Uri("https://ru.wikipedia.org/wiki/%D0%9E%D0%B3%D1%83%D0%B7-%D1%85%D0%B0%D0%BD")
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] {}

                    }
                });

            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(cfg =>
            {
                cfg.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
                cfg.RoutePrefix = String.Empty;
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
