using EventService.API.Controllers;
using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public class UpdateVoucherCommandHanlder : IRequestHandler<UpdateVoucherCommand, bool> {
        private readonly EventDbContext _context;
        private readonly ILogger<UpdateVoucherCommandHanlder> _logger;
        private readonly AzureClientService _azureClientService;
        public UpdateVoucherCommandHanlder(
            EventDbContext context,
            ILogger<UpdateVoucherCommandHanlder> logger,
            AzureClientService azureClientService) {
            _context = context;
            _logger = logger;
            _azureClientService = azureClientService;
        }
        public async Task<bool> Handle(UpdateVoucherCommand request, CancellationToken cancellationToken) {
            var voucher = await _context.Vouchers.FindAsync(request.id);
            if (voucher == null) {
                return false;
            }

            voucher.Update(voucher.Image, request.code, request.value, request.description, request.expireDate, request.status);
            _logger.LogInformation("Update Voucher for Brand - Voucher: {@voucher}", voucher);

            var result = await _context.SaveEntitiesAsync(cancellationToken);

            if (request.image != null && request.image.FileName != voucher.Image && result != false) {
                var filePath = await _azureClientService.UploadFileAsync(request.image, cancellationToken);
                voucher.Image = filePath;
                await _context.SaveEntitiesAsync(cancellationToken);
            }

            return result;
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
