using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auctions;

// Form model for creating an auction. The attributes give form validation and error messages
// before the request reaches the domain rules in Auction.
public class CreateAuctionVm
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = "";

    [StringLength(2000)]
    public string Description { get; set; } = "";

    [Range(0, 10000000)]
    [Display(Name = "Starting price")]
    public decimal StartingPrice { get; set; }

    [Display(Name = "End time")]
    public DateTime EndTime { get; set; }
}
