using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AwesomeChat.Models;
using AwesomeChat.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AwesomeChat.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(ApplicationContext db, IStringLocalizer<HomeController> localizer) : base(db, localizer)
        {
            
        }

        public ActionResult Index()
        {

            //ελέγχουμε αν υπάρχει ήδη χρήστης που έχει κάνει login
            string userEmail = GetUserEmail();

            if (!String.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Profile", "User");
            }

            LoginVM vm = new LoginVM()
            {
                Localizer = localizer
            };

            return View(vm);
        }

        public ActionResult Register(RegisterVM vm)
        {

            if (vm == null)
            {
                vm = new RegisterVM();
            }

            return View(vm);

        }

    }
}
