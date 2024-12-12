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

        modelBuilder.Ignore<CustomerPoint>();

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

//public virtual DbSet<Customer> Customers { get; set; }

//public virtual DbSet<CustomerOverview> CustomerOverviews { get; set; }

//public virtual DbSet<CustomerPoint> CustomerPoints { get; set; }

//public virtual DbSet<Order> Orders { get; set; }

//public virtual DbSet<Review> Reviews { get; set; }

//public virtual DbSet<TitlesByAuthor> TitlesByAuthors { get; set; }


/*
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    var connectionString = new SqlConnectionStringBuilder()
    {
        ServerSPN = "localhost",
        InitialCatalog = "bookshop", // Name of database
        TrustServerCertificate = true,
        IntegratedSecurity = true,
        //UserID = "login",
        //Password = "password"
    }.ToString();
}

    optionsBuilder.useSqlServer(connectionString);
*/


//modelBuilder.Entity<Customer>(entity =>
//{
//    entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8C96A16F0");

//    entity.HasIndex(e => e.Email, "UQ__Customer__A9D10534C08C4341").IsUnique();

//    entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
//    entity.Property(e => e.Email).HasMaxLength(200);
//    entity.Property(e => e.FirstName).HasMaxLength(100);
//    entity.Property(e => e.LastName).HasMaxLength(100);
//    entity.Property(e => e.PhoneNumber).HasMaxLength(20);
//});

//modelBuilder.Entity<CustomerOverview>(entity =>
//{
//    entity
//        .HasNoKey()
//        .ToView("CustomerOverview");

//    entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
//    entity.Property(e => e.Email).HasMaxLength(200);
//    entity.Property(e => e.Name).HasMaxLength(201);
//    entity.Property(e => e.PointsBalance).HasColumnName("Points Balance");
//    entity.Property(e => e.TotalOrders).HasColumnName("Total Orders");
//    entity.Property(e => e.TotalSpent)
//        .HasColumnType("decimal(38, 2)")
//        .HasColumnName("Total Spent");
//});

//modelBuilder.Entity<CustomerPoint>(entity =>
//{
//    entity.HasKey(e => e.CustomerPointsId).HasName("PK__Customer__8986639FC2E29609");

//    entity.Property(e => e.CustomerPointsId).HasColumnName("CustomerPointsID");
//    entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
//    entity.Property(e => e.LastUpdated)
//        .HasDefaultValueSql("(getdate())")
//        .HasColumnType("datetime");
//    entity.Property(e => e.PointsStatus)
//        .HasMaxLength(50)
//        .HasDefaultValue("Active");

//    entity.HasOne(d => d.Customer).WithMany(p => p.CustomerPoints)
//        .HasForeignKey(d => d.CustomerId)
//        .HasConstraintName("FK__CustomerP__Custo__4BAC3F29");
//});


//modelBuilder.Entity<Order>(entity =>
//{
//    entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF17D191FC");

//    entity.Property(e => e.OrderId).HasColumnName("OrderID");
//    entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
//    entity.Property(e => e.StoreOrderId).HasColumnName("StoreOrderID");
//    entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

//    entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
//        .HasForeignKey(d => d.CustomerId)
//        .OnDelete(DeleteBehavior.ClientSetNull)
//        .HasConstraintName("FK__Orders__Customer__4E88ABD4");

//    entity.HasOne(d => d.StoreOrder).WithMany(p => p.Orders)
//        .HasForeignKey(d => d.StoreOrderId)
//        .HasConstraintName("FK_Order_Store");
//});

//modelBuilder.Entity<Review>(entity =>
//{
//    entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AEDE9CF571");

//    entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
//    entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
//    entity.Property(e => e.Isbn13)
//        .HasMaxLength(13)
//        .IsUnicode(false)
//        .IsFixedLength()
//        .HasColumnName("ISBN13");

//    entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
//        .HasForeignKey(d => d.CustomerId)
//        .HasConstraintName("FK__Reviews__Custome__5535A963");

//    entity.HasOne(d => d.Isbn13Navigation).WithMany(p => p.Reviews)
//        .HasForeignKey(d => d.Isbn13)
//        .HasConstraintName("FK__Reviews__ISBN13__5441852A");
//});


//modelBuilder.Entity<TitlesByAuthor>(entity =>
//{
//    entity
//        .HasNoKey()
//        .ToView("TitlesByAuthor");

//    entity.Property(e => e.Name).HasMaxLength(101);
//    entity.Property(e => e.TotalValue).HasColumnType("decimal(38, 2)");
//});
