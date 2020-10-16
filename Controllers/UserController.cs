using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeChat.Models;
using AwesomeChat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AwesomeChat.Controllers
{
    public class UserController : BaseController
    {

        public UserController(ApplicationContext db, IStringLocalizer<UserController> localizer) : base(db, localizer)
        {

        }

        public ActionResult Profile()
        {

            //ελέγχουμε αν υπάρχει κάποιος χρήστης που έχει κάνει login
            string userEmail = GetUserEmail();

            if (String.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index", "Home");
            }

            User user = db.Users.Single(x => x.Email == userEmail);

            db.InitUserChatGroups(user);

            db.InitFriends(user);
            foreach (var friend in user.Friends)
            {
                db.InitUserChatGroups(friend);
            }

            db.InitRequests(user);

            ProfileVM vm = new ProfileVM()
            {
                User = user
            };

            return View(vm);
        }

        
        public ActionResult Search(ProfileVM vm)
        {
            //ελέγχουμε αν υπάρχει κάποιος χρήστης που έχει κάνει login
            string userEmail = GetUserEmail();

            if (String.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index", "Home");
            }

            User user = db.Users.Single(x => x.Email == userEmail);

            db.InitFriends(user);
            db.InitInvitations(user);
            db.InitRequests(user);

            //κάνουμε αναζήτηση με βάση το NickName
            var searchResults = String.IsNullOrWhiteSpace(vm.SearchText) ? new List<User>() : db.Users.Where(x => x.NickName.ToLower().Contains(vm.SearchText.TrimStart().TrimEnd().ToLower())).ToList();

            //ελέγχουμε αν υπάρχει στα αποτελέσματα ο χρήστης
            var searchUser = searchResults.SingleOrDefault(x => x.Id == user.Id);

            if (searchUser != null)
                searchResults.Remove(searchUser);


            SearchVM searchVM = new SearchVM()
            {
                User = user,
                Results = searchResults,
                SearchText = vm.SearchText
            };

            return View(searchVM);
        }

        [HttpPost]
        public string SendInvitation(int fromUserId, int toUserId)
        {

            //ελέγχουμε αν υπάρχει κάποιος χρήστης που έχει κάνει login
            string userEmail = GetUserEmail();

            if (String.IsNullOrEmpty(userEmail))
            {
                return "NOK";
            }

            User user = db.Users.Single(x => x.Email == userEmail);
            user.Invitations = db.Invitations.Where(x => x.SendFromUserId == user.Id).ToList();

            if (user.Id != fromUserId)
            {
                return "NOK";
            }

            if (fromUserId == toUserId)
            {
                return "NOK";
            }

            if (user.Invitations.Any(x => x.SendToUserId == toUserId))
            {
                return "NOK";
            }

            DateTime utcNow = DateTime.UtcNow;

            db.Invitations.Add(new Invitation()
            {
                SendFromUserId = fromUserId,
                SendToUserId = toUserId,
                CreatedAt = utcNow
            });
            db.SaveChanges();

            return "OK";

        }

        [HttpPost]
        public string DeleteInvitation(int invitationId)
        {

            //ελέγχουμε αν υπάρχει κάποιος χρήστης που έχει κάνει login
            string userEmail = GetUserEmail();

            if (String.IsNullOrEmpty(userEmail))
            {
                return "NOK";
            }

            User user = db.Users.Single(x => x.Email == userEmail);
            user.Invitations = db.Invitations.Where(x => x.SendFromUserId == user.Id).ToList();

            Invitation deletedInvitation = user.Invitations.SingleOrDefault(x => x.Id == invitationId);

            if (deletedInvitation == null)
            {
                return "NOK";
            }

            db.Invitations.Remove(deletedInvitation);
            db.SaveChanges();

            return "OK";

        }

        [HttpPost]
        public string ReplyToInvitation(int requestId, bool accept)
        {

            //ελέγχουμε αν υπάρχει κάποιος χρήστης που έχει κάνει login
            string userEmail = GetUserEmail();

            if (String.IsNullOrEmpty(userEmail))
            {
                return "NOK";
            }


            User user = db.Users.Single(x => x.Email == userEmail);
            user.Requests = db.Invitations.Where(x => x.SendToUserId == user.Id).ToList();

            var request = user.Requests.SingleOrDefault(x => x.Id == requestId);
            if (request == null)
            {
                return "NOK";
            }

            User requestUser = db.Users.Single(x => x.Id == request.SendFromUserId);

            if (accept)
            {

                var group = new ChatGroup()
                {
                    CreatedAt = DateTime.UtcNow,
                    LogoURL = "",
                    Name = ""
                };

                db.ChatGroups.Add(group);
                db.SaveChanges();

                var item1 = new UserChatGroup()
                {
                    UserId = user.Id,
                    ChatGroupId = group.Id
                };

                db.UserChatGroups.Add(item1);
                db.SaveChanges();

                var item2 = new UserChatGroup()
                {
                    UserId = requestUser.Id,
                    ChatGroupId = group.Id
                };

                db.UserChatGroups.Add(item2);
                db.SaveChanges();

            }

            db.Invitations.Remove(request);
            db.SaveChanges();


            return "OK";

        }

        public ActionResult ShowMessages(int groupId)
        {

            //ελέγχουμε αν υπάρχει κάποιος χρήστης που έχει κάνει login
            string userEmail = GetUserEmail();

            if (String.IsNullOrEmpty(userEmail))
            {
                return Content("");
            }


            User user = db.Users.Single(x => x.Email == userEmail);

            MessagesVM vm = new MessagesVM()
            {
                User = user,
                Messages = db.GetTop30Messages(groupId)
            };
            vm.Messages = vm.Messages.Reverse().ToList();

            return PartialView("~/Views/User/PartialViews/Messages.cshtml", vm);
        }

        [HttpPost]
        public string SendMessage(int groupId, string body)
        {

            //ελέγχουμε αν υπάρχει κάποιος χρήστης που έχει κάνει login
            string userEmail = GetUserEmail();

            if (String.IsNullOrEmpty(userEmail))
            {
                return "NOK";
            }


            User user = db.Users.Single(x => x.Email == userEmail);
            ChatGroup chatGroup = db.ChatGroups.Single(x => x.Id == groupId);

            db.Messages.Add(new Message()
            {
                ChatGroupId = chatGroup.Id,
                SendByUserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                Body = body
            });
            db.SaveChanges();

            return "OK";

        }

    }
}