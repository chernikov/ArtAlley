using ArtAlley.Data;
using ArtAlley.Data.Repositories;
using ArtAlley.Hubs;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ArtAlley
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
            var configurationSection = Configuration.GetSection("ConnectionStrings:DefaultConnection");
            services.AddDbContext<ArtAlleyDbContext>(options => options.UseSqlServer(configurationSection.Value));
            services.AddScoped<IArtAlleyDbContext, ArtAlleyDbContext>();
            services.AddScoped(provider =>
                   new Func<IArtAlleyDbContext>(() => provider.GetService<IArtAlleyDbContext>()));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options => //CookieAuthenticationOptions
                {
                   options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/login");
               });

            services.AddControllersWithViews();
            services.AddSignalR();

            RegisterRepositories(services);
            RegisterAutomapper(services);
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
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseStaticFiles(
                 new StaticFileOptions
                 {
                     FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "static")),
                     RequestPath = "/static"
                 });
            app.UseRouting();
            app.UseAuthentication();   
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ControlHub>("/controlhub");
            });
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(IBaseRepository));

            var types = assembly.GetTypes();
            var repositoryInterfaces = assembly.GetTypes()
                .Where(p => p.GetInterface(typeof(IBaseRepository).Name) != null && p.IsInterface && p.IsPublic).ToList();

            foreach (var _interface in repositoryInterfaces)
            {
                var type = assembly.GetTypes().FirstOrDefault(p => p.GetInterface(_interface.Name) != null && p.IsPublic && !p.IsAbstract);
                if (type != null)
                {
                    services.AddScoped(_interface, type);
                }
            }
        }

        private void RegisterAutomapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                var profileTypes = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(p => p.IsSubclassOf(typeof(Profile))).ToList();

                foreach (var profileType in profileTypes)
                {
                    var obj = (Profile)Activator.CreateInstance(profileType);
                    mc.AddProfile(obj);
                }
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}

