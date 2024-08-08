using EventService.Domain.AggregateModels.VoucherAggregate;
using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public class CreateVoucherCommandHanlder : IRequestHandler<CreateVoucherCommand, bool> {
        private readonly EventContext _context;
        private readonly ILogger<CreateVoucherCommandHanlder> _logger;
        private readonly IMediator _mediator;
        public CreateVoucherCommandHanlder(IMediator mediator,
            EventContext context,
            ILogger<CreateVoucherCommandHanlder> logger) {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }
        public async Task<bool> Handle(CreateVoucherCommand request, CancellationToken cancellationToken) {
            var brand = await _context.Brands.FindAsync(request.brandId);
            if (brand == null)
                return false;

            var guid = Guid.NewGuid();
            var voucherCode = guid.ToString().Substring(0, 9);

            brand.AddVoucher(voucherCode, request.Image, request.Value, request.Description, request.ExpireDate, request.Status);
            _logger.LogInformation("Creating Voucher for Brand - Voucher: {@voucher}", brand.Vouchers.Last());

            return await _context.SaveEntitiesAsync(cancellationToken);
        }
    }

    public class CreateVoucherIdentifiedCommandHanlder : IdentifiedCommandHandler<CreateVoucherCommand, bool> {
        public CreateVoucherIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CreateVoucherCommand, bool>> logger)
            : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest() {
            return false;
        }
    }
}
