using Bookings.Modules.Events.Domain.Abstractions;

namespace Bookings.Modules.Events.Domain.Categories;
public sealed partial class Category : Entity
{
    private Category() 
    {
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool IsArchived { get; private set; }

    public static Category Create(string Name) 
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = Name,
            IsArchived = false
        };
        category.Raise(new CategoryCreatedDomainEvent(category.Id));
        return category;
    }

    public void Archive ()
    {
        IsArchived = true;
        Raise(new CategoryArchivedDomainEvent(Id));
    }

    public void ChangeName(string name)
    {
        if (Name == name)
        {
            return;
        }

        Name = name;

        Raise(new CategoryNameChangedDomainEvent(Id, Name));
    }
}

