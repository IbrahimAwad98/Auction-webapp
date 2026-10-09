using AuctionApp.Core.Interfaces;

namespace AuctionApp.Core;

// Kopplar ihop persistens och domänregler så controllern slipper känna till båda.
// DateTime.Now hämtas här för att Auction/Bid ska förbli tidsoberoende.
public class AuctionService : IAuctionService
{
    private readonly IAuctionPersistence persistence;

    public AuctionService(IAuctionPersistence persistence)
    {
        this.persistence = persistence;
    }

    public List<Auction> ListActiveAuctions()
    {
        DateTime now = DateTime.Now;
        return persistence.GetActive(now);
    }

    public List<Auction> ListActiveAuctionsWithBidBy(string userName)
    {
        DateTime now = DateTime.Now;
        return persistence.GetActiveWithBidBy(userName, now);
    }

    public List<Auction> ListAuctionsWonBy(string userName)
    {
        DateTime now = DateTime.Now;
        return persistence.GetEndedWonBy(userName, now);
    }

    public Auction GetAuctionDetails(int auctionId)
    {
        DateTime now = DateTime.Now;
        Auction? auction = persistence.GetByIdWithBids(auctionId);
        if (auction == null)
        {
            throw new AuctionException("The auction does not exist.");
        }

        if (!auction.IsActive(now))
        {
            throw new AuctionException("The auction has ended.");
        }

        return auction;
    }

    public Auction GetAuctionForOwner(int auctionId, string userName)
    {
        Auction? auction = persistence.GetById(auctionId);
        if (auction == null)
        {
            throw new AuctionException("The auction does not exist.");
        }

        if (!auction.IsOwnedBy(userName))
        {
            throw new AuctionException("Only the owner can edit the auction.");
        }
        return auction;
    }

    public void CreateAuction(string name, string description, decimal startingPrice, DateTime endTime, string userName)
    {
        DateTime now = DateTime.Now;
        Auction? auction = new Auction(name, description, userName, startingPrice, endTime, now);
        persistence.Add(auction);
    }

    public void UpdateDescription(int auctionId, string newDescription, string userName)
    {
        Auction? auction = persistence.GetById(auctionId);
        if (auction == null)
        {
            throw new AuctionException("The auction does not exist.");
        }
        auction.UpdateDescription(newDescription, userName);
        persistence.UpdateDescription(auctionId, auction.Description);
    }

    public void PlaceBid(int auctionId, decimal amount, string userName)
    {
        DateTime now = DateTime.Now;
        Auction? auction = persistence.GetByIdWithBids(auctionId);
        if (auction == null)
        {
            throw new AuctionException("The auction does not exist.");
        }
        Bid bid = auction.PlaceBid(userName, amount, now);
        persistence.AddBid(auctionId, bid);
    }
}