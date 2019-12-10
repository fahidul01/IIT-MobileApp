using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Jdenticon.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Web.Infrastructure.AppServices;
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

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StudentDBContext>(opt =>
                 opt.UseSqlite("Filename=mydata.db"));
            //services.AddDbContext<StudentDBContext>(opt =>
            //   opt.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Temp\MITServer.mdf;Integrated Security=True;Connect Timeout=300"));
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

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.SlidingExpiration = true;
                options.LoginPath = "/Login";
            });
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<TokenService>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddHostedService<AppStartService>();

            services.AddControllersWithViews()
                    .AddNewtonsoftJson(x =>
                    {
                        x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        x.SerializerSettings.MaxDepth = 3;
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            AppConstants.DataPath = env.WebRootPath;
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

            //app.UseHttpsRedirection();
            app.UseJdenticon();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                   name: "ActionApi",
                   pattern: "api/{controller}/{action}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
          
        }
    }
}
