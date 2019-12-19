using CoreEngine.APIHandlers;
using CoreEngine.Helpers;
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
        private IMemberHandler _memberHandler;
        private SettingService _settingService;

        public ApiAuthenticationProvider(IMemberHandler memberHandler, SettingService settingService)
        {
            _memberHandler = memberHandler;
            _settingService = settingService;
        }

        public async Task<SignInResponse> Login(string username, string password, bool remember)
        {
            var res = await _memberHandler.Login(username, password);
            if(res != null && res.Success)
            {
                if (remember) _settingService.Token = res.Token;
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            return res;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await _memberHandler.TouchLogin();
                if (userInfo != null)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, userInfo.Name),
                        new Claim(ClaimTypes.Role, userInfo.Role),
                    };
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
