using Bookings.Modules.Events.Domain.Categories;
using Bookings.Modules.Events.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookings.Modules.Events.Infrastructure.Events;
internal sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasOne<Category>().WithMany();
    }
}
