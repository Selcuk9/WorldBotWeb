using InstaBotWeb.Classes;
using InstaBotWeb.Controllers;
using InstaBotWeb.Models.DataBaseContext;
using InstaBotWeb.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleAuthorize.Crypto;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using TelegramSystem;

namespace InstaBotWeb
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {   
            Configuration.Bind("Project",new Config());

            services.AddDbContext<ApplicationContext>(options =>
                 options.UseSqlServer(Config.ConnectionString));

            services.AddTransient<IHashMethod, MD5Hash>();

         //   services.AddSingleton<Pool>(Pool.Instance);
            services.AddSingleton<Dictionary<int, List<Pool>>>(new Dictionary<int, List<Pool>>());

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
