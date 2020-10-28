using Guide.Models;
using Guide.Models.Data;
using Guide.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            services.AddControllersWithViews();
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<UserRepository>();
            services.AddTransient<UploadService>();
            services.AddTransient<FileTypeChecker>();
            services.AddDbContext<GuideContext>(options =>
                    options.UseLazyLoadingProxies()
                        .UseNpgsql(connection))
                .AddIdentity<User, IdentityRole>(
                    options =>
                    { options.Password.RequiredLength = 6;   // минимальная длина
                        options.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                        options.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                        options.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                        options.Password.RequireDigit = false; // требуются ли цифры
                        options.User.AllowedUserNameCharacters = null;
                    }
                    )
                .AddEntityFrameworkStores<GuideContext>();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/Login");
            
            const int maxRequestLimit = 209715200;
            // If using IIS
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = maxRequestLimit;
            });
            // If using Kestrel
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = maxRequestLimit;
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = maxRequestLimit;
                x.MultipartBodyLengthLimit = maxRequestLimit;
                x.MultipartHeadersLengthLimit = maxRequestLimit;
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
                app.UseExceptionHandler("/Errors/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
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