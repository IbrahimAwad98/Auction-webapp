using AuctionApp.Core.Interfaces;

namespace AuctionApp.Core;

public class MockAuctionService : IAuctionService
{
    public List<Auction> ListActiveAuctions()
    {
        DateTime now = DateTime.Now;

        Auction a1 = new Auction(1, "Gaming laptop", "Laptop with 16 GB RAM.",
            "ibbe@kth.se", 4000, now.AddDays(1), new List<Bid>());
        Auction a2 = new Auction(2, "Mountain bike", "Used mountain bike, 21 gears.",
            "anna@kth.se", 500, now.AddDays(3), new List<Bid>());

        List<Auction> auctions = new List<Auction>();
        auctions.Add(a1);
        auctions.Add(a2);
        return auctions;
    }
}