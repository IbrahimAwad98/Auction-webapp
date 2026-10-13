using AuctionApp.Core;
using AuctionApp.Core.Interfaces;
namespace AuctionApp.Data;

public class AuctionPersistence : IAuctionPersistence
{
    private readonly AuctionDbContext _db;
    public AuctionPersistence(AuctionDbContext db)
    {
        _db = db;
    }

    public List<Auction> GetActive(DateTime now)
    {
        var auctionsDb = _db.Auctions
            .Where(a => a.EndTime > now)
            .OrderBy(a => a.EndTime)
            .ToList();
        var auctions = new List<Auction>();
        foreach (var a in auctionsDb)
        {
            Auction auction = new Auction(
                a.Id,
                a.Name,
                a.Description,
                a.OwnerUserName,
                a.StartingPrice,
                a.EndTime,
                new List<Bid>()
            );
            auctions.Add(auction);
        }
        return auctions;
    }

    public Auction? GetById(int auctionId)
    {
        var auctionDb = _db.Auctions
            .FirstOrDefault(a => a.Id == auctionId);
        if (auctionDb == null) return null;
        
        return new Auction(
            auctionDb.Id,
            auctionDb.Name,
            auctionDb.Description,
            auctionDb.OwnerUserName,
            auctionDb.StartingPrice,
            auctionDb.EndTime,
            new List<Bid>()
        );
        
    }

    public Auction? GetByIdWithBids(int auctionId)
    {
        var auctionDb = _db.Auctions
            .FirstOrDefault(a => a.Id == auctionId);

        if (auctionDb == null)
        {
            return null;
        }

        var bidsDb = _db.Bids
            .Where(b => b.AuctionId == auctionId)
            .OrderByDescending(b => b.Amount)
            .ToList();

        var bids = new List<Bid>();

        foreach (var b in bidsDb)
        {
            bids.Add(new Bid(
                b.Id,
                b.UserName,
                b.Amount,
                b.PlacedAt
            ));
        }

        return new Auction(
            auctionDb.Id,
            auctionDb.Name,
            auctionDb.Description,
            auctionDb.OwnerUserName,
            auctionDb.StartingPrice,
            auctionDb.EndTime,
            bids
        );
    }

    public List<Auction> GetActiveWithBidBy(string userName, DateTime now)
    {
        var auctionsDb = _db.Auctions
            .Where(a => a.EndTime > now &&
                        _db.Bids.Any(b => b.AuctionId == a.Id &&
                                          b.UserName == userName))
            .OrderBy(a => a.EndTime)
            .ToList();

        var auctionIds = auctionsDb.Select(a => a.Id).ToList();

        var bidsDb = _db.Bids
            .Where(b => auctionIds.Contains(b.AuctionId))
            .ToList();

        var auctions = new List<Auction>();

        foreach (var a in auctionsDb)
        {
            var bids = bidsDb
                .Where(b => b.AuctionId == a.Id)
                .Select(b => new Bid(
                    b.Id,
                    b.UserName,
                    b.Amount,
                    b.PlacedAt
                ))
                .ToList();

            auctions.Add(new Auction(
                a.Id,
                a.Name,
                a.Description,
                a.OwnerUserName,
                a.StartingPrice,
                a.EndTime,
                bids
            ));
        }

        return auctions;
    }

    public List<Auction> GetEndedWonBy(string userName, DateTime now)
    {
        var auctionsDb = _db.Auctions
            .Where(a => a.EndTime <= now &&
                        _db.Bids
                            .Where(b => b.AuctionId == a.Id)
                            .OrderByDescending(b => b.Amount)
                            .Select(b => b.UserName)
                            .FirstOrDefault() == userName)
            .OrderByDescending(a => a.EndTime)
            .ToList();

        var auctionIds = auctionsDb.Select(a => a.Id).ToList();

        var bidsDb = _db.Bids
            .Where(b => auctionIds.Contains(b.AuctionId))
            .OrderByDescending(b => b.Amount)
            .ToList();

        var auctions = new List<Auction>();

        foreach (var a in auctionsDb)
        {
            var bids = bidsDb
                .Where(b => b.AuctionId == a.Id)
                .Select(b => new Bid(
                    b.Id,
                    b.UserName,
                    b.Amount,
                    b.PlacedAt
                ))
                .ToList();

            auctions.Add(new Auction(
                a.Id,
                a.Name,
                a.Description,
                a.OwnerUserName,
                a.StartingPrice,
                a.EndTime,
                bids
            ));
        }

        return auctions;
    }

    public void Add(Auction auction)
    {
        var auctionDb = new AuctionDb
        {
            Name = auction.Name,
            Description = auction.Description,
            OwnerUserName = auction.OwnerUserName,
            StartingPrice = auction.StartingPrice,
            EndTime = auction.EndTime
        };

        _db.Auctions.Add(auctionDb);
        _db.SaveChanges();
    }

    public void UpdateDescription(int auctionId, string newDescription)
    {
        var auctionDb = _db.Auctions
            .FirstOrDefault(a => a.Id == auctionId);

        if (auctionDb == null)
        {
            throw new KeyNotFoundException("Auction not found.");
        }

        auctionDb.Description = newDescription;
        _db.SaveChanges();
    }

    public void AddBid(int auctionId, Bid bid)
    {
        var bidDb = new BidDb
        {
            AuctionId = auctionId,
            UserName = bid.UserName,
            Amount = bid.Amount,
            PlacedAt = bid.PlacedAt
        };

        _db.Bids.Add(bidDb);
        _db.SaveChanges();
    }
}