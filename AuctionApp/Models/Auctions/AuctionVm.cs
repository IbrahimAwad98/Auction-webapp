using AuctionApp.Core;

namespace AuctionApp.Models.Auctions;

public class AuctionVm
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OwnerUserName { get; set; }
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
