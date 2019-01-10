using System.Threading.Tasks;
using SerenBot.Models;

namespace SerenBot.Services.Interfaces
{
    public interface IDiscordGuildNotifier
    {
        Task SendVosNotifications(VoiceOfSeren[] voices);
    }
}