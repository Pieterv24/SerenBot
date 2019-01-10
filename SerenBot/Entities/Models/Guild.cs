using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SerenBot.Entities.Models
{
    public class Guild
    {
        [Key]
        public ulong GuildId { get; set; }
        public string GuildName { get; set; }
        public ulong NotificationWebhookId {get; set;}
        public string NotificationWebhookToken {get; set;}
        [MaxLength(4)]
        public string Prefix { get; set; }
        public bool EmotesEnabled {get; set;}
        public bool MentionsEnabled {get; set;}
#region Roles
        public ulong AmloddRoleId {get; set;}
        public ulong CadarnRoleId {get; set;}
        public ulong CrwysRoleId {get; set;}
        public ulong HefinRoleId {get; set;}
        public ulong IorwerthRoleId {get; set;}
        public ulong IthellRoleId {get; set;}
        public ulong MeilyrRoleId {get; set;}
        public ulong TrahaearnRoleId {get; set;}
#endregion
#region Emoji
        public ulong AmloddEmojiId {get; set;}
        public ulong CadarnEmojiId {get; set;}
        public ulong CrwysEmojiId {get; set;}
        public ulong HefinEmojiId {get; set;}
        public ulong IorwerthEmojiId {get; set;}
        public ulong IthellEmojiId {get; set;}
        public ulong MeilyrEmojiId {get; set;}
        public ulong TrahaearnEmojiId {get; set;}
#endregion
    }
}
