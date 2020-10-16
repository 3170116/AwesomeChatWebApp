using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeChat.Models
{
    public class Message
    {

        [Key]
        public int Id { get; set; }

        public int ChatGroupId { get; set; }

        public int SendByUserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Body { get; set; }

        [ForeignKey("SendByUserId")]
        public virtual User SendByUser { get; set; }

    }
}
