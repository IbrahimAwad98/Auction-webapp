namespace AuctionApp.Data;

// Flat EF entity with public setters. The domain class Auction has rules and private fields
// that EF cannot map, so the two are kept apart and translated in AuctionPersistence.
public class AuctionDb
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OwnerUserName { get; set; } = string.Empty;
    public decimal StartingPrice { get; set; }
    public DateTime EndTime { get; set; }
}
