using AsistenteTikTok.Application.Interfaces;
using AsistenteTikTok.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AsistenteTikTok.Infrastructure.Files
{
    public class FileBotAccountRepository : IBotAccountRepository
    {
        private const string FolderPath = "data/bots";

        public FileBotAccountRepository()
        {
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);
        }

        public async Task SaveAsync(BotAccount bot)
        {
            var fileName = $"{bot.Username}_{bot.Id}.json";
            var fullPath = Path.Combine(FolderPath, fileName);

            var json = JsonSerializer.Serialize(bot, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(fullPath, json);

            Console.WriteLine($"[✓] Cuenta guardada en: {fullPath}");
        }

        public async Task<IEnumerable<BotBatchRequest>> GetAllAsync()
        {
            var files = Directory.GetFiles(FolderPath, "*.json");
            var bots = new List<BotBatchRequest>();

            foreach (var file in files)
            {
                var json = await File.ReadAllTextAsync(file);
                var bot = JsonSerializer.Deserialize<BotBatchRequest>(json);
                if (bot != null)
                    bots.Add(bot);
            }

            return bots;
        }


    }
}