namespace Bookings.Modules.Users.Domain.Users;

public sealed class Role
{
    public string Name { get; private set; }
    private Role(string mane)
    {
        Name = mane;
    }

    private Role() { }

    public static readonly Role Administrator = new Role("Administrator");
    public static readonly Role Member = new Role("Member");
}
