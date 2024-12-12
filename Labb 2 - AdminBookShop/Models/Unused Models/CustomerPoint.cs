namespace Labb_2___AdminBookShop.Models;

public partial class CustomerPoint
{
    public int CustomerPointsId { get; set; }

    public int? CustomerId { get; set; }

    public int? PointsBalance { get; set; }

    public DateTime? LastUpdated { get; set; }

    public string? PointsStatus { get; set; }

    public virtual Customer? Customer { get; set; }
}
