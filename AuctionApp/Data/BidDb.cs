namespace AuctionApp.Data;

// EF entity for a bid. See AuctionDb for why this is separate from the domain class.
public class BidDb
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PlacedAt { get; set; }
    public int AuctionId { get; set; }
    public AuctionDb Auction { get; set; } = null!;
}
