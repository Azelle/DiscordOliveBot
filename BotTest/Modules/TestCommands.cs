using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using System;
//using System.Timers;
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
        [Command("t", RunMode = RunMode.Async)]
        public async Task Test2Command(int cd = 10, int period = 1, int startDelay = 0)
        {
            // get user info from the Context
            IUser user = Context.User;
            TimeSpan targetTime = TimeSpan.FromSeconds(cd);
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime + targetTime;
            int lastCount = cd;

            IUserMessage message = await ReplyAsync($"[{user.Username}] : " + cd);
            Console.WriteLine(cd + " @ " + DateTime.Now);
            // Create an AutoResetEvent to signal the timeout threshold in the
            // timer callback has been reached.
            //AutoResetEvent autoEvent = new AutoResetEvent(false);

            //StatusChecker statusChecker = new StatusChecker(cd, message, user.Username);
            //Timer timer = new Timer(statusChecker.CheckStatus, autoEvent, startDelay * 1000, period * 1000);

            //int i = 0;

            while (DateTime.Now < endTime)
            {
                int count = (endTime - DateTime.Now).Seconds;
                if (count != lastCount)
                {
                    lastCount = count;
                    Console.WriteLine(count + " @ " + DateTime.Now);
                    await message.ModifyAsync(msg => msg.Content = $"[{user.Username}] : " + count);
                    Console.WriteLine("after await @ " + DateTime.Now);
                }
                /*else
                {
                    i++;
                    Console.WriteLine("Too fast " + i + " @ " + DateTime.Now);
                }*/
            }

            //autoEvent.WaitOne();
            //autoEvent.Dispose();
            //timer.Dispose();
            Console.WriteLine("just before end @ " + DateTime.Now);
            await ReplyAsync("Test accomplished !");
            Console.WriteLine("end @ " + DateTime.Now);
        }

        [Command("test", RunMode = RunMode.Async)]
        public async Task TestCommand(int cd = 10, int period = 1, int startDelay = 0)
        {
            // get user info from the Context
            IUser user = Context.User;
            TimeSpan targetTime = TimeSpan.FromSeconds(cd);

            IUserMessage message = await ReplyAsync($"[{user.Username}] : " + cd);
            Console.WriteLine(cd + " @ " + DateTime.Now);
            // Create an AutoResetEvent to signal the timeout threshold in the
            // timer callback has been reached.
            AutoResetEvent autoEvent = new AutoResetEvent(false);

            StatusChecker statusChecker = new StatusChecker(cd, message, user.Username);
            Timer timer = new Timer(statusChecker.CheckStatus, autoEvent, startDelay * 1000, period * 1000);

            autoEvent.WaitOne();
            autoEvent.Dispose();
            timer.Dispose();
            Console.WriteLine("just before end @ " + DateTime.Now);
            await ReplyAsync("Test accomplished !");
            Console.WriteLine("end @ " + DateTime.Now);
        }

        class StatusChecker
        {
            private int invokeCount;
            private int minCount;

            private IUserMessage msg;
            private string user;

            public StatusChecker(int count, IUserMessage message, string userName)
            {
                invokeCount = count;
                minCount = 0;
                user = userName;
                msg = message;
            }

            // This method is called by the timer delegate.
            public void CheckStatus(Object stateInfo)
            {
                AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
                invokeCount -= 1;
                if (invokeCount <= minCount)
                {
                    Console.WriteLine("end of timer @ " + DateTime.Now);
                    autoEvent.Set();
                    return;
                }
                msg.ModifyAsync(msg => msg.Content = user + " : " + invokeCount);
                Console.WriteLine(invokeCount + " @ " + DateTime.Now);
            }
        }
    }
}
