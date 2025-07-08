using AsistenteTikTok.Application.Interfaces;
using AsistenteTikTok.Application.UseCases;
using AsistenteTikTok.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AsistenteTikTok.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BotController : ControllerBase
    {
        private readonly BotCreationService _botCreator;

        public BotController(BotCreationService botCreator)
        {
            _botCreator = botCreator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBot([FromQuery] string emulatorId = "127.0.0.1:5555")
        {
            var result = await _botCreator.CreateBotAsync(emulatorId);

            if (result == null)
                return BadRequest("Falló la creación del bot.");

            return Ok(new
            {
                result.Username,
                result.Email,
                result.Status,
                result.CreatedAt
            });
        }


        [HttpGet("list")]
        public async Task<IActionResult> ListBots([FromServices] IBotAccountRepository repo)
        {
            var bots = await repo.GetAllAsync();
            return Ok(bots);
        }



        [HttpPost("batch")]
        public async Task<IActionResult> CreateMultipleBots([FromBody] BotBatchRequest request)
        {
            var bots = new List<object>();

            for (int i = 0; i < request.Count; i++)
            {
                Console.WriteLine($"[*] Creando bot {i + 1} de {request.Count}");
                var bot = await _botCreator.CreateBotAsync(request.EmulatorId);
                if (bot != null)
                {
                    bots.Add(new
                    {
                        bot.Username,
                        bot.Email,
                        bot.Status,
                        bot.CreatedAt
                    });
                }
            }

            return Ok(bots);
        }

    }
}
