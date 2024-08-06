using EventService.Domain.AggregateModels;
using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public class AddVoucherToEventCommandHanlder : IRequestHandler<AddVoucherToEventCommand, bool> {
        private readonly EventContext _context;
        private readonly ILogger<AddVoucherToEventCommandHanlder> _logger;
        private readonly IMediator _mediator;
        public AddVoucherToEventCommandHanlder(IMediator mediator,
            EventContext context,
            ILogger<AddVoucherToEventCommandHanlder> logger) {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }
        public async Task<bool> Handle(AddVoucherToEventCommand request, CancellationToken cancellationToken) {
            var @event = await _context.Events.FindAsync(request.eventId);
            if (@event == null)
                return false;

            var voucher = await _context.Vouchers.FindAsync(request.voucherId);
            if (voucher == null)
                return false;

            _logger.LogInformation("Add Voucher to Event - Voucher: {@Voucher} - Event: {@Event}", voucher, @event);
            _context.EventVoucher.Add(new EventVoucher {
                EventId = request.eventId,
                VoucherId = request.voucherId
            });

            return await _context.SaveEntitiesAsync(cancellationToken);
        }
    }

    public class AddVoucherToEventIdentifiedCommandHanlder : IdentifiedCommandHandler<AddVoucherToEventCommand, bool> {
        public AddVoucherToEventIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<AddVoucherToEventCommand, bool>> logger)
            : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest() {
            return false;
        }
    }
}
