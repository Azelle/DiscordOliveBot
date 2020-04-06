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
        //static IUserMessage message;
        //static IUser user;

        [Command("test", RunMode = RunMode.Async)]
        public async Task TestCommand(int cd = 10)
        {
            // get user info from the Context
            IUser user = Context.User;
            //Timer timer = new Timer(1000);
            //timer.Elapsed += OnTimedEvent;
            //timer.AutoReset = true;
            //timer.Enabled = true;
            //TimeSpan timeSpan = timer.Elapsed;
            TimeSpan targetTime = TimeSpan.FromSeconds(cd);
            

            IUserMessage message = await ReplyAsync($"[{user.Username}] : 0");
            // Create an AutoResetEvent to signal the timeout threshold in the
            // timer callback has been reached.
            AutoResetEvent autoEvent = new AutoResetEvent(false);

            StatusChecker statusChecker = new StatusChecker(10, message, user.Username);
            //await message.PinAsync();
            Timer timer = new Timer(statusChecker.CheckStatus, autoEvent, 0, 1000);
            //int ms = 50;

            // Countdown
            /*while (targetTime > timeSpan)
            {
                System.Threading.Thread.Sleep(ms);
                //TimeSpan temp = targetTime - timeSpan;
                timeSpan = stopWatch.Elapsed;
                //ms = temp.Milliseconds;
                await message.ModifyAsync(msg => msg.Content = $"[{user.Username}] : " + timeSpan.Hours + "H" + timeSpan.Minutes + "m" + timeSpan.Seconds + "s" + timeSpan.Milliseconds);
            }*/
            //timer.Stop();
            //timer.Dispose();
            autoEvent.WaitOne();
            autoEvent.Dispose();
            timer.Dispose();
            await ReplyAsync("Test accomplished !");
            //await message.UnpinAsync();
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
                /*Console.WriteLine("{0} Checking status {1,2}.",
                    DateTime.Now.ToString("h:mm:ss.fff"),
                    (*/
                    --invokeCount//).ToString())
                    ;
                msg.ModifyAsync(msg => msg.Content = user + " : " + invokeCount);

                if (invokeCount == minCount)
                {
                    // Reset the counter and signal the waiting thread.
                    //invokeCount = 0;
                    autoEvent.Set();
                }
            }
        }


        /*private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            message.ModifyAsync(msg => msg.Content = $"[{user.Username}] : " + e.SignalTime);
        }*/

        /*[Command("t", RunMode = RunMode.Async)]
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
        }*/
    }
}
