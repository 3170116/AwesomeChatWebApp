using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeChat.Models
{
    public class ChatGroup
    {

        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public string LogoURL { get; set; }

        public virtual ICollection<UserChatGroup> UserChatGroups { get; set; }

        [NotMapped]
        public IList<Message> Messages { get; set; }


        public ChatGroup()
        {
            this.UserChatGroups = new HashSet<UserChatGroup>();
        }

        public IList<User> Members
        {
            get
            {
                return this.UserChatGroups.Select(x => x.User).ToList();
            }
        }

        public bool IsMember(int userId)
        {
            return UserChatGroups.SingleOrDefault(x => x.Id == userId) != null;
        }

    }
}
