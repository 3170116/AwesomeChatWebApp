using AwesomeChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeChat.ViewModels
{
    public class SearchVM: ProfileVM
    {

        public IList<User> Results { get; set; }


        public SearchVM(): base()
        {

        }

    }
}
