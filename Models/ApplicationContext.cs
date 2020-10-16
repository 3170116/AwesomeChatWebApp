using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeChat.Models
{
    public class ApplicationContext: DbContext
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<UserChatGroup> UserChatGroups { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Message> Messages { get; set; }

        public void InitUserChatGroups(User user)
        {
            user.UserChatGroups = UserChatGroups.Where(x => x.UserId == user.Id).ToList();
        }

        public void InitFriends(User user)
        {

            var userChatGroups = UserChatGroups.Where(x => x.UserId == user.Id).ToList();
            user.Friends = new List<User>();

            foreach (var userChatGroup in userChatGroups)
            {
                user.Friends.Add(Users.Single(y => y.Id == UserChatGroups.Single(x => x.ChatGroupId == userChatGroup.ChatGroupId && x.UserId != user.Id).UserId));
            }

        }

        public void InitRequests(User user)
        {
            user.Requests = Invitations.Where(x => x.SendToUserId == user.Id).ToList();
            foreach (var request in user.Requests)
            {
                request.SendFromUser = Users.SingleOrDefault(x => x.Id == request.SendFromUserId);
            }
        }

        public void InitInvitations(User user)
        {
            user.Invitations = Invitations.Where(x => x.SendFromUserId == user.Id).ToList();
            foreach (var invitation in user.Invitations)
            {
                invitation.SendToUser = Users.SingleOrDefault(x => x.Id == invitation.SendToUserId);
            }
        }

        public IList<Message> GetTop30Messages(int chatGroupId)
        {
            return Messages.Where(x => x.ChatGroupId == chatGroupId).OrderByDescending(x => x.Id).Take(30).ToList();
        }

    }
}
