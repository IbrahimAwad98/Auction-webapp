namespace AuctionApp.Core.Interfaces;

// Boundary between presentation and Core. The controller depends on this, not on the implementation.
public interface IAuctionService
{
    List<Auction> ListActiveAuctions();
    List<Auction> ListActiveAuctionsWithBidBy(string userName);
    List<Auction> ListAuctionsWonBy(string userName);
    List<Auction> ListAuctionsOwnedBy(string userName); //admin
    Auction GetAuctionDetails(int auctionId);
    Auction GetAuctionForOwner(int auctionId, string userName);
    void CreateAuction(string name, string description, decimal startingPrice,
        DateTime endTime, string userName);
    void UpdateDescription(int auctionId, string newDescription, string userName);
    void PlaceBid(int auctionId, decimal amount, string userName);
    void DeleteAuction(int auctionId);
}
