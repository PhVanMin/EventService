using EventService.Domain.Model;
using EventService.Infrastructure;
using MediatR;
using System.Runtime.Serialization;

namespace EventService.API.Application.Commands.BrandCommands {
    public record CreateBrandCommand(
        string Name, 
        string Field, 
        string? Address,
        string? Gps, 
        int Status
    ) : IRequest<bool>;
    public class CreateBrandCommandHanlder : IRequestHandler<CreateBrandCommand, bool> {
        private readonly EventContext _context;
        private readonly ILogger<CreateBrandCommandHanlder> _logger;
        private readonly IMediator _mediator;
        public CreateBrandCommandHanlder(IMediator mediator,
            EventContext context,
            ILogger<CreateBrandCommandHanlder> logger) {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }
        
        public async Task<bool> Handle(CreateBrandCommand request, CancellationToken cancellationToken) {
            var brand = new Brand() {
                Name = request.Name,
                Address = request.Address,
                Gps = request.Gps,
                Field = request.Field,
                Status = request.Status
            };

            _logger.LogInformation("Creating Brand - Brand: {@Brand}", brand);
            _context.Brands.Add(brand);

            return await _context.SaveEntitiesAsync(cancellationToken);
        }
    }
}
