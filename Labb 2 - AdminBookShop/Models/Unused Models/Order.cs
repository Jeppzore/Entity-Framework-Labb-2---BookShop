namespace Labb_2___AdminBookShop.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateOnly OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public int? StoreOrderId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Store? StoreOrder { get; set; }
}
