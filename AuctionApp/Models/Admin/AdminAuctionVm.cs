using AuctionApp.Core;

namespace AuctionApp.Models.Admin;

// One row in the admin's list of a user's auctions.
public class AdminAuctionVm
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal CurrentPrice { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsActive { get; set; }

    public static AdminAuctionVm FromAuction(Auction auction, DateTime now)
    {
        return new AdminAuctionVm
        {
            Id = auction.Id,
            Name = auction.Name,
            CurrentPrice = auction.CurrentPrice(),
            EndTime = auction.EndTime,
            IsActive = auction.IsActive(now)  
        };
    }
}