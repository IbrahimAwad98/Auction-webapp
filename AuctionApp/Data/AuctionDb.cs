namespace AuctionApp.Data;

public class AuctionDb
{
    public int Id { get; set; }
    public String Name { get; set; } = string.Empty;
    public String Description { get; set; } = String.Empty;
    public String OwnerUserName { get; set; } = String.Empty;
    public decimal StartingPrice { get; set; }
    public DateTime EndTime { get; set; }
    
}