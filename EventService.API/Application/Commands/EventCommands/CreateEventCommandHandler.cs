using EventService.API.Controllers;
using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Application.Commands.EventCommands
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, bool>
    {
        private readonly EventDbContext _context;
        private readonly ILogger<CreateEventCommandHandler> _logger;
        private readonly AzureClientService _azureClientService;
        public CreateEventCommandHandler(
            EventDbContext context,
            ILogger<CreateEventCommandHandler> logger,
            AzureClientService azureClientService)
        {
            _context = context;
            _logger = logger;
            _azureClientService = azureClientService;
        }
        public async Task<bool> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            if (request.image == null)
                return false;

            var brand = await _context.Brands
                .Include(b => b.Events)
                .FirstOrDefaultAsync(b => b.Id == request.brandId);

            if (brand == null)
                return false;

            if (brand.Events.FirstOrDefault(e => e.Name == request.name) != null)
                return false;
           
            var ev = brand.AddEvent(request.name, string.Empty, request.noVoucher, request.start, request.end, request.gameId, request.voucherIds);
            _logger.LogInformation("Adding Event to Brand - Event: {EventId}", ev.Id);

            var result = await _context.SaveEntitiesAsync(cancellationToken);
            if (result != false) {
                var filePath = await _azureClientService.UploadFileAsync(request.image, cancellationToken);
                ev.Image = filePath;
                await _context.SaveEntitiesAsync(cancellationToken);
            }

            return result;
        }
    }

    public class CreateEventIdentifiedCommandHanlder : IdentifiedCommandHandler<CreateEventCommand, bool>
    {
        public CreateEventIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateEventCommand, bool>> logger)
            : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return false;
        }
    }
}
