namespace Labb_2___AdminBookShop.Models;

public partial class CustomerOverview
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public int? PointsBalance { get; set; }

    public int? TotalOrders { get; set; }

    public decimal? TotalSpent { get; set; }
}
