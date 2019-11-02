using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Jdenticon.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Web.Infrastructure.DBModel;
using Web.Infrastructure.Services;
using Web.Models.Web;
using Web.WebServices;

namespace Web
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
            services.AddDbContext<StudentDBContext>(opt =>
                 opt.UseSqlite("Filename=mydata.db"));
            services.RegisterAllTypes<BaseService>(typeof(StudentDBContext).Assembly);

            services.AddIdentity<DBUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
                    .AddEntityFrameworkStores<StudentDBContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.SlidingExpiration = true;
                options.LoginPath = "/Accounts/Login";
            });
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
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
            app.UseJdenticon();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            CreateUserRoles(serviceProvider);
        }

        private async void CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<DBUser>>();
            //Adding Admin Role
            var adminRole = await RoleManager.FindByNameAsync(AppConstants.Admin);
            if (adminRole == null)
            {
                adminRole = new IdentityRole(AppConstants.Admin);
                //create the roles and seed them to the database
                await RoleManager.CreateAsync(adminRole);
                await RoleManager.CreateAsync(new IdentityRole(AppConstants.Student));
            }
            //Assign Admin role to the main User here we have given our newly registered 
            //login id for Admin management
            var user = await UserManager.FindByNameAsync("admin");
            if (user == null)
            {

                user = new DBUser()
                {
                    UserName = "admin",
                    Email = "sakib.buet51@outlook.com",
                };
                await UserManager.CreateAsync(user, "pass_WORD_1234");
            }
            await UserManager.AddToRoleAsync(user, "Admin");
        }
    }
}
