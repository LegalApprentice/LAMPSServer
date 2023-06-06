using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using LAMPSServer.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using LAMPSServer.Hubs;

//https://www.youtube.com/watch?v=MBpH8sGqrMs   CORS rules


namespace LAMPSServer;

public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            "Startup".WriteInLine();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           "ConfigureServices".WriteInLine();


            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            //https://docs.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-5.0&tabs=dotnet#configure-server-options
            services.AddSignalR(hubOptions =>
            {
                hubOptions.ClientTimeoutInterval= TimeSpan.FromMinutes(38);
                hubOptions.EnableDetailedErrors = true;
            }).AddMessagePackProtocol();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(
                        "http://localhost")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod();
                });
            });

            services.AddSingleton<IEnvConfig>(provider => new EnvConfig("./.env"));

            services.AddSingleton<IElasticWrapper, ElasticWrapper>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Register the Swagger services
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "Legal Apprentice API", Version = "v2" });
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.0&tabs=visual-studio-code     
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Legal Apprentice API");
            });
            
            app.UseRouting();
            app.UseCors();

            // Uncomment to force https
            // app.UseHttpsRedirection(); 
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseStaticFiles(); // For the wwwroot folder
            app.UseDefaultFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<DocumentHub>("/documentHub");
                endpoints.MapControllers();

                // Route for passing parameters to Angular project
                endpoints.MapFallbackToFile("index.html");
            });


        }
    }

