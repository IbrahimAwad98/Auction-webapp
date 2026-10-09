using AuctionApp.Core.Interfaces;

namespace AuctionApp.Core;

// Tillfällig tjänst med testdata i minnet. Tas bort när databasen finns.
public class MockAuctionService : IAuctionService
{

    private static List<Auction> auctions = new List<Auction>();
    private static int nextId = 1;

    public MockAuctionService()
    {
        if (auctions.Count == 0)
        {
            AddTestData();
        }
    }

    public void AddTestData()
    {
        DateTime now = DateTime.Now;

        // 1: pågående, ägs av iawad, inga bud
        List<Bid> bids1 = new List<Bid>();
        auctions.Add(new Auction(1, "Gaming laptop", "Laptop with 16 GB RAM.",
            "iawad@kth.se", 4000, now.AddDays(1), bids1));

        // 2: pågående, ägs av wajeed, iawad har budat
        List<Bid> bids2 = new List<Bid>();
        bids2.Add(new Bid("iawad@kth.se", 600, now.AddHours(-5)));
        bids2.Add(new Bid("sara@kth.se", 750, now.AddHours(-2)));
        auctions.Add(new Auction(2, "Mountain bike", "Used mountain bike, 21 gears.",
            "wajeed@kth.se", 500, now.AddDays(3), bids2));

        // 3: avslutad, ägs av wajeed, vunnen av iawad
        List<Bid> bids3 = new List<Bid>();
        bids3.Add(new Bid("sara@kth.se", 250, now.AddDays(-4)));
        bids3.Add(new Bid("iawad@kth.se", 300, now.AddDays(-3)));
        auctions.Add(new Auction(3, "Office chair", "Ergonomic office chair.",
            "wajeed@kth.se", 200, now.AddDays(-2), bids3));

        nextId = 4;
    }
    
    
    private Auction FindById(int auctionId)
    {
        foreach (Auction auction in auctions)
        {
            if (auction.Id == auctionId)
            {
                return auction;
            }
        }
        throw new AuctionException("The auction does not exist.");
    }

    private bool HasBidBy(Auction auction, string userName)
    {
        foreach (Bid bid in auction.GetBids())
        {
            if (bid.UserName == userName)
            {
                return true;
            }
        }
        return false;
    }
    
    public List<Auction> ListActiveAuctions()
    {
        DateTime now = DateTime.Now;
        List<Auction> result = new List<Auction>();

        foreach (Auction auction in auctions)
        {
            if (auction.IsActive(now))
            {
                result.Add(auction);
            }
        }

        return result;
    }
    
    public List<Auction> ListActiveAuctionsWithBidBy(string userName)
    {
        DateTime now = DateTime.Now;
        List<Auction> result = new List<Auction>();

        foreach (Auction auction in auctions)
        {
            if (auction.IsActive(now) && HasBidBy(auction, userName))
            {
                result.Add(auction);
            }
        }

        return result;
    }

    public List<Auction> ListAuctionsWonBy(string userName)
    {
        DateTime now = DateTime.Now;
        List<Auction> result = new List<Auction>();

        foreach (Auction auction in auctions)
        {
            if (auction.Winner(now) == userName)
            {
                result.Add(auction);
            }
        }

        return result;
    }

    public Auction GetAuctionDetails(int auctionId)
    {
        Auction auction = FindById(auctionId);

        if (!auction.IsActive(DateTime.Now))
        {
            throw new AuctionException("The auction has ended.");
        }

        return auction;
    }

    public Auction GetAuctionForOwner(int auctionId, string userName)
    {
        Auction auction = FindById(auctionId);

        if (!auction.IsOwnedBy(userName))
        {
            throw new AuctionException("Only the owner can edit the auction.");
        }

        return auction;
    }

    public void CreateAuction(string name, string description, decimal startingPrice,
        DateTime endTime, string userName)
    {
        // Första konstruktorn kontrollerar reglerna och kastar undantag vid fel
        Auction checkedAuction = new Auction(name, description, userName,
            startingPrice, endTime, DateTime.Now);

        // Andra konstruktorn ger auktionen ett id, som databasen gör senare
        Auction saved = new Auction(nextId, name, description, userName,
            startingPrice, endTime, new List<Bid>());

        auctions.Add(saved);
        nextId = nextId + 1;
    }

    public void UpdateDescription(int auctionId, string newDescription, string userName)
    {
        Auction auction = FindById(auctionId);
        auction.UpdateDescription(newDescription, userName);
    }

    public void PlaceBid(int auctionId, decimal amount, string userName)
    {
        Auction auction = FindById(auctionId);
        auction.PlaceBid(userName, amount, DateTime.Now);
    }
}