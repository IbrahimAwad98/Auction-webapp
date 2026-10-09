using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auctions;

public class PlaceBidVm
{
    public int AuctionId { get; set; }
    
    [Range(1, 10000000)]
    public decimal Amount { get; set; }
    
}