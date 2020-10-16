using AwesomeChat.Controllers;
using AwesomeChat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeChat.ViewModels
{
    public class BaseVM
    {

        public IStringLocalizer<BaseController> Localizer { get; set; }

        public ISet<string> ErrorMessages { get; set; }

        public User User { get; set; }


        public BaseVM()
        {
            ErrorMessages = new HashSet<string>();
        }

        private void AddErrorMessage(string key)
        {

            if (ErrorMessages == null)
            {
                ErrorMessages = new HashSet<string>();
            }

            if (!ErrorMessages.Contains(key))
                ErrorMessages.Add(key);

        }

        private string GetErrorMessage(string key)
        {
            if (!ErrorMessages.Contains(key))
            {
                return "";
            }

            return key;
            //return Localizer[key];
        }

        public void AddErrorMessageForInvalidEmail()
        {
            AddErrorMessage("INVALID_EMAIL");
        }

        public string GetErrorMessageForInvalidEmail()
        {
            return GetErrorMessage("INVALID_EMAIL");
        }

        public void AddErrorMessageForNoAccount()
        {
            AddErrorMessage("INVALID_ACCOUNT");
        }

        public string GetErrorMessageForNoAccount()
        {
            return GetErrorMessage("INVALID_ACCOUNT");
        }

        public void AddErrorMessageForAccountExist()
        {
            AddErrorMessage("ACCOUNT_EXIST");
        }

        public string GetErrorMessageForAccountExist()
        {
            return GetErrorMessage("ACCOUNT_EXIST");
        }

    }
}
