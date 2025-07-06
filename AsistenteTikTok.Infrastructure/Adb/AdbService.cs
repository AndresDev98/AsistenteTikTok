using AsistenteTikTok.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteTikTok.Infrastructure.Adb
{
    public class AdbService : IAdbService
    {
        private string RunAdbCommand(string emulatorId, string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "adb",
                Arguments = $"-s {emulatorId} {arguments}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = psi };
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(error))
            {
                Console.WriteLine($"[ADB ERROR] {error}");
            }

            return output;
        }

        public Task LaunchTikTokAsync(string emulatorId)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("[ADB] Lanzando TikTok...");
                RunAdbCommand(emulatorId, "shell monkey -p com.zhiliaoapp.musically -c android.intent.category.LAUNCHER 1");
                Task.Delay(4000).Wait();
            });
        }

        public Task TapAsync(string emulatorId, int x, int y)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"[ADB] Tap en ({x},{y})");
                RunAdbCommand(emulatorId, $"shell input tap {x} {y}");
                Task.Delay(1000).Wait();
            });
        }

        public Task InputTextAsync(string emulatorId, string text)
        {
            return Task.Run(() =>
            {
                var safeText = text.Replace(" ", "%s");
                Console.WriteLine($"[ADB] Input: {safeText}");
                RunAdbCommand(emulatorId, $"shell input text \"{safeText}\"");
                Task.Delay(500).Wait();
            });
        }

        public Task PressEnterAsync(string emulatorId)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("[ADB] Enter");
                RunAdbCommand(emulatorId, "shell input keyevent 66");
                Task.Delay(500).Wait();
            });
        }
    }
}
