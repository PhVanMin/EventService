using EventService.Domain.AggregateModels;
using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public class UpdateEventVouchersCommandHanlder : IRequestHandler<UpdateEventVouchersCommand, bool> {
        private readonly EventContext _context;
        private readonly ILogger<UpdateEventVouchersCommandHanlder> _logger;
        private readonly IMediator _mediator;
        public UpdateEventVouchersCommandHanlder(IMediator mediator,
            EventContext context,
            ILogger<UpdateEventVouchersCommandHanlder> logger) {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }
        public async Task<bool> Handle(UpdateEventVouchersCommand request, CancellationToken cancellationToken) {
            var @event = await _context.Events.FindAsync(request.eventId);
            if (@event == null)
                return false;

            foreach (var voucherId in request.voucherIds) {
                var voucher = await _context.Vouchers.FindAsync(voucherId);
                if (voucher == null) return false;

                _logger.LogInformation("Add Voucher to Event - Voucher: {@Voucher} - Event: {@Event}", voucher, @event);
                _context.EventVoucher.Add(new EventVoucher {
                    EventId = request.eventId,
                    VoucherId = voucherId
                });
            }

            return await _context.SaveEntitiesAsync(cancellationToken);
        }
    }

    public class UpdateEventVouchersIdentifiedCommandHanlder : IdentifiedCommandHandler<UpdateEventVouchersCommand, bool> {
        public UpdateEventVouchersIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<UpdateEventVouchersCommand, bool>> logger)
            : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest() {
            return false;
        }
    }
}
