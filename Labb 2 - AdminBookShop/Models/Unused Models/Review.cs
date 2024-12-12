namespace Labb_2___AdminBookShop.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public string? Isbn13 { get; set; }

    public int? CustomerId { get; set; }

    public int? Rating { get; set; }

    public DateOnly? ReviewDate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Book? Isbn13Navigation { get; set; }
}
