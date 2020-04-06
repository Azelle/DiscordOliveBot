using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace BotTest.Modules
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class CountdownCommands : ModuleBase
    {
        [Command("countdown", RunMode = RunMode.Async)]
        [Alias("cd")]
        public async Task CountdownCommand(int seconds = 10)
        {
            // get user info from the Context
            IUser user = Context.User;

            IUserMessage message = await ReplyAsync($"[{user.Username}] : " + seconds);
            await message.PinAsync();

            // Countdown
            for (int i = seconds - 1; i >= 0; i--)
            {
                System.Threading.Thread.Sleep(1000);
                await message.ModifyAsync(msg => msg.Content = $"[{user.Username}] : " + i);
            }
            await ReplyAsync("Test accomplished !");
            await message.UnpinAsync();
        }
    }
}