using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Labb_2___AdminBookShop.Models;

public partial class BookshopContext : DbContext
{
    public BookshopContext()
    {
    }

    public BookshopContext(DbContextOptions<BookshopContext> options)
        : base(options)
    {

    }

    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<Inventory> Inventories { get; set; }
    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<BookshopContext>().Build();
        var connectionString = config["ConnectionString"];
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Author
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Author__70DAFC1487AD3AC8");

            entity.ToTable("Author");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        // Book
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn13).HasName("PK__Books__3BF79E031DA11D8A");

            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN13");
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .HasDefaultValue("-");
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasMany(d => d.Authors).WithMany(p => p.Isbn13s)
                .UsingEntity<Dictionary<string, object>>(
                    "BookAuthor",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_BookAuthors_Author"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("Isbn13")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_BookAuthors_Book"),
                    j =>
                    {
                        j.HasKey("Isbn13", "AuthorId").HasName("PK__BookAuth__6CFA31C2D7F66A1E");
                        j.ToTable("BookAuthors");
                        j.IndexerProperty<string>("Isbn13")
                            .HasMaxLength(13)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("ISBN13");
                        j.IndexerProperty<int>("AuthorId").HasColumnName("AuthorID");
                    });
        });

        // Inventory
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.Isbn }).HasName("PK__Inventor__183D890129AB1313");

            entity.ToTable("Inventory");

            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Book");

            entity.HasOne(d => d.Store).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Store");
        });
        // Store

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stores__3B82F0E1D4349CC4");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.StoreName).HasMaxLength(100);
            entity.Property(e => e.StreetAddress).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
