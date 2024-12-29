using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Lesson21_API_.Api.Manage;
using Lesson21_API_.Api.Manage.Dtos.CategoryDtos;
using Lesson21_API_.Data;
using Lesson21_API_.Data.Entities;
using Lesson21_API_.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lesson21_API_
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddFluentValidation(option=>option.RegisterValidatorsFromAssemblyContaining<CategoryCreateDtoValidator>());
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(_config.GetConnectionString("Default"));
            }).AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
            //services.AddScoped<IValidator<Person>, PersonValidator>(); internetrden numune kimi
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lesson21_API_", Version = "v1" });

                //using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                    cfg.AddProfile(new ManageMapper());
                    //cfg.AddProfile(new ClientMapper());//client ucun

            }).CreateMapper());
            services.AddAuthentication(options =>
            {
                //jwt tokeni uzerinde bearer schemi istf edirik
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //tokenden baxib validasiyanin heyata kecirilmesi ucun lazm olan deyerler
                    //gelen token audience issuer deyerini bu olaraq gozleyir,symmetricSecurityKey (encoeolunmus) burada verecem-gore validasiya versin
                    ValidIssuer= _config.GetSection("JWT:issuer").Value,
                    ValidAudience= _config.GetSection("JWT:audience").Value,
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:secret").Value))
                    //Bunnanda jwt condig tamamlandi

                };
            });
            services.AddScoped<IJwtService, JwtService>();
            //services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;//routede hecne gondermirsen base url-e giren kimi,seni documentationa atir
                }));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
