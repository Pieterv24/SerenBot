using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace SerenBot.Discord.Ultils
{
    public static class DiscordExtensions
    {
        public static async Task<bool> CheckChannelPermissionsAsync(this ICommandContext context, IGuildChannel channel, ChannelPermission[] perms)
        {
            List<ChannelPermission> channelPerms = (await context.Guild.GetCurrentUserAsync()).GetPermissions(channel).ToList();

            foreach (ChannelPermission permission in perms)
            {
                if (!channelPerms.Contains(permission))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
