namespace AuctionApp.Core;

// The business rules live here, not in the controller, so they can be tested without web or database.
// "now" is passed in so the time-based rules can be tested deterministically.
public class Auction
{
    public int Id { get; }
    public string Name { get; }
    public string Description { get; private set; }
    public string OwnerUserName { get; }
    public decimal StartingPrice { get; }
    public DateTime EndTime { get; }
    private readonly List<Bid> bids;

    public Auction(string name, string description, string ownerUserName,
        decimal startingPrice, DateTime endTime, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new AuctionException("An auction must have a name.");
        }
        if (string.IsNullOrWhiteSpace(ownerUserName))
        {
            throw new AuctionException("An auction must have an owner.");
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
        bids = new List<Bid>();
    }

    // No validation here: the data comes from the database and was validated when it was saved.
    public Auction(int id, string name, string description,
        string ownerUserName, decimal startingPrice, DateTime endTime, List<Bid> existingBids)
    {
        Id = id;
        Name = name;
        Description = description;
        OwnerUserName = ownerUserName;
        StartingPrice = startingPrice;
        EndTime = endTime;
        bids = new List<Bid>(existingBids);
    }

    // A copy, so nobody outside can add bids and bypass the rules in PlaceBid.
    public List<Bid> GetBids()
    {
        return new List<Bid>(bids);
    }

    public bool IsActive(DateTime now)
    {
        return now < EndTime;
    }

    public bool IsOwnedBy(string userName)
    {
        return OwnerUserName == userName;
    }

    public Bid? HighestBid()
    {
        return bids.MaxBy(bid => bid.Amount);
    }

    public decimal CurrentPrice()
    {
        return HighestBid()?.Amount ?? StartingPrice;
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

    // Null while the auction is still running or if nobody bid.
    public string? Winner(DateTime now)
    {
        if (IsActive(now))
        {
            return null;
        }
        return HighestBid()?.UserName;
    }
}
