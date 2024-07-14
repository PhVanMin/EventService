using EventService.Infrastructure.Idempotency;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventService.Infrastructure.EntityConfigurations {
    class ClientRequestEntityTypeConfiguration
    : IEntityTypeConfiguration<ClientRequest> {
        public void Configure(EntityTypeBuilder<ClientRequest> requestConfiguration) {
            requestConfiguration.ToTable("requests");
        }
    }
}
