using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpatialiteCrash.Database;

namespace SpatialiteCrash.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc();

            MigrateDatabase();
            AddData();
        }

        private void MigrateDatabase()
        {
            using (var db = new SpatialiteCrashDbContext())
            {
                db.Database.Migrate();
            }
        }

        private void AddData()
        {
            using (var db = new SpatialiteCrashDbContext())
            {
                //quick hack to make sure we dont seed all the time
                if (db.Orders.Count() < 20000)
                {
                    var dataSeedRange = Enumerable.Range(1, 10000);

                    var data = dataSeedRange.Select(x => new Database.Order
                    {
                        Id = Guid.NewGuid(),
                        OrderNumber = 145325235,
                        OrderDate = DateTime.UtcNow
                    });

                    db.Orders.AddRange(data);

                    db.SaveChanges();
                }
            }
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
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
