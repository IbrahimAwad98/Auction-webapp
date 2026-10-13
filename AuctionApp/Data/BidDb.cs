namespace AuctionApp.Data;

public class BidDb
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PlacedAt { get; set; }
    public int AuctionId { get; set; } //foreign key
    public AuctionDb Auction { get; set; } = null!;

}