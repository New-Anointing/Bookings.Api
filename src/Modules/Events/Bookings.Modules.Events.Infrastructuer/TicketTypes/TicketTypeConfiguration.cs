using Bookings.Modules.Events.Domain.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookings.Modules.Events.Infrastructure.TicketTypes;
internal sealed class TicketTypeConfiguration : IEntityTypeConfiguration<TicketType>
{
    public void Configure(EntityTypeBuilder<TicketType> builder)
    {
        builder.HasOne<TicketType>().WithMany().HasForeignKey(t => t.EventId);
    }
}
