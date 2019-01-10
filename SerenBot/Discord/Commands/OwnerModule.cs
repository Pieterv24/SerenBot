using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Webhook;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SerenBot.Entities;
using SerenBot.Entities.Models;

namespace SerenBot.Discord.Commands
{
    [RequireOwner]
    [Group("Owner"), Alias("O", "")]
    public class OwnerModule : ModuleBase
    {
        private SerenDbContext _context;
        private ILogger<OwnerModule> _logger;

        public OwnerModule(SerenDbContext context, ILogger<OwnerModule> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Command("BroadcastMessage")]
        public async Task BroadcastMessage([Remainder]string message) {
            List<Guild> guilds = await _context.Guilds.ToListAsync();

            foreach (Guild guild in guilds)
            {
                if (guild.NotificationWebhookId != default(ulong) && guild.NotificationWebhookToken != null) {
                    try {
                        DiscordWebhookClient client = new DiscordWebhookClient(guild.NotificationWebhookId, guild.NotificationWebhookToken);
                        await client.SendMessageAsync($"@everyone Incomming bot service message.", embeds: new [] {CreateBroadcastEmbed(message)});
                    } catch (Exception e) {
                        _logger.LogError(e.Message);
                    }
                } else {
                    await Context.Message.Channel.SendMessageAsync($"Could not deliver message to guild: {guild.GuildName}");
                }
            }

            await Context.Message.Channel.SendMessageAsync("Broadcast send");
        }

        private Embed CreateBroadcastEmbed(string message) {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithAuthor(author => {
                author.Name = Context.User.Username;
                author.IconUrl = Context.User.GetAvatarUrl();
            });
            builder.WithColor(Color.Gold);
            builder.WithDescription(message);
            builder.WithCurrentTimestamp();

            return builder.Build();
        }
    }
}