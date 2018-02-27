using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoinDriveICO.BusinessLayer;
using CoinDriveICO.Framework.SettingsModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoinDriveICO.Web
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
            services.AddMvc();
            services.AddOptions();
            services.Configure<IcoAdapterSettings>(options =>
                Configuration.GetSection(nameof(IcoAdapterSettings)).Bind(options));
            services.Configure<DbContextSettings>(options =>
                Configuration.GetSection(nameof(DbContextSettings)).Bind(options));
            services.Configure<MainSettings>(options => 
                Configuration.GetSection(nameof(MainSettings)).Bind(options));
            var dbSettings = new DbContextSettings();
            Configuration.GetSection(nameof(DbContextSettings)).Bind(dbSettings);
            services.RegisterServices(dbSettings);

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("C:\\storage"))
                .SetApplicationName("myApplicationName");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
