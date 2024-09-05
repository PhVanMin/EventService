using EventService.API.Controllers;
using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Application.Commands.EventCommands {
    public class UpdateEventCommandHanlder : IRequestHandler<UpdateEventCommand, bool> {
        private readonly EventDbContext _context;
        private readonly ILogger<UpdateEventCommandHanlder> _logger;
        private readonly IMediator _mediator;
        private readonly AzureClientService _azureClientService;
        public UpdateEventCommandHanlder(IMediator mediator,
            EventDbContext context,
            ILogger<UpdateEventCommandHanlder> logger,
            AzureClientService azureClientService) {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _azureClientService = azureClientService;
        }

        public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken) {
            var @event = await _context.Events.Include(e => e.Vouchers).FirstOrDefaultAsync(
                e => e.Id == request.id
                && e.BrandId == request.brandId);

            if (@event == null) {
                return false;
            }

            _logger.LogInformation("Updating Brand Event - Brand Event: {@event}", @event);

            @event.Update(
                request.name, @event.Image,
                request.noVoucher, request.start,
                request.end, request.gameId);

            if (request.voucherIds != null) {
                foreach (var id in request.voucherIds) {
                    @event.AddVoucher(id);
                }
            }

            var result = await _context.SaveEntitiesAsync(cancellationToken);

            if (request.image != null && request.image.FileName != @event.Image && result != false) {
                var filePath = await _azureClientService.UploadFileAsync(request.image, cancellationToken);
                @event.Image = filePath;
                await _context.SaveEntitiesAsync(cancellationToken);
            }

            return result;
        }
    }

    public class UpdateEventIdentifiedCommandHanlder : IdentifiedCommandHandler<UpdateEventCommand, bool> {
        public UpdateEventIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<UpdateEventCommand, bool>> logger)
        : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest() {
            return true;
        }
    }
}
