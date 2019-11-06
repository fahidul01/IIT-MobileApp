using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public abstract class BaseController : Controller
    {
        [ViewData]
        public string SuccessMessage { get; set; } = string.Empty;
        [ViewData]
        public string FailedMessage { get; set; } = string.Empty;

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        protected internal string Translate(string data)
        {
            return data.Replace("_", " ");
        }

        protected internal void Success()
        {
            SuccessMessage = Translate("Operation_Success");
        }

        protected internal void Failed(string data, bool translate = false)
        {
            if (translate)
            {
                data = Translate(data);
            }

            FailedMessage = data;
        }
    }
}