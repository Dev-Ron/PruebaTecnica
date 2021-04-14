using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infracstructure.DataAccess;
using System;
using Microsoft.EntityFrameworkCore;
using Core.UseCases;
using Core.Interfaces;
using Core.Entities;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using static PruebaTecnica.Constantes;
using Microsoft.AspNetCore.Http;

namespace PruebaTecnica
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddDbContext<PTContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IRepositorio, Repositorio>();
            services.AddScoped<IBussinessLogic, BussinessLogic>();




            //Autenticacion
            services.AddAuthentication(options =>
            {
                //AuthenticationScheme = "Bearer"
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = context =>
                    {
                        List<Claim> cl = new List<Claim>(((ClaimsIdentity)context.Principal.Identity).Claims);
                        string strUsuario = cl.Where(c => c.Type == JWT_CLAIM_USUARIO).First().Value;

                        if (string.IsNullOrWhiteSpace(strUsuario))
                        {
                            context.Fail("Unauthorized");
                        }

                        return Task.CompletedTask;
                    }
                };

                //https://tools.ietf.org/html/rfc7519#page-9
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    SaveSigninToken = true,
                    ValidateActor = true,
                    ValidateIssuer = true, //Issuer: Emisor
                    ValidateAudience = true, //Audience: Son los destinatarios del token
                    ValidateLifetime = true, //Lifetime: Tiempo de vida del token
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.GetValue<string>("Issuer"),
                    ValidAudience = Configuration.GetValue<string>("Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey")))
                };
            });


            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(new[]
                    {
                        "http://localhost:62147",
                        "https://localhost:44325"
                    })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRouting();

            //https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.0&tabs=visual-studio
            /*For most apps, calls to UseAuthentication, UseAuthorization, and UseCors must appear between 
             * the calls to UseRouting and UseEndpoints to be effective.*/
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                //spa.Options.SourcePath = "ClientApp";
                spa.Options.SourcePath = Path.Join(env.ContentRootPath, "ClientApp");
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
