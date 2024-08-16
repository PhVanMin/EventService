using EventService.Infrastructure.Idempotency;
using EventService.Infrastructure;
using MediatR;
using EventService.API.Application.Queries;

namespace EventService.API.Application.Commands.VoucherCommands {
    public class CreateRedeemVoucherCommandHanlder : IRequestHandler<CreateRedeemVoucherCommand, RedeemVoucherVM?> {
        private readonly EventContext _context;
        private readonly ILogger<CreateRedeemVoucherCommand> _logger;
        private readonly IMediator _mediator;
        public CreateRedeemVoucherCommandHanlder(IMediator mediator,
            EventContext context,
            ILogger<CreateRedeemVoucherCommand> logger) {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }
        public async Task<RedeemVoucherVM?> Handle(CreateRedeemVoucherCommand request, CancellationToken cancellationToken) {
            var @event = await _context.Events.FindAsync(request.eventId);
            if (@event == null)
                return null;

            var voucher = @event.AddRedeemVoucher(request.playerId, request.voucherId);
            _logger.LogInformation("Creating Redeem Voucher for Event - Voucher: {@voucher}", voucher);

            await _context.SaveEntitiesAsync(cancellationToken);
            return new RedeemVoucherVM {
                Id = voucher.Id,
                Description = voucher.BaseVoucher.Description,
                ExpireDate = voucher.ExpireDate,
                Image = voucher.BaseVoucher.Image,
                RedeemCode = voucher.RedeemCode,
                RedeemTime = voucher.RedeemTime,
                Value = voucher.BaseVoucher.Value
            };
        }
    }

    public class CreateRedeemVoucherIdentifiedCommandHanlder : IdentifiedCommandHandler<CreateRedeemVoucherCommand, RedeemVoucherVM?> {
        public CreateRedeemVoucherIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateRedeemVoucherCommand, RedeemVoucherVM?>> logger)
            : base(mediator, requestManager, logger) { }

        protected override RedeemVoucherVM? CreateResultForDuplicateRequest() {
            return null;
        }
    }
}
