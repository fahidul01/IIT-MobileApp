using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Student.Infrastructure.DBModel;

namespace Web.WebServices
{
    public class AppStartService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public AppStartService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            // Get the DbContext instance
            var db = scope.ServiceProvider.GetRequiredService<StudentDBContext>();
            await db.Database.MigrateAsync();

            var _roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<DBUser>>();
            var adminRole = await _roleManager.FindByNameAsync(AppConstants.Admin);
            if (adminRole == null)
            {
                adminRole = new IdentityRole(AppConstants.Admin);
                //create the roles and seed them to the database
                await _roleManager.CreateAsync(adminRole);
                await _roleManager.CreateAsync(new IdentityRole(AppConstants.Student));
            }
            //Assign Admin role to the main User here we have given our newly registered 
            //login id for Admin management
            var user = await _userManager.FindByNameAsync("admin");
            if (user == null)
            {

                user = new DBUser()
                {
                    UserName = "admin",
                    Email = "sakib.buet51@outlook.com",
                };
                await _userManager.CreateAsync(user, "pass_WORD_1234");
                await _userManager.AddToRoleAsync(user, "Admin");
            }

        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
