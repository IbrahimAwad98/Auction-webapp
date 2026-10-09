using System.ComponentModel.DataAnnotations;
using AuctionApp.Core;
namespace AuctionApp.Models.Auctions;

/**
 * klass för att skapa formuläret.
 * använder etiketter istället än if-satser
 */
public class CreateAuctionVm
{
    [Required] 
    [StringLength(100)]
    public string Name { get; set; } = "";
    
    [StringLength(2000)]
    public string Description { get; set; } = "";
    
    [Range(0, 10000000)]
    [Display(Name = "Starting price")]
    public decimal StartingPrice { get; set; }
    
    [Display(Name = "End time")]
    public DateTime EndTime { get; set; }
}