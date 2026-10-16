using AuctionApp.Core;
using AuctionApp.Core.Interfaces;

namespace AuctionApp.Data;

// Translates between the EF entities (AuctionDb, BidDb) and the domain objects,
// so Core never depends on Entity Framework.
public class AuctionPersistence : IAuctionPersistence
{
    private readonly AuctionDbContext db;

    public AuctionPersistence(AuctionDbContext db)
    {
        this.db = db;
    }

    public List<Auction> GetActive(DateTime now)
    {
        List<AuctionDb> auctions = db.Auctions
            .Where(a => a.EndTime > now)
            .OrderBy(a => a.EndTime)
            .ToList();

        if (auctions.Count == 0)
        {
            return new List<Auction>();
        }

        List<int> ids = auctions.Select(a => a.Id).ToList();

        List<BidDb> highestBids = db.Bids
            .Where(b => ids.Contains(b.AuctionId) &&
                        !db.Bids.Any(other =>
                            other.AuctionId == b.AuctionId &&
                            other.Amount > b.Amount))
            .ToList();

        return auctions
            .Select(a => ToAuction(a,
                highestBids.Where(b => b.AuctionId == a.Id).ToList()))
            .ToList();
    }

    public Auction? GetById(int auctionId)
    {
        AuctionDb? auction = db.Auctions.FirstOrDefault(a => a.Id == auctionId);
        return auction == null ? null : ToAuction(auction, new List<BidDb>());
    }

    public Auction? GetByIdWithBids(int auctionId)
    {
        AuctionDb? auction = db.Auctions.FirstOrDefault(a => a.Id == auctionId);
        if (auction == null)
        {
            return null;
        }
        List<BidDb> bids = db.Bids.Where(b => b.AuctionId == auctionId).ToList();
        return ToAuction(auction, bids);
    }

    public List<Auction> GetActiveWithBidBy(string userName, DateTime now)
    {
        List<AuctionDb> auctions = db.Auctions
            .Where(a => a.EndTime > now &&
                        db.Bids.Any(b =>
                            b.AuctionId == a.Id &&
                            b.UserName == userName))
            .OrderBy(a => a.EndTime)
            .ToList();

        if (auctions.Count == 0)
        {
            return new List<Auction>();
        }

        List<int> ids = auctions.Select(a => a.Id).ToList();

        List<BidDb> highestBids = db.Bids
            .Where(b => ids.Contains(b.AuctionId) &&
                        !db.Bids.Any(other =>
                            other.AuctionId == b.AuctionId &&
                            other.Amount > b.Amount))
            .ToList();

        return auctions
            .Select(a => ToAuction(a,
                highestBids.Where(b => b.AuctionId == a.Id).ToList()))
            .ToList();
    }

    // The winner is decided in the query so only won auctions are loaded.
    // Auction.Winner() still holds the rule for the details page.
    public List<Auction> GetEndedWonBy(string userName, DateTime now)
    {
        List<AuctionDb> auctions = db.Auctions
            .Where(a => a.EndTime <= now &&
                        db.Bids
                            .Where(b => b.AuctionId == a.Id)
                            .OrderByDescending(b => b.Amount)
                            .Select(b => b.UserName)
                            .FirstOrDefault() == userName)
            .OrderByDescending(a => a.EndTime)
            .ToList();

        return ToAuctions(auctions);
    }

    public void Add(Auction auction)
    {
        db.Auctions.Add(new AuctionDb
        {
            Name = auction.Name,
            Description = auction.Description,
            OwnerUserName = auction.OwnerUserName,
            StartingPrice = auction.StartingPrice,
            EndTime = auction.EndTime
        });
        db.SaveChanges();
    }

    public void UpdateDescription(int auctionId, string newDescription)
    {
        AuctionDb auction = db.Auctions.FirstOrDefault(a => a.Id == auctionId)
                            ?? throw new KeyNotFoundException("Auction not found.");
        auction.Description = newDescription;
        db.SaveChanges();
    }

    public void AddBid(int auctionId, Bid bid)
    {
        db.Bids.Add(new BidDb
        {
            AuctionId = auctionId,
            UserName = bid.UserName,
            Amount = bid.Amount,
            PlacedAt = bid.PlacedAt
        });
        db.SaveChanges();
    }

    // One query for all bids instead of one per auction.
    private List<Auction> ToAuctions(List<AuctionDb> auctions)
    {
        List<int> ids = auctions.Select(a => a.Id).ToList();
        List<BidDb> bids = db.Bids.Where(b => ids.Contains(b.AuctionId)).ToList();
        return auctions
            .Select(a => ToAuction(a, bids.Where(b => b.AuctionId == a.Id).ToList()))
            .ToList();
    }

    private static Auction ToAuction(AuctionDb a, List<BidDb> bids)
    {
        return new Auction(a.Id, a.Name, a.Description, a.OwnerUserName, a.StartingPrice, a.EndTime,
            bids.Select(b => new Bid(b.Id, b.UserName, b.Amount, b.PlacedAt)).ToList());
    }
}
