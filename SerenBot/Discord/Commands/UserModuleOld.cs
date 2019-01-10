using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using SerenBot.Discord.Ultils;
using SerenBot.Entities;
using SerenBot.Entities.Models;
using SerenBot.Models;

namespace SerenBot.Discord.Commands
{
    [DontAutoLoad]
    [Group("User"), Alias("U", "")]
    public class UserModuleOld : ModuleBase
    {
        private readonly SerenDbContext _context;

        public UserModuleOld(SerenDbContext context) {
                _context = context;
        }

        [Group("Notifications")]
        [RequireContext(ContextType.DM)]
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

                NotificationUser user = await _context.NotificationUsers.FindAsync(Context.Message.Author.Id);
                if (user == null) {
                    byte vosByte = 0;
                    user = new NotificationUser() {
                        UserId = Context.Message.Author.Id,
                        UserName = Context.Message.Author.Username,
                        NotificationValue = vosByte.AddVos(vos)
                    };
                    
                    await _context.NotificationUsers.AddAsync(user);
                    await _context.SaveChangesAsync();
                } else {
                    user.NotificationValue = user.NotificationValue.AddVos(vos);
                    user.UserName = Context.Message.Author.Username;

                    _context.NotificationUsers.Update(user);
                    await _context.SaveChangesAsync();
                }

                await Context.Message.Channel.SendMessageAsync("Your notification settings were updated", embed: CreateNotificationEmbed(user.NotificationValue));
            }

            [Command("Remove"), Alias("Delete")]
            public async Task RemoveUserNotification([Remainder]string voices) {
                VoiceOfSeren[] vos = SplitVoices(voices);

                NotificationUser user = await _context.NotificationUsers.FindAsync(Context.Message.Author.Id);
                byte state = 0;
                if (user != null) {
                    user.NotificationValue = user.NotificationValue.RemoveVos(vos);
                    user.UserName = Context.Message.Author.Username;

                    if (user.NotificationValue > 0) {
                        _context.NotificationUsers.Update(user);
                        state = user.NotificationValue;
                    } else {
                        _context.NotificationUsers.Remove(user);
                    }
                    await _context.SaveChangesAsync();
                }
                await Context.Message.Channel.SendMessageAsync("Your notification settings were updated", embed: CreateNotificationEmbed(state));
            }

            [Command("List"), Alias("Print", "Output")]
            public async Task PrintUserNotifications() {
                NotificationUser user = await _context.NotificationUsers.FindAsync(Context.Message.Author.Id);
                if (user == null) {
                    await Context.Message.Channel.SendMessageAsync("No notification settings were found");
                } else {
                    await Context.Message.Channel.SendMessageAsync("", embed: CreateNotificationEmbed(user.NotificationValue));
                }
            }

            private Embed CreateNotificationEmbed(byte settingByte) {
                EmbedBuilder builder = new EmbedBuilder();

                List<VoiceOfSeren> notificationVoices = new List<VoiceOfSeren>(settingByte.GetVoiceOfSerens());
                StringBuilder activeVos = new StringBuilder();
                StringBuilder inactiveVos = new StringBuilder();

                foreach (VoiceOfSeren voice in (VoiceOfSeren[]) Enum.GetValues(typeof(VoiceOfSeren)))
                {
                    string emojiString = _config.GetValue<string>($"Discord:Emoji:{voice.ToString()}");
                    if (notificationVoices.Contains(voice)) {
                        activeVos.AppendLine($"{emojiString}\t{voice.ToString()}");
                    } else {
                        inactiveVos.AppendLine($"{emojiString}\t{voice.ToString()}");
                    }
                }

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