using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Infrastructure.DBModel;
using Microsoft.EntityFrameworkCore;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using CoreEngine.APIHandlers;
using IIT.Web.Controllers;
using Web.Api;
using IIT.Web.Helpers;
using CoreEngine.Model.Common;
using Web.Infrastructure.AppServices;
using IIT.Web.WebServices;
using MatBlazor;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace IIT.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

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
                    .AddEntityFrameworkStores<StudentDBContext>();


            //services.AddAuthentication(options =>
            //     {
            //         options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //     })
            //     .AddJwtBearer(x =>
            //     {
            //         x.RequireHttpsMetadata = false;
            //         x.SaveToken = true;
            //         x.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             ValidateIssuerSigningKey = true,
            //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
            //             ValidateIssuer = false,
            //             ValidateAudience = false
            //         };
            //     });

            services.AddTransient<ICourseHandler, CoursesController>();
            services.AddTransient<IMemberHandler, MemberController>();
            services.AddTransient<IBatchHandler, BatchesController>();
            services.AddTransient<ILessonHandler, LessonController>();
            services.AddTransient<INoticeHandler, NoticesController>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<TokenService>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddHostedService<AppStartService>();
            services.AddTransient<FilesController>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddControllers();

            services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });

            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();
            services.AddHttpClient();
            services.AddScoped<HttpClient>();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllerRoute(
                   name: "ActionApi",
                   pattern: "api/{controller}/{action}/{id?}");

            });
        }
    }
}
