using AuctionApp.Core.Interfaces;

namespace AuctionApp.Core;

// Connects persistence and domain rules so the controller does not need to know either.
// DateTime.Now is read here so Auction and Bid stay independent of the clock.
public class AuctionService : IAuctionService
{
    private readonly IAuctionPersistence persistence;

    public AuctionService(IAuctionPersistence persistence)
    {
        this.persistence = persistence;
    }

    public List<Auction> ListActiveAuctions()
    {
        return persistence.GetActive(DateTime.Now);
    }

    public List<Auction> ListActiveAuctionsWithBidBy(string userName)
    {
        return persistence.GetActiveWithBidBy(userName, DateTime.Now);
    }

    public List<Auction> ListAuctionsWonBy(string userName)
    {
        return persistence.GetEndedWonBy(userName, DateTime.Now);
    }

    // Ended auctions are returned too, so won auctions can still be viewed.
    public Auction GetAuctionDetails(int auctionId)
    {
        return persistence.GetByIdWithBids(auctionId)
               ?? throw new AuctionException("The auction does not exist.");
    }

    public Auction GetAuctionForOwner(int auctionId, string userName)
    {
        Auction auction = persistence.GetById(auctionId)
                          ?? throw new AuctionException("The auction does not exist.");
        if (!auction.IsOwnedBy(userName))
        {
            throw new AuctionException("Only the owner can edit the auction.");
        }
        return auction;
    }

    public void CreateAuction(string name, string description, decimal startingPrice, DateTime endTime, string userName)
    {
        Auction auction = new Auction(name, description, userName, startingPrice, endTime, DateTime.Now);
        persistence.Add(auction);
    }

    public void UpdateDescription(int auctionId, string newDescription, string userName)
    {
        Auction auction = persistence.GetById(auctionId)
                          ?? throw new AuctionException("The auction does not exist.");
        auction.UpdateDescription(newDescription, userName);
        persistence.UpdateDescription(auctionId, auction.Description);
    }

    public void PlaceBid(int auctionId, decimal amount, string userName)
    {
        Auction auction = persistence.GetByIdWithBids(auctionId)
                          ?? throw new AuctionException("The auction does not exist.");
        Bid bid = auction.PlaceBid(userName, amount, DateTime.Now);
        persistence.AddBid(auctionId, bid);
    }
}
