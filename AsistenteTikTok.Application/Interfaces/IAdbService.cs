using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteTikTok.Application.Interfaces
{
    public interface IAdbService
    {
        Task LaunchTikTokAsync(string emulatorId);
        Task TapAsync(string emulatorId, int x, int y);
        Task InputTextAsync(string emulatorId, string text);
        Task PressEnterAsync(string emulatorId);
    }

}
