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
        [Command("countdown")]
        [Alias("cd")]
        public async Task CountdownCommand(int seconds = 10)
        {
            // initialize empty string builder for reply
            var sb = new StringBuilder();

            // get user info from the Context
            var user = Context.User;

            // build out the reply
            sb.AppendLine($"[{user.Username}] has initiated a countdown for " + seconds + " seconds");
            sb.AppendLine("I am not already coded to do it but I will soon !");

            // send simple string reply
            await ReplyAsync(sb.ToString());
        }
    }
}