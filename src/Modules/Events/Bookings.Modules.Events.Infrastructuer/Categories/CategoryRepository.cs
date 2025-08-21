
using Bookings.Modules.Events.Domain.Categories;
using Bookings.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Modules.Events.Infrastructure.Categories;
public class CategoryRepository(EventsDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void Insert(Category category)
    {
        context.Categories.Add(category);
    }
}
