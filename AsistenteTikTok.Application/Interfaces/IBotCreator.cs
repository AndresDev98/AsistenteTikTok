using AsistenteTikTok.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteTikTok.Application.Interfaces
{
    public interface IBotCreator
    {
        Task<BotAccount> CreateAsync();
    }

}
