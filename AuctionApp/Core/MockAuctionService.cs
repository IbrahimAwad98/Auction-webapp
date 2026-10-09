using AuctionApp.Core.Interfaces;

namespace AuctionApp.Core;

// Hårdkodad data så vyerna kan byggas och visas innan databasen finns (labbens första steg).
public class MockAuctionService : IAuctionService
{
    public List<Auction> ListActiveAuctions()
    {
        DateTime now = DateTime.Now;

        Auction a1 = new Auction(1, "Gaming laptop", "Laptop with 16 GB RAM.",
            "iawad@kth.se", 4000, now.AddDays(1), new List<Bid>());
        Auction a2 = new Auction(2, "Mountain bike", "Used mountain bike, 21 gears.",
            "wajeed@kth.se", 500, now.AddDays(3), new List<Bid>());

        List<Auction> auctions = new List<Auction>();
        auctions.Add(a1);
        auctions.Add(a2);
        return auctions;
    }
    
    // Resten är inte implementerat i mocken
    public List<Auction> ListActiveAuctionsWithBidBy(string userName)
    {
        throw new NotImplementedException();
    }

    public List<Auction> ListAuctionsWonBy(string userName)
    {
        throw new NotImplementedException();
    }

    public Auction GetAuctionDetails(int auctionId)
    {
        throw new NotImplementedException();
    }

    public Auction GetAuctionForOwner(int auctionId, string userName)
    {
        throw new NotImplementedException();
    }

    public void CreateAuction(string name, string description, decimal startingPrice,
        DateTime endTime, string userName)
    {
        throw new NotImplementedException();
    }

    public void UpdateDescription(int auctionId, string newDescription, string userName)
    {
        throw new NotImplementedException();
    }

    public void PlaceBid(int auctionId, decimal amount, string userName)
    {
        throw new NotImplementedException();
    }
    
}