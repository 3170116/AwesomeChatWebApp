﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(int fromUserId, int groupId)
        {
            await Clients.All.SendAsync("ReceiveMessage", fromUserId, groupId);
        }
    }

}
