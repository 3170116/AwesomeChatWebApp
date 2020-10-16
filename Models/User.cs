using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwesomeChat.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Το email είναι υποχρεωτικό πεδίο*")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Δεν είναι email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Το password είναι υποχρεωτικό πεδίο*")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Απαιτείται μέγεθος τουλάχιστων 8 χαρακτήρων*")]
        [MaxLength(15, ErrorMessage = "Απαιτείται μέγεθος το πολύ 15 χαρακτήρων*")]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Απαραίτητο πεδίο. Με βάση αυτό θα σας αναζητούν οι υπόλοιποι χρήστες!")]
        [MinLength(5, ErrorMessage = "Ελάχιστος αριθμός χαρακτήρων 5!")]
        [MaxLength(20, ErrorMessage = "Μέγιστος αριθμός χαρακτήρων 20!")]
        public string NickName { get; set; }

        public DateTime LastLogOutDateTime { get; set; }

        public virtual ICollection<UserChatGroup> UserChatGroups { get; set; }

        [InverseProperty(nameof(Invitation.SendFromUser))]
        public virtual ICollection<Invitation> Invitations { get; set; }

        [InverseProperty(nameof(Invitation.SendToUser))]
        public virtual ICollection<Invitation> Requests { get; set; }

        [NotMapped]
        public bool IsOnline
        {
            get
            {
                return LastLogOutDateTime == new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
            }

            set
            {
                if (value == true)
                    LastLogOutDateTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
                else
                    LastLogOutDateTime = DateTime.UtcNow;
            }
        }

        [NotMapped]
        public IList<User> Friends { get; set; }

        [NotMapped]
        public IList<ChatGroup> ChatGroups { get; set; }

        public User()
        {
            this.UserChatGroups = new HashSet<UserChatGroup>();
            this.Invitations = new HashSet<Invitation>();
            this.Requests = new HashSet<Invitation>();
        }


        public bool IsEmailValid()
        {
            if (String.IsNullOrEmpty(Email))
                return false;

            Regex emailRegex = new Regex(@"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$");

            return emailRegex.Match(Email.ToLower()).Success;
        }

        public bool IsPasswordValid()
        {
            if (String.IsNullOrEmpty(Password))
                return false;

            return Password.Length >= 8 && Password.Length <= 15;
        }

        public bool IsNickNameValid()
        {
            if (String.IsNullOrEmpty(NickName))
                return false;

            return NickName.Length >= 5 && NickName.Length <= 20;
        }

        public int GetCommonChatGroupId(User user)
        {

            foreach (var myGroup in this.UserChatGroups)
            {
                foreach (var userGroup in user.UserChatGroups)
                {
                    if (myGroup.ChatGroupId == userGroup.ChatGroupId)
                        return myGroup.ChatGroupId;
                }
            }

            return 0;
        }

        public string GetActiveString()
        {
            DateTime utcNow = DateTime.UtcNow;

            int minutes = (utcNow - LastLogOutDateTime).Minutes;
            int hours = (utcNow - LastLogOutDateTime).Hours;

            if (hours == 24)
            {
                return "Ενεργός πριν μια ημέρα";
            }

            if (hours > 24)
            {
                return "Έχει πάνω από μια μέρα ανενργός";
            }

            if (hours > 0)
            {
                return "Ενεργός πριν από " + hours + " ώρες και " + minutes + " λεπτά";
            }

            return "Ενεργός πριν από " + minutes + " λεπτά";

        }

    }
}
