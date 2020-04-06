using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace BotTest.Modules
{
    public class TestCommands : ModuleBase
    {
        [Command("test", RunMode = RunMode.Async)]
        public async Task TestCommand(double cd = 10)
        {
            // get user info from the Context
            var user = Context.User;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            TimeSpan timeSpan = stopWatch.Elapsed;
            TimeSpan targetTime = TimeSpan.FromSeconds(cd);

            var message = await ReplyAsync($"[{user.Username}] : " + targetTime);

            int ms = 500;

            // Countdown
            while (targetTime > timeSpan)
            {
                System.Threading.Thread.Sleep(ms);
                TimeSpan temp = targetTime - timeSpan;
                timeSpan = stopWatch.Elapsed;
                //ms = temp.Milliseconds;
                await message.ModifyAsync(msg => msg.Content = $"[{user.Username}] : " + temp);
            }
            await ReplyAsync("Test accomplished !");
        }

        [Command("t", RunMode = RunMode.Async)]
        public async Task TCommand(int cd = 10)
        {
            // Initialize a queue and a CountdownEvent
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>(Enumerable.Range(0, cd * 1000));
            CountdownEvent cde = new CountdownEvent(cd * 1000); // initial count = 10000

            // This is the logic for all queue consumers
            Action consumer = () =>
            {
                int local;
                // decrement CDE count once for each element consumed from queue
                while (queue.TryDequeue(out local)) cde.Signal();
            };

            cde.Wait();

            // get user info from the Context
            var user = Context.User;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            TimeSpan timeSpan = stopWatch.Elapsed;
            TimeSpan targetTime = TimeSpan.FromSeconds(cd);

            var message = await ReplyAsync($"[{user.Username}] : " + targetTime);

            int ms = 500;

            // Countdown
            while (targetTime > timeSpan)
            {
                System.Threading.Thread.Sleep(ms);
                TimeSpan temp = targetTime - timeSpan;
                timeSpan = stopWatch.Elapsed;
                //ms = temp.Milliseconds;
                await message.ModifyAsync(msg => msg.Content = $"[{user.Username}] : " + temp);
            }
            await ReplyAsync("Test accomplished !");
        }
    }
}
