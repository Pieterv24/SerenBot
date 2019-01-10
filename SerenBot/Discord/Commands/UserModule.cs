using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using SerenBot.Discord.Ultils;
using SerenBot.Entities;
using SerenBot.Entities.Models;
using SerenBot.Models;

namespace SerenBot.Discord.Commands
{
    [Group("User"), Alias("U", "")]
    [RequireContext(ContextType.Guild)]
    public class UserModule : ModuleBase
    {
        private readonly SerenDbContext _context;

        public UserModule(SerenDbContext context) {
                _context = context;
        }

        [Group("Notifications"), Alias("Notification")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        class VosUserModule : ModuleBase {
            private readonly SerenDbContext _context;
            private readonly IConfiguration _config;
            public VosUserModule(SerenDbContext context, IConfiguration config) {
                _context = context;
                _config = config;
            }
            
            [Command("Add")]
            public async Task SetUserNotification([Remainder]string voices) {   
                VoiceOfSeren[] vos = SplitVoices(voices);

                if (vos.Length == 0 && voices.ToLowerInvariant() == "all") {
                    vos = (VoiceOfSeren[])Enum.GetValues(typeof(VoiceOfSeren));
                }

                Guild guild = await _context.Guilds.FindAsync(Context.Guild.Id);
                if (!guild.MentionsEnabled || guild == null) {
                    await Context.Message.Channel.SendMessageAsync("Notifications are not enabled in this discord server, please ask an admin to enable it.");
                    return;
                }

                IGuildUser user = await Context.Guild.GetUserAsync(Context.User.Id);
                List<IRole> roles = new List<IRole>();
                foreach (VoiceOfSeren voice in vos)
                {
                    IRole role = Context.Guild.GetRole(guild.GetRoleIdByVos(voice));
                    roles.Add(role);
                }
                await user.AddRolesAsync(roles);

                await Context.Message.Channel.SendMessageAsync("Your notification settings were updated", embed: await CreateNotificationEmbed(guild, addedVoices: vos));
            }

            [Command("Remove"), Alias("Delete")]
            public async Task RemoveUserNotification([Remainder]string voices) {
                VoiceOfSeren[] vos = SplitVoices(voices);

                if (vos.Length == 0 && voices.ToLowerInvariant() == "all") {
                    vos = (VoiceOfSeren[])Enum.GetValues(typeof(VoiceOfSeren));
                }

                Guild guild = await _context.Guilds.FindAsync(Context.Guild.Id);
                if (!guild.MentionsEnabled || guild == null) {
                    await Context.Message.Channel.SendMessageAsync("Notifications are not enabled in this discord server, please ask an admin to enable it.");
                    return;
                }

                IGuildUser user = await Context.Guild.GetUserAsync(Context.User.Id);
                List<IRole> roles = new List<IRole>();
                foreach (VoiceOfSeren voice in vos)
                {
                    IRole role = Context.Guild.GetRole(guild.GetRoleIdByVos(voice));
                    roles.Add(role);
                }
                await user.RemoveRolesAsync(roles);

                await Context.Message.Channel.SendMessageAsync("Your notification settings were updated", embed: await CreateNotificationEmbed(guild, removedVoices: vos));
            }

            [Command("List"), Alias("Print", "Output")]
            public async Task PrintUserNotifications() 
            {
                Guild guild = await _context.Guilds.FindAsync(Context.Guild.Id);
                if (!guild.MentionsEnabled || guild == null) {
                    await Context.Message.Channel.SendMessageAsync("Notifications are not enabled in this discord server, please ask an admin to enable it.");
                    return;
                }

                await Context.Message.Channel.SendMessageAsync("", embed: await CreateNotificationEmbed(guild));
            }

            private async Task<Embed> CreateNotificationEmbed(Guild guild, IEnumerable<VoiceOfSeren> addedVoices = null, IEnumerable<VoiceOfSeren> removedVoices = null) {
                EmbedBuilder builder = new EmbedBuilder();

                var roleIds = (await Context.Guild.GetUserAsync(Context.User.Id)).RoleIds;

                StringBuilder activeVos = new StringBuilder();
                StringBuilder inactiveVos = new StringBuilder();
                List<VoiceOfSeren> vosRoles = new List<VoiceOfSeren>();
                List<VoiceOfSeren> removedVosRoles = removedVoices != null ? new List<VoiceOfSeren>(removedVoices) : null; 

                foreach (ulong roleId in roleIds)
                {
                    var vos = guild.GetVosByRoleId(roleId);
                    if (vos != null) {
                        vosRoles.Add(vos.Value);
                    }
                }

                if (addedVoices != null) {vosRoles.AddRange(addedVoices);};

                foreach(VoiceOfSeren vos in (VoiceOfSeren[])Enum.GetValues(typeof(VoiceOfSeren))) {
                    string emojiString = guild.EmotesEnabled ? $"<:{vos.ToString()}:{guild.GetEmojiIdByVos(vos)}>\t" : "";
                    if (vosRoles.Contains(vos) && !(removedVosRoles != null && removedVosRoles.Contains(vos))) 
                    {
                        activeVos.AppendLine($"{emojiString}{vos.ToString()}");
                    } else
                    {
                        inactiveVos.AppendLine($"{emojiString}{vos.ToString()}");
                    }
                }
                if (activeVos.Length == 0) {activeVos.Append("None");}
                if (inactiveVos.Length == 0) {inactiveVos.Append("None");}

                builder.WithColor(Color.Blue);
                builder.WithAuthor(author => {
                    author.Name = Context.Message.Author.Username;
                    author.IconUrl = Context.Message.Author.GetAvatarUrl();
                });
                builder.AddField("__**Active**__", activeVos.ToString(), true);
                builder.AddField("__**Inactive**__", inactiveVos.ToString(), true);
                builder.WithDescription("Current notification settings");

                return builder.Build();
            }

            private VoiceOfSeren[] SplitVoices(string voices) {
                string[] splitVoices = voices.Split(' ');
                List<VoiceOfSeren> vosList = new List<VoiceOfSeren>();

                for(int i = 0; i < splitVoices.Length; i++) {
                    VoiceOfSeren vos;
                    if (Enum.TryParse(splitVoices[i], true, out vos)) {
                        vosList.Add(vos);
                    }
                }

                return vosList.ToArray();
            }
        }
    }
}