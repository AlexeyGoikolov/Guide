using System.Globalization;
using Guide.Models;
using Guide.Models.Data;
using Guide.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Guide
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
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews().AddViewLocalization();
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<UserRepository>();
            services.AddTransient<UploadService>();
            services.AddDbContext<GuideContext>(options =>
                    options.UseLazyLoadingProxies()
                        .UseNpgsql(connection))
                .AddIdentity<User, IdentityRole>(options => { options.User.RequireUniqueEmail = true; })
                .AddEntityFrameworkStores<GuideContext>();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/Login");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("ru"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("de"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStatusCodePagesWithRedirects("/Errors/?code={0}");
            app.UseRouting();

            
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "AdminArea",
                    areaName: "Admin",
                    pattern: "admin/{controller}/{action}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Details}/{id?}");
            });
        }
    }
}