using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerInPyjamas.Hubs
{
    public class DataFlow : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.Client("").SendAsync(message);
        }

    }
}
