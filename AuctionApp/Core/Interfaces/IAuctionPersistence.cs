namespace AuctionApp.Core.Interfaces;

// Boundary between Core and the data layer. Core only sees domain objects, never EF entities.
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
