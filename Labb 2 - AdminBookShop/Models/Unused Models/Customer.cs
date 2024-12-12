namespace Labb_2___AdminBookShop.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public virtual ICollection<CustomerPoint> CustomerPoints { get; set; } = new List<CustomerPoint>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
