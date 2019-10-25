using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{
    public abstract class BaseController : Controller
    {
        [ViewData]
        public string SuccessMessage { get; set; }
        [ViewData]
        public string FailedMessage { get; set; }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //private LanguageService languageService;
        //internal string Translate(string data)
        //{
        //    if (languageService == null)
        //    {
        //        languageService = Startup.ServiceProvider
        //                                 .GetRequiredService<LanguageService>();
        //    }
        //    return languageService[data];
        //}

        internal protected string Translate(string data)
        {
            return data.Replace("_", " ");
        }

        internal protected void Success()
        {
            SuccessMessage = Translate("Operation_Success");
        }

        internal protected void Failed(string data, bool translate = false)
        {
            if (translate) data = Translate(data);
            FailedMessage = data;
        }
    }
}