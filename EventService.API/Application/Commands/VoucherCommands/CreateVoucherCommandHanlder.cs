using EventService.API.Controllers;
using EventService.Infrastructure;
using EventService.Infrastructure.Idempotency;
using MediatR;

namespace EventService.API.Application.Commands.VoucherCommands {
    public class CreateVoucherCommandHanlder : IRequestHandler<CreateVoucherCommand, bool> {
        private readonly EventDbContext _context;
        private readonly ILogger<CreateVoucherCommandHanlder> _logger;
        private readonly AzureClientService _azureClientService;
        public CreateVoucherCommandHanlder(
            EventDbContext context,
            ILogger<CreateVoucherCommandHanlder> logger,
            AzureClientService azureClientService) {
            _context = context;
            _logger = logger;
            _azureClientService = azureClientService;
        }
        public async Task<bool> Handle(CreateVoucherCommand request, CancellationToken cancellationToken) {
            var brand = await _context.Brands.FindAsync(request.brandId);
            if (brand == null)
                return false;

            if (request.Image == null)
                return false;

            var voucher = brand.AddVoucher(string.Empty, request.Code, request.Value, request.Description, request.ExpireDate, request.Status);
            _logger.LogInformation("Creating Voucher for Brand - Voucher: {@voucher}", voucher);

            var result = await _context.SaveEntitiesAsync(cancellationToken);
            if (result != false) {
                var filePath = await _azureClientService.UploadFileAsync(request.Image, cancellationToken);
                voucher.Image = filePath;
                await _context.SaveEntitiesAsync(cancellationToken);
            }

            return result;
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
