using EventService.Domain.AggregateModels.VoucherAggregate;
using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Application.Commands.EventCommands {
    public class AddRedeemVoucherOfEventCommandHandler : IRequestHandler<AddRedeemVoucherOfEventCommand, RedeemVoucherDataVM?> {
        private readonly EventDbContext _context;
        private readonly ILogger<AddRedeemVoucherOfEventCommandHandler> _logger;
        public AddRedeemVoucherOfEventCommandHandler(
            EventDbContext context,
            ILogger<AddRedeemVoucherOfEventCommandHandler> logger) {
            _context = context;
            _logger = logger;
        }
        public async Task<RedeemVoucherDataVM?> Handle(AddRedeemVoucherOfEventCommand request, CancellationToken cancellationToken) {
            var ev = await _context.EventVoucher
                .Include(e => e.Voucher)
                .Include(e => e.Event)
                .Where(e => e.EventId == request.eventId)
                .ToListAsync();

            if (ev == null || ev.Count == 0)
                return null;

            var random = new Random();
            int randomIndex = random.Next(ev.Count);
            var randomVoucher = ev[randomIndex];

            _logger.LogInformation("Updating Event {id} Redeem Voucher Count to {count}", request.eventId, randomVoucher.Event.RedeemVoucherCount + 1);
            randomVoucher.Event.IncrementRedeemVoucherCount();
            _context.RedeemVouchers.Add(new RedeemVoucher(randomVoucher.Event.BrandId, request.eventId, randomVoucher.Voucher.Value));

            await _context.SaveEntitiesAsync();
            return new RedeemVoucherDataVM(
                randomVoucher.Voucher.Code, 
                randomVoucher.Voucher.Id,
                randomVoucher.Voucher.ExpireDate);
        }
    }

    public class AddRedeemVoucherOfEventIdentifiedCommandHanlder : IdentifiedCommandHandler<AddRedeemVoucherOfEventCommand, RedeemVoucherDataVM?> {
        public AddRedeemVoucherOfEventIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<AddRedeemVoucherOfEventCommand, RedeemVoucherDataVM?>> logger)
            : base(mediator, requestManager, logger) { }

        protected override RedeemVoucherDataVM? CreateResultForDuplicateRequest() {
            return null;
        }
    }
}
