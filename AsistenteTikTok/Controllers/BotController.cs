using AsistenteTikTok.Application.UseCases;
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
    }
}
