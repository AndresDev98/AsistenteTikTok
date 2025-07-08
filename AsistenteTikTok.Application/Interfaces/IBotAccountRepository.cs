using AsistenteTikTok.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteTikTok.Application.Interfaces
{
    public interface IBotAccountRepository
    {
        Task SaveAsync(BotAccount bot);
        Task<IEnumerable<BotBatchRequest>> GetAllAsync();

    }
}
