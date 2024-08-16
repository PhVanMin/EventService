using EventService.Infrastructure.Idempotency;
using EventService.Infrastructure;
using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public class RedeemVoucherCommandHandler : IRequestHandler<RedeemVoucherCommand, bool> {
        private readonly EventContext _context;
        private readonly ILogger<RedeemVoucherCommand> _logger;
        private readonly IMediator _mediator;
        public RedeemVoucherCommandHandler(IMediator mediator,
            EventContext context,
            ILogger<RedeemVoucherCommand> logger) {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }
        public async Task<bool> Handle(RedeemVoucherCommand request, CancellationToken cancellationToken) {
            var @event = await _context.Events.FindAsync(request.eventId);
            if (@event == null) {
                return false;
            }

            @event.RedeemVoucher(request.redeemVoucherId, request.code);
            _logger.LogInformation("Redeem Voucher for User - Voucher: {@voucherId}", request.redeemVoucherId);

            return await _context.SaveEntitiesAsync(cancellationToken);
        }
    }

    public class RedeemVoucherIdentifiedCommandHanlder : IdentifiedCommandHandler<RedeemVoucherCommand, bool> {
        public RedeemVoucherIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<RedeemVoucherCommand, bool>> logger)
            : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest() {
            return true;
        }
    }
}
