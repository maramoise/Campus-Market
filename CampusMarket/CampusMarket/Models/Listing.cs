using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CampusMarket.Models;

public enum ProductCondition
{
    [Display(Name = "Nou")] New,
    [Display(Name = "Ca nou")] LikeNew,
    [Display(Name = "Folosit")] Used
}

public enum ListingStatus
{
    [Display(Name = "În așteptare")] Pending,
    [Display(Name = "Aprobat")] Approved,
    [Display(Name = "Respins")] Rejected
}

public enum RequestStatus
{
    [Display(Name = "În așteptare")] Pending,
    [Display(Name = "Aprobată")] Approved,
    [Display(Name = "Respinsă")] Rejected
}

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Numele categoriei este obligatoriu")]
    [StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
}

public class Listing
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Titlul este obligatoriu")]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrierea este obligatorie")]
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Imaginea este obligatorie")]
    public string ImagePath { get; set; } = string.Empty;

    [Range(0.01, 1000000, ErrorMessage = "Prețul trebuie să fie mai mare decât 0")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stocul nu poate fi negativ")]
    public int Stock { get; set; }

    [Required(ErrorMessage = "Starea produsului este obligatorie")]
    public ProductCondition Condition { get; set; }

    public ListingStatus Status { get; set; } = ListingStatus.Pending;

    public string? AdminFeedback { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    [Required(ErrorMessage = "Categoria este obligatorie")]
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;

    public string SellerId { get; set; } = string.Empty;
    public virtual IdentityUser Seller { get; set; } = null!;

    [Timestamp]
    public byte[]? RowVersion { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<FaqItem> FaqItems { get; set; } = new List<FaqItem>();
}

public class Rating
{
    [Key]
    public int Id { get; set; }

    [Range(1, 5, ErrorMessage = "Rating-ul trebuie să fie între 1 și 5")]
    public int Value { get; set; }

    public int ListingId { get; set; }
    public virtual Listing Listing { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public virtual IdentityUser User { get; set; } = null!;
}

public class Review
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Comentariul nu poate fi gol")]
    [StringLength(1000)]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EditedAt { get; set; }

    public int ListingId { get; set; }
    public virtual Listing Listing { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public virtual IdentityUser User { get; set; } = null!;
}

public class SellerRequest
{
    [Key]
    public int Id { get; set; }

    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }

    public string UserId { get; set; } = string.Empty;
    public virtual IdentityUser User { get; set; } = null!;
}

public class CartItem
{
    [Key]
    public int Id { get; set; }

    [Range(1, 1000, ErrorMessage = "Cantitatea trebuie să fie cel puțin 1")]
    public int Quantity { get; set; } = 1;

    public string UserId { get; set; } = string.Empty;
    public virtual IdentityUser User { get; set; } = null!;

    public int ListingId { get; set; }
    public virtual Listing Listing { get; set; } = null!;
}

public class Favorite
{
    public string UserId { get; set; } = string.Empty;
    public virtual IdentityUser User { get; set; } = null!;

    public int ListingId { get; set; }
    public virtual Listing Listing { get; set; } = null!;
}

public class Reservation
{
    [Key]
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; } = string.Empty;
    public virtual IdentityUser User { get; set; } = null!;

    public virtual ICollection<ReservationItem> Items { get; set; } = new List<ReservationItem>();
}

public class ReservationItem
{
    [Key]
    public int Id { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public int ReservationId { get; set; }
    public virtual Reservation Reservation { get; set; } = null!;

    public int ListingId { get; set; }
    public virtual Listing Listing { get; set; } = null!;
}

public class FaqItem
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Întrebarea este obligatorie")]
    public string Question { get; set; } = string.Empty;

    [Required(ErrorMessage = "Răspunsul este obligatoriu")]
    public string Answer { get; set; } = string.Empty;

    public int ListingId { get; set; }
    public virtual Listing Listing { get; set; } = null!;
}