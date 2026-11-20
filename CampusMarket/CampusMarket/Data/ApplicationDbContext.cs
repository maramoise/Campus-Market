using CampusMarket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusMarket.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Listing> Listings { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<SellerRequest> SellerRequests { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<ReservationItem> ReservationItems { get; set; }
    public DbSet<FaqItem> FaqItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // primul, configurează tabelele Identity

        // numele categoriei este unic
        modelBuilder.Entity<Category>()
            .HasIndex(c => c.CategoryName)
            .IsUnique();

        // prețul cu 2 zecimale
        modelBuilder.Entity<Listing>()
            .Property(l => l.Price).HasPrecision(18, 2);
        modelBuilder.Entity<ReservationItem>()
            .Property(i => i.UnitPrice).HasPrecision(18, 2);

        // vânzătorul: Restrict (evită "multiple cascade paths" în SQL Server)
        modelBuilder.Entity<Listing>()
            .HasOne(l => l.Seller)
            .WithMany()
            .HasForeignKey(l => l.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        // un user are cel mult un rating per anunț
        modelBuilder.Entity<Rating>()
            .HasIndex(r => new { r.ListingId, r.UserId })
            .IsUnique();

        // coș: un produs apare o singură dată per user
        modelBuilder.Entity<CartItem>()
            .HasIndex(c => new { c.UserId, c.ListingId })
            .IsUnique();

        // favorite: cheie primară compusă => fără duplicate
        modelBuilder.Entity<Favorite>()
            .HasKey(f => new { f.UserId, f.ListingId });

        // maxim o cerere "În așteptare" per utilizator (Pending = 0)
        modelBuilder.Entity<SellerRequest>()
            .HasIndex(r => r.UserId)
            .HasFilter("[Status] = 0")
            .IsUnique();
    }
}