using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SerenBot.Discord.Commands;
using SerenBot.Entities;
using SerenBot.Services.Interfaces;

namespace SerenBot.HostedServices
{
    public class DiscordBotService : IHostedService
    {
        private IConfiguration _config;
        private ILogger _logger;
        private DiscordShardedClient _client;
        private CommandService _commands;
        private SerenDbContext _context;
        private IServiceProvider _services;

        public DiscordBotService(
            IConfiguration config,
            IServiceProvider services,
            ILogger<DiscordBotService> logger,
            SerenDbContext context
        )
        {
            _config = config;
            _logger = logger;
            _context = context;
            _services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            DiscordSocketConfig config = new DiscordSocketConfig()
            {
                TotalShards = _config.GetValue<int>("Discord:Shards")
            };   

            _client = new DiscordShardedClient(config);
            _commands = new CommandService();
            await SetupCommands();

            _client.Log += Log;
            _client.ShardReady += SetupShard;

            await _client.SetActivityAsync(new Game("The Voice of Seren", ActivityType.Listening));

            await _client.LoginAsync(TokenType.Bot, _config.GetValue<string>("Discord:BotToken"));
            await _client.StartAsync();
        }

        private Task SetupShard(DiscordSocketClient client)
        {
            _logger.LogInformation("Setting up shard");
            client.JoinedGuild += OnGuildJoined;
            client.MessageReceived += HandleCommand;
            foreach (SocketGuild clientGuild in client.Guilds)
            {
                _logger.LogInformation($"Connected to guild: {clientGuild.Name}");
            }

            return Task.CompletedTask;
        }

        private async Task SetupCommands()
        { 
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private Task OnGuildJoined(SocketGuild guild)
        {
            _logger.LogDebug($"Joined guild: {guild.Name}");

            return Task.CompletedTask;
        }

        private async Task HandleCommand(SocketMessage messageParam)
        {
            if (messageParam.Author.Id == _client.CurrentUser.Id) return;
            if (messageParam.Author.IsBot || messageParam.Author.IsWebhook) return;

            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            string prefix = ">>";
            var context = new CommandContext(_client, message);
            if (context.Guild != null)
            {
                string guildPrefix = await _context.Guilds.Where(g => g.GuildId == context.Guild.Id).Select(g => g.Prefix).FirstOrDefaultAsync();
                if (!string.IsNullOrWhiteSpace(guildPrefix) && guildPrefix != prefix)
                {
                    prefix = guildPrefix;
                }
            }

            if (!(message.HasStringPrefix(prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos) || context.IsPrivate))
                return;

            if (argPos >= message.Content.Length)
                return;

            while (char.IsWhiteSpace(message.Content[argPos]) && argPos < message.Content.Length )
            {
                argPos++;
            }

            var result = await _commands.ExecuteAsync(context, argPos, _services);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.ErrorReason);
                switch (result.Error) {
                    case CommandError.UnknownCommand:
                    case CommandError.BadArgCount:
                    case CommandError.UnmetPrecondition:
                        await context.Channel.SendMessageAsync(result.ErrorReason);
                        break;
                    default:
                        await context.Channel.SendMessageAsync("Unknown Error");
                        break;
                }
                // await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.LogoutAsync();
            await _client.StopAsync();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
