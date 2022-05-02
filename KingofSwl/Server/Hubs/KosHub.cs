using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScixingTetrisCore;
using ScixingTetrisCore.Battle;
using ScixingTetrisCore.Interface;
using System.Diagnostics;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace KingofSwl.Server.Hubs
{
    public class KosHub: Hub
    {
        class BattlePlayer
        {
            public string user1, user2;
        }
        public static List<string> test = new();
        //public static List<string> KosBattles = new();
        static BattlePlayer[] Battles = new BattlePlayer[1000];
        static KosBattle[] KosBattles = new KosBattle[1000];
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
            Debug.WriteLine("当前登陆id: " + Context.ConnectionId);
            //Clients.User.
            if (id >= 1000) return;
            if (Battles[id] == null)
            {
                Battles[id] = new BattlePlayer();
                KosBattles[id] = new();
                Battles[id].user1 = Context.ConnectionId;
                //await Clients.Caller.SendAsync("ReceiveMessage", "1232", "456");
                //var ff = JsonSerializer.Serialize(KosBattles[id].Player1.CreateClient());
                //await Clients.Caller.SendAsync("Initstring", KosBattles[id].Player1.CreateClient(), 1);
                //await Clients.Caller.SendAsync("Initstring", KosBattles[id].Player1.Field, KosBattles[id].Player1.NextQueue.Select(s => s.MinoType), ); ;
                await Clients.Caller.SendAsync("Initstring", KosBattles[id].Player1.CreateMessage(7) ); 
                //await Clients.Caller.SendAsync("Initstring", 1, 1);
            }
            else if (Battles[id].user2 == default)
            {
                Battles[id].user2 = Context.ConnectionId;
                await Clients.Caller.SendAsync("Initstring", KosBattles[id].Player2.CreateMessage(7));
            }
            else if (Battles[id].user1 == default)
            {
                Battles[id].user1 = Context.ConnectionId;
                //await Clients.Caller.SendAsync("Initstring", 1);
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
                //if (Battles[bid].user2 == Context.ConnectionId)
                //{
                //    System.Console.WriteLine("very");
                //}
                if (Battles[bid].user1 != null)
                {
                    await Clients.Client(Battles[bid].user1).SendAsync("updateField", field, hold, nextQueue);
                }
            }
            //await Clients.Others.SendAsync("updateField", field, hold, nextQueue);
        }
        public async Task SendAtk(int bid, List<int> garbage)
        {
            if (Battles[bid].user1 == Context.ConnectionId)
            {
                //Context.User.Identity.
                //System.Console.WriteLine(Context);
                if (Battles[bid].user2 != null)
                {
                    await Clients.Client(Battles[bid].user2).SendAsync("GetAtk", garbage);
                }
            }
            else if (Battles[bid].user2 == Context.ConnectionId)
            {
                //System.Console.WriteLine(Context);
                //if (Battles[bid].user2 == Context.ConnectionId)
                //{
                //    System.Console.WriteLine("very");
                //}
                if (Battles[bid].user1 != null)
                {
                    await Clients.Client(Battles[bid].user1).SendAsync("GetAtk", garbage);
                }
            }
        }
        // 要不要判断一下是谁提交的，回合数等
        public async Task Commit(int bid, List<List<MoveType>> moveTypes)
        {
            // 判断一下id
            KosBattles[bid].CommitMove(moveTypes);
            // 强制刷新两者场地
            // 如谁应该动等等
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Debug.WriteLine("当前断线id: " + Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

    }
}
