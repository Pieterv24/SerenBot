using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SerenBot.Discord.Ultils;
using SerenBot.Entities;
using System.Linq;
using SerenBot.Entities.Models;
using Discord.WebSocket;
using Discord.Webhook;
using System.IO;
using SerenBot.Models;
using Discord.Rest;

namespace SerenBot.Discord.Commands
{
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireContext(ContextType.Guild)]
    [Group("A"), Alias("Admin")]
    public class AdminModule : ModuleBase
    {
        private SerenDbContext _context;

        public AdminModule(SerenDbContext context)
        {
            _context = context;
        }

        [Command("SetupRoles")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task SetupRoles() {
            var guildRoles = Context.Guild.Roles;
            Guild guild = await _context.Guilds.FindAsync(Context.Guild.Id);
            if (guild == null) {
                guild = new Guild() {
                    GuildId = Context.Guild.Id,
                    GuildName = Context.Guild.Name
                };
                await _context.AddAsync(guild);
                await _context.SaveChangesAsync();
            }
            foreach(VoiceOfSeren voice in (VoiceOfSeren[]) Enum.GetValues(typeof(VoiceOfSeren))) {
                // Add roles to server
                if(!guildRoles.Where(gr => gr.Name == voice.ToString()).Any()) {
                    RestRole role = await Context.Guild.CreateRoleAsync(voice.ToString(), permissions: new GuildPermissions(0), color: voice.GetColorByVos()) as RestRole;
                    await role.ModifyAsync(props => {
                        props.Mentionable = true;
                    });
                    guild.SetRoleIdByVos(voice, role.Id);
                }
            }
            guild.MentionsEnabled = true;
            _context.Guilds.Update(guild);
            await _context.SaveChangesAsync();
            await Context.Channel.SendMessageAsync("Roles were added");
        }

        [Command("SetupEmotes")]
        [RequireBotPermission(GuildPermission.ManageEmojis)]
        public async Task SetupEmoji() {
            var guildEmoji = Context.Guild.Emotes;
            Guild guild = await _context.Guilds.FindAsync(Context.Guild.Id);
            if (guildEmoji.Count + 8 <= 50) {
                if (guild == null) {
                    guild = new Guild() {
                        GuildId = Context.Guild.Id,
                        GuildName = Context.Guild.Name
                    };
                    await _context.AddAsync(guild);
                    await _context.SaveChangesAsync();
                }
                foreach(VoiceOfSeren voice in (VoiceOfSeren[]) Enum.GetValues(typeof(VoiceOfSeren))) {
                    // Add emotes to server
                    if (!guildEmoji.Where(ge => ge.Name.ToLowerInvariant() == voice.ToString().ToLowerInvariant()).Any()) {
                        GuildEmote emote = await Context.Guild.CreateEmoteAsync(voice.ToString(), new Image($"assets/{voice.ToString()}_Clan.png"));
                        guild.SetEmojiIdByVos(voice, emote.Id);
                    }
                }
                guild.EmotesEnabled = true;
                _context.Guilds.Update(guild);
                await _context.SaveChangesAsync();
                await Context.Channel.SendMessageAsync("Emotes were added");
            } else
            {
                await Context.Channel.SendMessageAsync("You cannot add more than 50 emoji on your server. make sure at leas 8 can be added");
            }
        }

        [Command("SetChannel")]
        [RequireBotPermission(GuildPermission.ManageWebhooks)]
        public async Task SetChannel(ITextChannel channel)
        {
            if (await Context.CheckChannelPermissionsAsync(channel,
                new[] {ChannelPermission.ManageWebhooks}))
            {
                var webHooks = await (Context.Guild as SocketGuild).GetWebhooksAsync();
                IWebhook webHook;
                if (webHooks.Count == 0 || !webHooks.Any(wh => wh.Creator.Id == Context.Client.CurrentUser.Id)) {
                    webHook = await CreateWebhookAsync(channel);
                } else {
                    webHook = webHooks.First(wh => wh.Creator.Id == Context.Client.CurrentUser.Id);
                }

                if (channel.Id != webHook.ChannelId) {
                    await webHook.DeleteAsync();
                    webHook = await CreateWebhookAsync(channel);
                }

                Guild guild = await _context.Guilds.FindAsync(Context.Guild.Id);
                if (guild == null) {
                    guild = new Guild() {
                        GuildId = Context.Guild.Id,
                        GuildName = Context.Guild.Name,
                        NotificationWebhookId = webHook.Id,
                        NotificationWebhookToken = webHook.Token
                    };

                    await _context.Guilds.AddAsync(guild);
                    await _context.SaveChangesAsync();
                } else {
                    guild.NotificationWebhookId = webHook.Id;
                    guild.NotificationWebhookToken = webHook.Token;
                    
                    _context.Update(guild);
                    await _context.SaveChangesAsync();
                }

                await Context.Message.Channel.SendMessageAsync($"The voice of seren notification channel is now: {channel.Name}");
            } else {
                await Context.Message.Channel.SendMessageAsync("The bot has insufficient privileges to talk in that channel");
            }
        }

        [Command("SetPrefix")]
        public async Task SetCommandPrefix(string newPrefix) 
        {
            if (newPrefix.Length <= 0 || newPrefix.Length > 4) {
                await Context.Message.Channel.SendMessageAsync("The prefix cannot be longer then 4 characters");
            }

            Guild guild = await _context.Guilds.FindAsync(Context.Guild.Id);
            if (guild == null) {
                guild = new Guild() {
                    GuildId = Context.Guild.Id,
                    GuildName = Context.Guild.Name,
                    Prefix = newPrefix
                };

                await _context.Guilds.AddAsync(guild);
                await _context.SaveChangesAsync();
            } else {
                guild.Prefix = newPrefix;

                _context.Update(guild);
                await _context.SaveChangesAsync();
            }

            await Context.Message.Channel.SendMessageAsync("The prefix has been updated");
        }

        [Command("Uninstall")]
        [RequireOwner]
        public async Task Uninstall() {
            Guild guild = await _context.Guilds.FindAsync(Context.Guild.Id);

            if (guild != null) {
                _context.Guilds.Remove(guild);

                await _context.SaveChangesAsync();
            }

            var webHooks = await (Context.Guild as SocketGuild).GetWebhooksAsync();
            foreach (IWebhook wh in webHooks.Where(wh => wh.Creator.Id == Context.Client.CurrentUser.Id).ToList())
            {
                await wh.DeleteAsync();
            }

            var guildRoles = Context.Guild.Roles;
            foreach(VoiceOfSeren voice in (VoiceOfSeren[]) Enum.GetValues(typeof(VoiceOfSeren))) {
                IRole role = guildRoles.FirstOrDefault(gr => gr.Name == voice.ToString());
                if (role != null) {
                    await role.DeleteAsync();
                }
            }

            var guildEmotes = Context.Guild.Emotes;
            foreach(VoiceOfSeren voice in (VoiceOfSeren[]) Enum.GetValues(typeof(VoiceOfSeren))) {
                GuildEmote emote = guildEmotes.FirstOrDefault(ge => ge.Name == voice.ToString());
                if (emote != null) {
                    await Context.Guild.DeleteEmoteAsync(emote);
                }
            }

            await Context.Message.Channel.SendMessageAsync("Bot data was removed");
        }

        private async Task<IWebhook> CreateWebhookAsync(ITextChannel channel)
        {
            using(FileStream stream = File.Open(@"assets/seren.png", FileMode.Open)) {
                    return await channel.CreateWebhookAsync(Context.Client.CurrentUser.Username, stream);
                }
        }
    }
}
