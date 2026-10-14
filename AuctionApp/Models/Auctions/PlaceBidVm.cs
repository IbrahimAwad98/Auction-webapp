using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auctions;

// Form model for a bid. Range rejects zero and negative amounts before the rules in Auction run.
public class PlaceBidVm
{
    public int AuctionId { get; set; }

    [Range(1, 10000000)]
    public decimal Amount { get; set; }
}
