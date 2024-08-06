using EventService.Domain.AggregateModels.VoucherAggregate;
using EventService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase {
        private readonly EventContext _context;
        private readonly EventAPIService _services;
        public VouchersController(EventContext context, EventAPIService services) {
            _context = context;
            _services = services;
        }
    }
}
