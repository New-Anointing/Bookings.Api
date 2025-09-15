using Bookings.Common.Domain;

namespace Bookings.Modules.Ticketing.Domain.Customers;

public sealed class Customer : Entity
{
    private Customer()
    {
    }

    public Guid Id { get; private set; }

    public string Email { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public static Customer Create(Guid id, string email, string firstName, string lastName)
    {
        var customer = new Customer()
        {
            Id = id,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

        return customer;
    }

    public void Update(string firstName, string lastName)
    {
        if (FirstName == firstName && LastName == lastName)
        {
            return;
        }

        FirstName = firstName;
        LastName = lastName;
    }
}
