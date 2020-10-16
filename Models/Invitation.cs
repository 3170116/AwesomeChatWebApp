using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeChat.Models
{
    public class Invitation
    {

        [Key]
        public int Id { get; set; }

        public int SendFromUserId { get; set; }

        public int SendToUserId { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("SendFromUserId")]
        public virtual User SendFromUser { get; set; }

        [ForeignKey("SendToUserId")]
        public virtual User SendToUser { get; set; }

    }
}
