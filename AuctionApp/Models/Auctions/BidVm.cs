using AuctionApp.Core;

namespace AuctionApp.Models.Auctions;

// Keeps the view's bid separate from the domain Bid so the view can change without touching the rules.
public class BidVm
{
    public string UserName { get; set; } = "";
    public decimal Amount { get; set; }
    public DateTime PlacedAt { get; set; }

    public static BidVm FromBid(Bid bid)
    {
        return new BidVm
        {
            UserName = bid.UserName,
            Amount = bid.Amount,
            PlacedAt = bid.PlacedAt
        };
    }
}
