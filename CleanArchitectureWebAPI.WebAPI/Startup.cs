using CleanArchitectureWebAPI.Application;
using CleanArchitectureWebAPI.Infrastructure.Data;
using CleanArchitectureWebAPI.Infrastructure.Data.Context;
using CleanArchitectureWebAPI.Infrastructure.IoC;
using CleanArchitectureWebAPI.WebAPI.Middlewares;
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
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureWebAPI.WebAPI
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
            // Setting Response Caching
            services.AddResponseCaching();

            // Setting In Memory Cache
            services.AddMemoryCache();

            services.AddControllers();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            // This is where we register the context
            services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MyConnectionString")));

            // Registering the Identity Infrastructure
            services.AddIdentityInfrastructure();

            // Registering Inversion Of Control
            services.AddIoCService();

            // Registering the Swagger
            services.AddSwaggerDocument();

            // Registering the HttpContext Accessor which we use for auditing in the LibraryDbContext
            services.AddHttpContextAccessor();

            // Registering the AutoMapper that mapps from entity to view model and vice versa
            services.AddApplicationLayer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Using the Response Caching
            app.UseResponseCaching();

            // Use Serilog Logger
            app.UseSerilogRequestLogging();

            // Error Logging middleware
            app.UseMiddleware<ErrorLoggingMiddleware>();

            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();

            app.UseSwaggerUi3();
        }
    }
}
