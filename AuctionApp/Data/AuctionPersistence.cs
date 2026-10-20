using AuctionApp.Core;
using AuctionApp.Core.Interfaces;
using AuctionApp.Data.Repositories;

namespace AuctionApp.Data;

// Implements the persistence interface that Core owns.
// All database access goes through the generic repository, one per entity type.
// EF classes (AuctionDb, BidDb) never leave this class: everything returned is Auction/Bid.
public class AuctionPersistence : IAuctionPersistence
{
    private readonly IRepository<AuctionDb> _auctions;
    private readonly IRepository<BidDb> _bids;
    
    public AuctionPersistence(AuctionDbContext db)
    {
        _auctions = new Repository<AuctionDb>(db);
        _bids = new Repository<BidDb>(db);
    }
    
    public List<Auction> GetActive(DateTime now)
    {
        return LoadWithHighestBid(_auctions.Query()
            .Where(a => a.EndTime > now)
            .OrderBy(a => a.EndTime));
    }

    // Admin view: every auction a user has created, ended ones included.
    public List<Auction> GetByOwner(string userName)
    {
        return LoadWithHighestBid(_auctions.Query()
            .Where(a => a.OwnerUserName == userName)
            .OrderByDescending(a => a.EndTime));
    }
    
    public Auction? GetById(int id)
    {
        AuctionDb? entity = _auctions.GetById(id);
        if (entity == null)
        {
            return null;
        }
        return ToAuction(entity, new List<BidDb>());
    }
    
    public Auction? GetByIdWithBids(int id)
    {
        AuctionDb? entity = _auctions.GetById(id);
        if (entity == null)
        {
            return null;
        }

        List<BidDb> bidRows = _bids.Query()
            .Where(b => b.AuctionId == id)
            .ToList();

        return ToAuction(entity, bidRows);
    }
    
    public List<Auction> GetActiveWithBidBy(string userName, DateTime now)
    {
        IQueryable<BidDb> bids = _bids.Query();

        return LoadWithHighestBid(_auctions.Query()
            .Where(a => a.EndTime > now
                        && bids.Any(b => b.AuctionId == a.Id && b.UserName == userName))
            .OrderBy(a => a.EndTime));
    }

    public List<Auction> GetEndedWonBy(string userName, DateTime now)
    {
        IQueryable<BidDb> bids = _bids.Query();

        return LoadWithHighestBid(_auctions.Query()
            .Where(a => a.EndTime <= now
                        && bids.Where(b => b.AuctionId == a.Id)
                               .OrderByDescending(b => b.Amount)
                               .Select(b => b.UserName)
                               .FirstOrDefault() == userName)
            .OrderByDescending(a => a.EndTime));
    }
    
    public void Add(Auction auction)
    {
        AuctionDb entity = new AuctionDb
        {
            Name = auction.Name,
            Description = auction.Description,
            OwnerUserName = auction.OwnerUserName,
            StartingPrice = auction.StartingPrice,
            EndTime = auction.EndTime
        };

        _auctions.Add(entity);
        _auctions.SaveChanges();
    }

    public void UpdateDescription(int auctionId, string description)
    {
        AuctionDb? entity = _auctions.GetById(auctionId);
        if (entity == null)
        {
            throw new AuctionException("Auction not found.");
        }

        entity.Description = description; 
        _auctions.SaveChanges();
    }
    
    public void AddBid(int auctionId, Bid bid)
    {
        BidDb entity = new BidDb
        {
            AuctionId = auctionId,   
            UserName = bid.UserName,
            Amount = bid.Amount,
            PlacedAt = bid.PlacedAt
        };

        _bids.Add(entity);
        _bids.SaveChanges();
    }

    // Bids are removed by the database: the FK Bids.AuctionId has ON DELETE CASCADE.
    public void Delete(int auctionId)
    {
        AuctionDb? entity = _auctions.GetById(auctionId);
        if (entity == null)
        {
            throw new AuctionException("Auction not found.");
        }

        _auctions.Remove(entity);
        _auctions.SaveChanges();
    }

    // Runs the query and joins in each auction's highest bid, so list pages can show
    // the current price without loading every bid.
    private List<Auction> LoadWithHighestBid(IQueryable<AuctionDb> query)
    {
        IQueryable<BidDb> bids = _bids.Query();

        var rows = query
            .Select(a => new
            {
                Auction = a,
                HighestBid = bids
                    .Where(b => b.AuctionId == a.Id)
                    .OrderByDescending(b => b.Amount)
                    .FirstOrDefault()
            })
            .ToList();

        List<Auction> result = new List<Auction>();
        foreach (var row in rows)
        {
            result.Add(ToAuction(row.Auction, row.HighestBid));
        }
        return result;
    }
    
    private static Auction ToAuction(AuctionDb entity, BidDb? highestBid)
    {
        List<BidDb> bidRows = new List<BidDb>();
        if (highestBid != null)
        {
            bidRows.Add(highestBid);
        }
        return ToAuction(entity, bidRows);
    }
    
    private static Auction ToAuction(AuctionDb entity, List<BidDb> bidRows)
    {
        List<Bid> bids = new List<Bid>();
        foreach (BidDb row in bidRows)
        {
            bids.Add(new Bid(row.Id, row.UserName, row.Amount, row.PlacedAt));
        }

        return new Auction(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.OwnerUserName,
            entity.StartingPrice,
            entity.EndTime,
            bids);
    }
}
