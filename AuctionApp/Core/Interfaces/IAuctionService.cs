namespace AuctionApp.Core.Interfaces;
/**
 * gränsen mellan presentation och core
 */
public interface IAuctionService
{
    List<Auction> ListActiveAuctions();
    List<Auction> ListActiveAuctionsWithBidBy(String userName);
    List<Auction> ListAuctionsWonBy(String userName);
    Auction GetAuctionDetails(int auctionId);
    Auction GetAuctionForOwner(int auctionId, String userName);
    void CreateAuction(string name, string description, decimal startingPrice,
        DateTime endTime, string userName);
    void UpdateDescription(int auctionId, string newDescription, string userName);
    void PlaceBid(int auctionId, decimal amount, string userName);
}