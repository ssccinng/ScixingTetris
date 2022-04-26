using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KingofSwl.Server.Hubs
{
    public class KosHub: Hub
    {
        class BattlePlayer
        {
            public string user1, user2;
        }
        public static List<string> test = new();
        static BattlePlayer[] Battles = new BattlePlayer[100];
        public static string host;
        public async Task SendMessage(string user, string message)
        {
            test.Add(user + message);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendMessage1(string user, string message)
        {
            test.Add(user + message);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendInit(int id)
        {
            if (id >= 100) return;
            if (Battles[id] == null)
            {
                Battles[id] = new BattlePlayer();
                Battles[id].user1 = Context.ConnectionId;
                await Clients.Caller.SendAsync("Initstring", 1);
            }
            else if (Battles[id].user2 == default)
            {
                Battles[id].user2 = Context.ConnectionId;
                await Clients.Caller.SendAsync("Initstring", 2);
            }
            else if (Battles[id].user1 == default)
            {
                Battles[id].user1 = Context.ConnectionId;
                await Clients.Caller.SendAsync("Initstring", 1);
            }
            //System.Console.WriteLine("临流");
            //await Clients.All.SendAsync("ReceiveMessage", "11", "11");
            //if (host == null)
            //{
            //    host = "1";
            //    await Clients.Caller.SendAsync("Initstring", true);

            //}

            //else
            //{
            //    await Clients.Caller.SendAsync("Initstring", false);

            //}
        }

        public async Task UpdateField(int bid, byte[][] field, byte[][] hold, List<byte[][]> nextQueue)
        {
            if (Battles[bid].user1 == Context.ConnectionId)
            {
                //Context.User.Identity.
                //System.Console.WriteLine(Context);
                if (Battles[bid].user2 != null)
                {
                    await Clients.Client(Battles[bid].user2).SendAsync("updateField", field, hold, nextQueue);
                }
            }
            else
            {
                //System.Console.WriteLine(Context);
                if (Battles[bid].user2 == Context.ConnectionId)
                {
                    System.Console.WriteLine("very");
                }
                if (Battles[bid].user1 != null)
                {
                    await Clients.Client(Battles[bid].user1).SendAsync("updateField", field, hold, nextQueue);
                }
            }
            //await Clients.Others.SendAsync("updateField", field, hold, nextQueue);
        }
    }
}
