using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public DateTime CurrentTime => DateTime.UtcNow.AddHours(6);
        [ViewData]
        public string SuccessMessage { get; set; } = string.Empty;
        [ViewData]
        public string FailedMessage { get; set; } = string.Empty;


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