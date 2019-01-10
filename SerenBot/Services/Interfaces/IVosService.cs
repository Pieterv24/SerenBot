using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SerenBot.Models;
using SerenBot.Models.Twitter;

namespace SerenBot.Services.Interfaces
{
    public interface IVosService
    {
        Task<List<Status>> GetVosStatuses();

        Task<VoiceOfSeren[]> GetActiveVos();
    }
}
