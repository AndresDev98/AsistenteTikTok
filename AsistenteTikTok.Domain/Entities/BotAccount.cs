using AsistenteTikTok.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteTikTok.Domain.Entities
{
    public class BotAccount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = "BotPassword123";
        public string EmulatorId { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public BotStatus Status { get; set; } = BotStatus.Pending;
    }

}
