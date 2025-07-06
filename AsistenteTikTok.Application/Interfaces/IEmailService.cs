using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteTikTok.Application.Interfaces
{
    public interface IEmailService
    {
        Task<(string Email, object AuthContext)> CreateTempEmailAsync();
        Task<string?> GetVerificationCodeAsync(object authContext);
    }
}
