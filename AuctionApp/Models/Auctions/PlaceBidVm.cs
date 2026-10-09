using System.ComponentModel.DataAnnotations;

namespace AuctionApp.Models.Auctions;

// Egen VM för bud så formulärvalidering sker innan affärsreglerna i AuctionService.
public class PlaceBidVm
{
    public int AuctionId { get; set; }
    
    [Range(1, 10000000)]
    public decimal Amount { get; set; }
    
}