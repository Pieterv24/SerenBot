using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SerenBot.Services.Interfaces
{
    public interface IAuth
    {
        Task<string> GetAccessToken();
    }
}
