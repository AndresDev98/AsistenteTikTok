using AsistenteTikTok.Application.Interfaces;
using AsistenteTikTok.Domain.Entities;
using AsistenteTikTok.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteTikTok.Application.UseCases
{
    public class BotCreationService
    {
        private readonly IEmailService _emailService;
        private readonly IAdbService _adbService;
        private readonly IBotAccountRepository _botRepo;

        public BotCreationService(IEmailService emailService, IAdbService adbService, IBotAccountRepository botRepo)
        {
            _emailService = emailService;
            _adbService = adbService;
            _botRepo = botRepo;
        }

        public async Task<BotAccount?> CreateBotAsync(string emulatorId)
        {
            try
            {
                var (email, auth) = await _emailService.CreateTempEmailAsync();
                Console.WriteLine($"[+] Email generado: {email}");

                await _adbService.LaunchTikTokAsync(emulatorId);

                await _adbService.TapAsync(emulatorId, 540, 1600); // Registrarse
                await _adbService.TapAsync(emulatorId, 540, 1800); // Email
                await _adbService.TapAsync(emulatorId, 540, 1500); // Fecha
                await _adbService.PressEnterAsync(emulatorId);

                await _adbService.InputTextAsync(emulatorId, email);
                await _adbService.PressEnterAsync(emulatorId);

                Console.WriteLine("[*] Esperando código...");
                var code = await _emailService.GetVerificationCodeAsync(auth);
                if (string.IsNullOrWhiteSpace(code))
                {
                    Console.WriteLine("[-] Código no recibido. Bot cancelado.");
                    return null;
                }

                await _adbService.InputTextAsync(emulatorId, code);
                await _adbService.PressEnterAsync(emulatorId);

                var password = "BotPassword123";
                var username = $"bot{code}";

                await _adbService.InputTextAsync(emulatorId, password);
                await _adbService.PressEnterAsync(emulatorId);

                await _adbService.InputTextAsync(emulatorId, username);
                await _adbService.PressEnterAsync(emulatorId);

                var bot = new BotAccount
                {
                    Email = email,
                    Username = username,
                    EmulatorId = emulatorId,
                    Password = password,
                    Status = BotStatus.Active
                };

                await _botRepo.SaveAsync(bot);
                Console.WriteLine("[✓] Bot creado y guardado correctamente.");

                return bot;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error creando bot: {ex.Message}");
                return null;
            }
        }
    }
}
