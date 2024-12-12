namespace Labb_2___AdminBookShop.Models;

public partial class Store
{
    public int Id { get; set; }

    public string StoreName { get; set; } = null!;

    public string StreetAddress { get; set; } = null!;

    public string City { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string Country { get; set; } = null!;

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
