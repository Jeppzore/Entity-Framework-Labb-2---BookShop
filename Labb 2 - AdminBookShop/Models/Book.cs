namespace Labb_2___AdminBookShop.Models;

public partial class Book
{
    public string Isbn13 { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Language { get; set; } = null!;

    public int Pages { get; set; }

    public decimal Price { get; set; }

    public DateOnly PublicationDate { get; set; }

    public string Genre { get; set; } = null!;

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
