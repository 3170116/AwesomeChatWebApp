using AwesomeChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeChat.ViewModels
{
    public class ProfileVM: BaseVM
    {

        public string SearchText { get; set; }

        public ProfileVM(): base()
        {
            
        }

    }
}
