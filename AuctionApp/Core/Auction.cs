namespace AuctionApp.Core;
/**
 * Auction klass
 * två constructor en för nytt auction och en för befintligt från databas. 
 */
public class Auction
{
    public int Id { get; }
    public string Name { get; }
    public string Description { get; private set; }
    public string OwnerUserName { get; }
    public decimal StartingPrice { get; }
    public DateTime EndTime { get; }
    private List<Bid> bids = new List<Bid>();

    public Auction(string name, string description, string ownerUserName, 
        decimal startingPrice, DateTime endTime, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new AuctionException("An auction must have a name.");
        }
        if (string.IsNullOrWhiteSpace(ownerUserName))
        {
            throw new AuctionException("An auction must have a owner.");
        }
        if (startingPrice < 0)
        {
            throw new AuctionException("The starting price cannot be negative.");
        }
        if (endTime <= now)
        {
            throw new AuctionException("The end time must be in the future.");
        }
        Name = name;
        Description = description;
        OwnerUserName = ownerUserName;
        StartingPrice = startingPrice;
        EndTime = endTime;
    }

    
    public Auction(int id, string name, string description,
        string ownerUserName, decimal startingPrice, DateTime endTime, List<Bid> existingsBids)
    {
        Id = id;
        Name = name;
        Description = description;
        OwnerUserName = ownerUserName;
        StartingPrice = startingPrice;
        EndTime = endTime;
        foreach (Bid bid in existingsBids)
        {
            bids.Add(bid);
        }
    }

    // Returnerar en kopia
    public List<Bid> GetBids()
    {
        return new List<Bid>(bids);
    }

    public bool IsActive(DateTime now)
    {
        return now < EndTime;
    }

    public bool IsOwnedBy(String userName)
    {
        return OwnerUserName == userName;
    }
    
    //kan vara null
    public Bid? HighestBid()
    {
        Bid? highest = null;

        foreach (Bid bid in bids)
        {
            if (highest == null || bid.Amount > highest.Amount)
            {
                highest = bid;
            }
        }

        return highest;
    }

    public Bid HighestBig()
    {
        Bid? highest = null;
        foreach (Bid bid in bids)
        {
            if (highest == null || bid.Amount > highest.Amount)
            {
                highest = bid;
            }
        }

        return highest;
    }

    public decimal CurrentPrice()
    {
        Bid? highest = HighestBid();
        if (highest == null)
        {
            return StartingPrice;
        }

        return highest.Amount;
    }
    
    
    public void UpdateDescription(string newDescription, string userName)
    {
        if (!IsOwnedBy(userName))
        {
            throw new AuctionException("Only the owner can edit the auction.");
        }

        Description = newDescription;
    }
    
    public Bid PlaceBid(string userName, decimal amount, DateTime now)
    {
        if (!IsActive(now))
        {
            throw new AuctionException("The auction has ended.");
        }
        if (IsOwnedBy(userName))
        {
            throw new AuctionException("You cannot bid on your own auction.");
        }
        if (amount <= CurrentPrice())
        {
            throw new AuctionException("The bid must be higher than the current price.");
        }

        Bid bid = new Bid(userName, amount, now);
        bids.Add(bid);
        return bid;
    }

    public string? Winner(DateTime now)
    {
        if (IsActive(now))
        {
            return null;
        }

        Bid? highest = HighestBid();

        if (highest == null)
        {
            return null;
        }

        return highest.UserName;
    }
    
}