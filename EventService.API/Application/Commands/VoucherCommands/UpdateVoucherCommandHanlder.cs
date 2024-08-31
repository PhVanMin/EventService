using EventService.Infrastructure.Idempotency;
using EventService.Infrastructure;
using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public class UpdateVoucherCommandHanlder : IRequestHandler<UpdateVoucherCommand, bool> {
        private readonly EventDbContext _context;
        private readonly ILogger<UpdateVoucherCommandHanlder> _logger;
        private readonly IMediator _mediator;
        public UpdateVoucherCommandHanlder(IMediator mediator,
            EventDbContext context,
            ILogger<UpdateVoucherCommandHanlder> logger) {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }
        public async Task<bool> Handle(UpdateVoucherCommand request, CancellationToken cancellationToken) {
            var voucher = await _context.Vouchers.FindAsync(request.id);
            if (voucher == null) {
                return false;
            }

            voucher.Update(request.image, request.value, request.description, request.expireDate, request.status);
            _logger.LogInformation("Update Voucher for Brand - Voucher: {@voucher}", voucher);

            return await _context.SaveEntitiesAsync(cancellationToken);
        }
    }

    public class UpdateVoucherIdentifiedCommandHanlder : IdentifiedCommandHandler<UpdateVoucherCommand, bool> {
        public UpdateVoucherIdentifiedCommandHanlder(IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<UpdateVoucherCommand, bool>> logger)
            : base(mediator, requestManager, logger) { }

        protected override bool CreateResultForDuplicateRequest() {
            return true;
        }
    }
}
