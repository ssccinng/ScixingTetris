using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KingofSwl.Server.Hubs
{
    public class KosHub: Hub
    {
        public static List<string> test = new();
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
        public async Task SendInit()
        {
            //System.Console.WriteLine("临流");
            //await Clients.All.SendAsync("ReceiveMessage", "11", "11");
            if (host == null)
            {
                host = "1";
                await Clients.Caller.SendAsync("Initstring", true);

            }

            else
            {
                await Clients.Caller.SendAsync("Initstring", false);

            }
        }

        public async Task UpdateField(byte[][] field, byte[][] hold, List<byte[][]> nextQueue)
        {
            await Clients.Others.SendAsync("updateField", field, hold, nextQueue);
        }
    }
}
