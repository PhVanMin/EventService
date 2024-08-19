using EventService.API.Application.Commands;
using EventService.API.Application.Commands.GameCommands;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase {
        private readonly EventContext _context;
        private readonly EventAPIService _services;
        public GameController(EventContext context, EventAPIService services) {
            _context = context;
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetGames() {
            try {
                var games = await _services.Queries.GetGame();
                return Ok(games);
            } catch {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame(CreateGameCommand command) {
            var createGameOrder = new IdentifiedCommand<CreateGameCommand, bool>(command, Guid.NewGuid());

            _services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                createGameOrder.GetType().Name,
                nameof(createGameOrder.Id),
                createGameOrder.Id,
                createGameOrder
            );

            var result = await _services.Mediator.Send(createGameOrder);

            if (result) {
                _services.Logger.LogInformation("CreateGameCommand succeeded");
                return Ok();
            } else {
                _services.Logger.LogWarning("CreateGameCommand failed");
                return BadRequest();
            }
        }
    }
}
