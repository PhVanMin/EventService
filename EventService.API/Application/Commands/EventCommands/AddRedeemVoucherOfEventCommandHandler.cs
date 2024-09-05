using EventService.Infrastructure.Idempotency;
using EventService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EventService.Domain.AggregateModels.VoucherAggregate;
using MassTransit.Initializers;

namespace EventService.API.Application.Commands.EventCommands {
    public class AddRedeemVoucherOfEventCommandHandler : IRequestHandler<AddRedeemVoucherOfEventCommand, bool> {
        private readonly EventDbContext _context;
        private readonly ILogger<AddRedeemVoucherOfEventCommandHandler> _logger;
        public AddRedeemVoucherOfEventCommandHandler(
            EventDbContext context,
            ILogger<AddRedeemVoucherOfEventCommandHandler> logger) {
            _context = context;
            _logger = logger;
        }
        public async Task<bool> Handle(AddRedeemVoucherOfEventCommand request, CancellationToken cancellationToken) {
            var ev = await _context.EventVoucher
                .Include(e => e.Voucher)
                .Include(e => e.Event)
                .FirstOrDefaultAsync(e => e.EventId == request.eventId && e.VoucherId == request.voucherId)
                .Select(ev => new {
                    Event = ev?.Event,
                    Value = ev?.Voucher.Value
                });

            if (ev.Event == null || ev.Value == null)
                return false;

            _logger.LogInformation("Updating Event {id} Redeem Voucher Count to {count}", request.eventId, ev.Event.RedeemVoucherCount + 1);
            ev.Event.IncrementRedeemVoucherCount();
            _context.RedeemVouchers.Add(new RedeemVoucher(request.eventId, ev.Event.BrandId, ev.Value.Value));

            return await _context.SaveEntitiesAsync();
        }
    }

    public class AddRedeemVoucherOfEventIdentifiedCommandHanlder : IdentifiedCommandHandler<AddRedeemVoucherOfEventCommand, bool> {
        public AddRedeemVoucherOfEventIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<AddRedeemVoucherOfEventCommand, bool>> logger)
            : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest() {
            return false;
        }
    }
}
