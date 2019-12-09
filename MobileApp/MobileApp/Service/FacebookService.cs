using CoreEngine.Model.Common;
using Mobile.Core.Engines.Services;
using System.Threading.Tasks;

namespace MobileApp.Service
{
    public class FacebookService : IFacebookService
    {
        public bool IsLoggedIn => CheckLogin();

        private bool CheckLogin()
        {
            return false;
        }

        public async Task<ActionResponse> Login()
        {
            return null;
        }

        public async Task<ActionResponse> Logout()
        {
            return null;
        }

        public async Task<ActionResponse> Post(string title, string data)
        {
            return null;
        }
    }
}
