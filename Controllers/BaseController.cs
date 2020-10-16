using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeChat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AwesomeChat.Controllers
{
    public class BaseController : Controller
    {

        protected readonly IStringLocalizer<BaseController> localizer;
        protected readonly ApplicationContext db;

        public BaseController(ApplicationContext db, IStringLocalizer<BaseController> localizer)
        {
            this.db = db;
            this.localizer = localizer;
        }

        public void SaveUserEmailToSession(string email)
        {
            HttpContext.Session.SetString("User", email);
        }

        public string GetUserEmail()
        {
            return HttpContext.Session.GetString("User");
        }
    }
}