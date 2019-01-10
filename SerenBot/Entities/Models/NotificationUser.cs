using System.ComponentModel.DataAnnotations;

namespace SerenBot.Entities.Models
{

    public class NotificationUser
    {
        [Key]
        public ulong UserId { get; set; }
        public string UserName {get; set;}
        public byte NotificationValue { get; set; }
    }
}
