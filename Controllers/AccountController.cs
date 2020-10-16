using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using AwesomeChat.Models;
using AwesomeChat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AwesomeChat.Controllers
{
    public class AccountController : BaseController
    {

        public AccountController(ApplicationContext db, IStringLocalizer<AccountController> localizer) : base(db, localizer)
        {

        }

        [HttpPost]
        public ActionResult Login(LoginVM vm)
        {

            //ελέγχουμε αν το password είναι valid
            if (!vm.User.IsPasswordValid())
            {
                return RedirectToAction("Login", "Home");
            }

            //ελέγχουμε αν το email είναι valid
            if (!vm.User.IsEmailValid())
            {

                vm = new LoginVM()
                {
                    Localizer = localizer
                };
                vm.AddErrorMessageForInvalidEmail();

                return View("~/Views/Home/Index.cshtml", vm);
            }

            //ελέγχουμε αν δεν υπάρχει λογαριασμός με αυτά τα στοιχεία
            User loginUser = db.Users.SingleOrDefault(x => x.Email == vm.User.Email.ToLower());
            if (loginUser == null || loginUser.Password != vm.User.Password)
            {

                vm = new LoginVM()
                {
                    Localizer = localizer
                };
                vm.AddErrorMessageForNoAccount();

                return View("~/Views/Home/Index.cshtml", vm);
            }

            //αλλάζουμε το status του χρήστη σε online
            loginUser.IsOnline = true;
            db.SaveChanges();

            SaveUserEmailToSession(vm.User.Email.ToLower());

            return RedirectToAction("Profile", "User");
        }

        [HttpPost]
        public ActionResult CreateAccount(RegisterVM vm)
        {

            //ελέγχουμε αν το password και το nickName είναι valid
            if (!vm.User.IsPasswordValid() || !vm.User.IsNickNameValid())
            {
                return RedirectToAction("Login", "Home");
            }

            //ελέγχουμε αν το email είναι valid
            if (!vm.User.IsEmailValid())
            {
                vm = new RegisterVM();
                vm.AddErrorMessageForInvalidEmail();

                return View("~/Views/Home/Register.cshtml", vm);
            }

            //ελέγχουμε αν υπάρχει ήδη λογαριασμός με αυτά τα στοιχεία, αν δεν υπάρχει τον προσθέτουμε
            User registerUser = db.Users.SingleOrDefault(x => x.Email == vm.User.Email);
            if (registerUser != null)
            {
                vm = new RegisterVM();
                vm.AddErrorMessageForAccountExist();

                return View("~/Views/Home/Register.cshtml", vm);
            }

            registerUser = new User()
            {
                Email = vm.User.Email.ToLower(),
                Password = vm.User.Password,
                FirstName = vm.User.FirstName,
                LastName = vm.User.LastName,
                NickName = vm.User.NickName
            };

            //αλλάζουμε το status του χρήστη σε online
            registerUser.IsOnline = true;

            db.Users.Add(registerUser);
            db.SaveChanges();

            SaveUserEmailToSession(vm.User.Email.ToLower());


            return RedirectToAction("Profile", "User");

        }

        public ActionResult Edit()
        {

            string userEmail = GetUserEmail();

            if (String.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index", "Home");
            }

            User myUser = db.Users.Single(x => x.Email == userEmail);

            ProfileVM vm = new ProfileVM()
            {
                User = myUser
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(ProfileVM vm)
        {

            string userEmail = GetUserEmail();

            if (String.IsNullOrEmpty(userEmail) || userEmail != vm.User.Email)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!vm.User.IsNickNameValid())
            {
                return RedirectToAction("Index", "Home");
            }


            var editUser = db.Users.SingleOrDefault(x => x.Email == vm.User.Email);

            vm.User.FirstName = vm.User.FirstName ?? "";
            vm.User.LastName = vm.User.LastName ?? "";

            editUser.FirstName = vm.User.FirstName;
            editUser.LastName = vm.User.LastName;
            editUser.NickName = vm.User.NickName;

            db.SaveChanges();

            return RedirectToAction("Profile", "User");

        }

        public ActionResult Logout()
        {

            string userEmail = GetUserEmail();

            if (String.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index", "Home");
            }

            User myUser = db.Users.Single(x => x.Email == userEmail);

            myUser.IsOnline = false;
            db.SaveChanges();

            SaveUserEmailToSession("");

            return RedirectToAction("Index", "Home");
        }

    }
}