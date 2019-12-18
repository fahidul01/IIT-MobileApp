using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using IIT.Server.Helpers;
using IIT.Server.WebServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Student.Infrastructure.AppServices;
using Student.Infrastructure.DBModel;
using System.Text;
using Newtonsoft.Json;

namespace IIT.Server
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


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<TokenService>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddHostedService<AppStartService>();
            services.AddControllers()
                    .AddNewtonsoftJson(x =>
                     {
                         x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                         x.SerializerSettings.MaxDepth = 3;
                     });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseClientSideBlazorFiles<IIT.Client.Startup>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                  name: "ActionApi",
                  pattern: "api/{controller}/{action}/{id?}");
                endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
            });
        }
    }
}
