using EventService.Domain.AggregateModels.GameAggregate;
using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;

namespace EventService.API.Application.Commands.GameCommands {
    public class CreateGameCommandHanlder : IRequestHandler<CreateGameCommand, bool> {
        private readonly EventContext _context;
        private readonly ILogger<CreateGameCommandHanlder> _logger;
        private readonly IMediator _mediator;
        public CreateGameCommandHanlder(IMediator mediator,
            EventContext context,
            ILogger<CreateGameCommandHanlder> logger) {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<bool> Handle(CreateGameCommand request, CancellationToken cancellationToken) {
            var game = new Game() {
                Name = request.Name,
                Image = request.Image
            };

            _logger.LogInformation("Creating Game - Game: {@Game}", game);
            _context.Games.Add(game);

            return await _context.SaveEntitiesAsync(cancellationToken);
        }
    }

    public class CreateGameIdentifiedCommandHanlder : IdentifiedCommandHandler<CreateGameCommand, bool> {
        public CreateGameIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateGameCommand, bool>> logger)
        : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest() {
            return false;
        }
    }
}
