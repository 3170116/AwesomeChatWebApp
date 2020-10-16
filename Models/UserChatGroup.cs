using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeChat.Models
{
    public class UserChatGroup
    {

        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ChatGroupId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ChatGroupId")]
        public virtual ChatGroup ChatGroup { get; set; }

    }
}
