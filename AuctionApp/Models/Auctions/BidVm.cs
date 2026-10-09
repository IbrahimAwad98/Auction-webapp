using AuctionApp.Core;

namespace AuctionApp.Models.Auctions;

public class BidVm
{
    public string UserName { get; set; } = "";  //kan vara null
    public decimal Amount { get; set; }
    public DateTime PlacedAt { get; set; }

    public static BidVm fromBid(Bid bid)
    {
        BidVm vm = new BidVm();
        vm.UserName = bid.UserName;
        vm.Amount = bid.Amount;
        vm.PlacedAt = bid.PlacedAt;
        return vm;
    }
}