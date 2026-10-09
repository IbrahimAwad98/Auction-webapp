using AuctionApp.Core;

namespace AuctionApp.Models.Auctions;

// Lättviktig VM för listan: bara det som visas, så vyn inte beror på Auction direkt.
public class AuctionVm
{
    public int Id { get; set; }
    public string Name { get; set; } = ""; //kan vara null
    public string OwnerUserName { get; set; } = ""; //kan vara null
    public decimal CurrentPrice { get; set; }
    public DateTime EndTime { get; set; }
    
    public static AuctionVm FromAuction(Auction auction)
    {
        AuctionVm vm = new AuctionVm();
        vm.Id = auction.Id;
        vm.Name = auction.Name;
        vm.OwnerUserName = auction.OwnerUserName;
        vm.CurrentPrice = auction.CurrentPrice();
        vm.EndTime = auction.EndTime;
        return vm;
    }
}
