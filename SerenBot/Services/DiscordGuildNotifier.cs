using System.Collections.Generic;
using System.Threading.Tasks;
using SerenBot.Entities;
using SerenBot.Entities.Models;
using SerenBot.Models;
using SerenBot.Services.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Discord.Webhook;
using Discord;
using System.Text;
using Microsoft.Extensions.Configuration;
using SerenBot.Discord.Ultils;
using System;
using Microsoft.Extensions.Logging;

namespace SerenBot.Services
{
    public class DiscordGuildNotifier : IDiscordGuildNotifier
    {
        private readonly SerenDbContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<DiscordGuildNotifier> _logger;

        public DiscordGuildNotifier(SerenDbContext context, IConfiguration config, ILogger<DiscordGuildNotifier> logger) {
            _context = context;
            _config = config;
            _logger = logger;
        }

        public async Task SendVosNotifications(VoiceOfSeren[] voices)
        {
            List<Guild> notifyGuilds = await _context.Guilds.Where(g => 
                g.NotificationWebhookId != default(ulong) 
                && !string.IsNullOrEmpty(g.NotificationWebhookToken)
            ).ToListAsync();

            List<Task> tasks = new List<Task>();

            foreach (Guild guildEntity in notifyGuilds)
            {
                tasks.Add(SendWebHook(guildEntity, voices));
            }

            Task.WaitAll(tasks.ToArray());
        }

        private async Task SendWebHook(Guild guildEntity, VoiceOfSeren[] voices) {
                Embed embed = await BuildEmbed(guildEntity, voices);
                try {
                    DiscordWebhookClient client = new DiscordWebhookClient(guildEntity.NotificationWebhookId, guildEntity.NotificationWebhookToken);
                    StringBuilder mentionstring = new StringBuilder();
                    if (guildEntity.MentionsEnabled) {
                        foreach (VoiceOfSeren vos in voices)
                        {
                            mentionstring.Append($"<@&{guildEntity.GetRoleIdByVos(vos)}> ");
                        }
                    }
                    await client.SendMessageAsync($"{mentionstring.ToString()}", embeds: new [] {embed});
                } catch (Exception e) {
                    _logger.LogError(e.Message);
                }
        }

        private Task<Embed> BuildEmbed(Guild guildEntity, VoiceOfSeren[] voices) {
            var builder = new EmbedBuilder();

            StringBuilder text = new StringBuilder();
            foreach (VoiceOfSeren vos in voices)
            {
                ulong emojiId = guildEntity.GetEmojiIdByVos(vos);
                string emojiString = guildEntity.EmotesEnabled ? $"<:{vos.ToString()}:{emojiId}>\t" : "";
                text.AppendLine($"{emojiString}{vos.ToString()}");
            }

            builder.WithColor(Color.Blue);
            builder.WithCurrentTimestamp();
            builder.AddField(" 	__**Active Districts**__", $"{text.ToString()}");

            return Task.FromResult(builder.Build());
        }
    }
}