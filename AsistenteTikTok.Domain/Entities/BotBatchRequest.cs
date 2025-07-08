using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteTikTok.Domain.Entities
{
    public class BotBatchRequest
    {
        public int Count { get; set; }
        public string EmulatorId { get; set; } = "127.0.0.1:5555";
    }
}
