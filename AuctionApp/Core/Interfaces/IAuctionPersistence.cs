namespace AuctionApp.Core.Interfaces;
/**
 * gränsen mellan core och datalager
 */
public interface IAuctionPersistence
{
    List<Auction> GetActive(DateTime now);
    Auction? GetById(int auctionId);
    Auction? GetByIdWithBids(int auctionId);
    List<Auction> GetActiveWithBidBy(string userName, DateTime now);
    List<Auction> GetEndedWonBy(string userName, DateTime now);
    void Add(Auction auction);
    void UpdateDescription(int auctionId, string newDescription);
    void AddBid(int auctionId, Bid bid);
}