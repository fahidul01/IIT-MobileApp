using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IIT.Client.Services
{
    public class ApiAuthenticationProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(100);
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = new User()
                {
                    Name = "Admin",
                    UserName = AppConstants.Admin,
                    Role = AppConstants.Admin
                };
                if (userInfo != null)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, userInfo.Name) };
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}
