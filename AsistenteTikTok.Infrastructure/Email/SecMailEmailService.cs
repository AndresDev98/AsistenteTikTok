using AsistenteTikTok.Application.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistenteTikTok.Infrastructure.Email
{
    public class SecMailEmailService : IEmailService
    {
        private const string BaseUrl = "https://www.1secmail.com/api/v1/";
        private readonly HttpClient _httpClient = new();

        public async Task<(string Email, object AuthContext)> CreateTempEmailAsync()
        {
            var username = $"bot{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{new Random().Next(100, 999)}";
            var domain = "1secmail.com";
            var email = $"{username}@{domain}";

            Console.WriteLine($"[*] Email generado: {email}");

            var authContext = new { username, domain };
            return (email, authContext);
        }

        public async Task<string?> GetVerificationCodeAsync(object authContext)
        {
            var context = (dynamic)authContext;
            string login = context.username;
            string domain = context.domain;

            for (int attempt = 0; attempt < 12; attempt++) // 12 * 5s = 60s
            {
                try
                {
                    var msgListUrl = $"{BaseUrl}?action=getMessages&login={login}&domain={domain}";
                    var response = await _httpClient.GetStringAsync(msgListUrl);
                    var messages = JArray.Parse(response);

                    if (messages.Count > 0)
                    {
                        var msgId = messages[0]["id"]?.ToString();
                        var readMsgUrl = $"{BaseUrl}?action=readMessage&login={login}&domain={domain}&id={msgId}";
                        var msgContent = await _httpClient.GetStringAsync(readMsgUrl);
                        var body = JObject.Parse(msgContent)["body"]?.ToString();

                        if (!string.IsNullOrEmpty(body))
                        {
                            var match = System.Text.RegularExpressions.Regex.Match(body, @"\b(\d{4,6})\b");
                            if (match.Success)
                            {
                                return match.Value;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Error al consultar correo: {ex.Message}");
                }

                await Task.Delay(5000);
            }

            return null;
        }
    }
}