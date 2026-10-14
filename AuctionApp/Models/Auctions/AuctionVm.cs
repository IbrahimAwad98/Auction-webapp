using AuctionApp.Core;

namespace AuctionApp.Models.Auctions;

// Lightweight VM for the lists: only what is shown, so the views do not depend on Auction.
public class AuctionVm
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string OwnerUserName { get; set; } = "";
    public decimal CurrentPrice { get; set; }
    public DateTime EndTime { get; set; }

    public static AuctionVm FromAuction(Auction auction)
    {
        return new AuctionVm
        {
            Id = auction.Id,
            Name = auction.Name,
            OwnerUserName = auction.OwnerUserName,
            CurrentPrice = auction.CurrentPrice(),
            EndTime = auction.EndTime
        };
    }
}
