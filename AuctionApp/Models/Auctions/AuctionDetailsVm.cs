using AuctionApp.Core;

namespace AuctionApp.Models.Auctions;

// Own VM for the details page so the view gets price, ownership and bids ready-made
// without knowing the domain model.
public class AuctionDetailsVm
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string OwnerUserName { get; set; } = "";
    public decimal StartingPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsOwner { get; set; }
    public bool IsActive { get; set; }
    public string? Winner { get; set; }
    public List<BidVm> Bids { get; set; } = new List<BidVm>();

    public static AuctionDetailsVm FromAuction(Auction auction, string userName, DateTime now)
    {
        return new AuctionDetailsVm
        {
            Id = auction.Id,
            Name = auction.Name,
            Description = auction.Description,
            OwnerUserName = auction.OwnerUserName,
            StartingPrice = auction.StartingPrice,
            CurrentPrice = auction.CurrentPrice(),
            EndTime = auction.EndTime,
            IsOwner = auction.IsOwnedBy(userName),
            IsActive = auction.IsActive(now),
            Winner = auction.Winner(now),
            // Highest bid first, which is how the page lists them.
            Bids = auction.GetBids()
                .OrderByDescending(bid => bid.Amount)
                .Select(BidVm.FromBid)
                .ToList()
        };
    }
}
