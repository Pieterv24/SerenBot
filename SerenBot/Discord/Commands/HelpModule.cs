using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace SerenBot.Discord.Commands
{
    [Group("")]
    public class HelpModule : ModuleBase
    {
        [Command("Help"), Alias("h")]
        public async Task Help() {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync("", embed: CreateHelpEmbed());
        }

        private Embed CreateHelpEmbed() {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithColor(Color.Blue);
            builder.WithTitle("Seren commands");
            builder.WithDescription("Commands are not case sensitive.\nCommands from examples only need to be run once, these are simply variations of the same commands");

            builder.AddField("Help - H", "sends a list with commands to your dm");

            builder.AddField("Notifications - Notification", @"
            **Add** `full voice of seren name(eg. Amlodd)`: Add specified voice of seren to list with voices you get notified about.
            **Remove - Delete** `full voice of seren name(eg. Amlodd)`: Remove specified voice of seren from list with voices you get notified about.
            **List**: Print what voices you get notified about.
            ");

            builder.AddField("Examples", @"
             ```
Add notification:
>> Notifications Add Iorwerth Crwys
>> Notification Add Amlodd

Remove notification:
>> Notification Remove Amlodd Crwys
>> Notification Remove Iorwerth

List notification settings:
>> Notification List
>> Notifications List
            ```");

            builder.AddField("Admin - A", @"
            **SetupRoles**: Enable mention support by adding roles that can be mentioned. (Requires Role Management Permissions)
            **SetupEmotes**: Add emotes to the server for some extra bling in messages. (Requires Emoji Management Permissions)
            **SetChannel** `#ChannelName`: Define in which channel notifications should be posted. (Requires Webhook Management Permissions)
            **SetPrefix** `prefix`: Define a custom prefix to execute commands. Default is "">>"". (a Prefix cannot be more then 4 characters)
            **Uninstall**: Remove all roles, emojis and data added by the bot from the discord server.
            ");

            builder.AddField("Examples", @"
            ```
Setup Roles/Emotes:
>> Admin SetupRoles
>> a SetupEmotes

Set Channel:
>> A SetChannel #voice-of-seren
>> Admin SetChannel voice-of-seren

Set Prefix:
>> A SetPrefix <>
>> Admin SetPrefix <>

Uninstall:
>> Admin Uninstall
>> a Uninstall
            ```");

            return builder.Build();
        } 
    }
}