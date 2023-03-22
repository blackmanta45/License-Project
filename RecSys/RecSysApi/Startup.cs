using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RecSysApi.Application;
using RecSysApi.Application.Commons.Settings;
using RecSysApi.Application.Services;
using RecSysApi.Domain;
using RecSysApi.Domain.Dtos;
using RecSysApi.Domain.Models;
using RecSysApi.Infrastructure;
using RecSysApi.Infrastructure.Context;
using RecSysApi.Presentation.Middleware;

namespace RecSysApi.Presentation
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
            services.AddControllers();
            services.AddOptions();

            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
            services.Configure<SfqSettings>(Configuration.GetSection(nameof(SfqSettings)));
            services.Configure<UpvSettings>(Configuration.GetSection(nameof(UpvSettings)));
            services.Configure<TagsSettings>(Configuration.GetSection(nameof(TagsSettings)));

            var connection = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>().DbConnectionString;
            services.AddDbContext<UpvDbContext>(options => options.UseSqlServer(connection, 
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)));

            services.AddInfrastructureLayerDependencies();
            services.AddDomainLayerDependencies();
            services.AddApplicationLayerDependencies();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "RecSysApi", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RecSysApi v1"));
            }

            app.UseHttpsRedirection();

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<UpvDbContext>();
                context?.Database.Migrate();

                var adminUser = context?.Users.FirstOrDefault(b => b.Username == "admin");
                if (adminUser is null)
                {
                    context?.Users.Add(new User
                    {
                        Username = "admin",
                        Hash = Configuration.GetSection("AdminHash").Value,
                        Role = "Admin",
                        Created = DateTime.Now
                    });
                }
                var updateUser = context?.Users.FirstOrDefault(b => b.Username == "update");
                if (updateUser is null)
                {
                    context?.Users.Add(new User
                    {
                        Username = "update",
                        Hash = Configuration.GetSection("UpdateHash").Value,
                        Role = null,
                        Created = DateTime.Now
                    });
                }
                context?.SaveChanges();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}