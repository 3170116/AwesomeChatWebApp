using AwesomeChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeChat.ViewModels
{
    public class MessagesVM: ProfileVM
    {

        public IList<Message> Messages { get; set; }

        public MessagesVM(): base()
        {

        }

        public bool IsMessageMine(Message message)
        {
            return message.SendByUserId == User.Id;
        }

    }
}
