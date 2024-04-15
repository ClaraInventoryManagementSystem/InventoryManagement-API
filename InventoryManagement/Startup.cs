using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Business.Common;
using InventoryManagement.Business.Common.Interface;
using InventoryManagement.DataAccess.Common.Interface;
using InventoryManagement.Filters;
using InventoryManagement.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using InventoryManagement.cache;

namespace InventoryManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // Init Serilog configuration
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();            
            Configuration = configuration;
           
            
        }

        private void InitApplicationCache(IWebHostEnvironment env)
        {
            ApplicationCache.Init(Configuration, env);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddControllers()
            .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null); 
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            

            services.AddAuthentication("InventoryManagementAuthentication")
               .AddScheme<AuthenticationSchemeOptions, InventoryAuthendicationHandler>("InventoryManagementAuthentication", null);

            // configure DI for application services
            services.AddScoped<ITokenServices, TokenServices>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory Management System Api", Version = "v1" });
                c.OperationFilter<TokenFilter>();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            // logging
            //loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerConfiguration()));
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            var builder = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json");

            var config = builder.Build();

            app.UseRouting();
            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

            //var enableApm = config.GetValue<bool>("ElasticApm:IsEnabled");

            //app.UseWelcomePage();
            //app.Use

            app.UseExceptionHandlingMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseEndpoints(x => x.MapControllers());
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            InitApplicationCache(env);

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url:"swagger/v1/swagger.json", "Inventory Management System V1");
                c.RoutePrefix = string.Empty;
            });


            //app.UseMvc();
            //app.UseSession();

        }
    }
}
