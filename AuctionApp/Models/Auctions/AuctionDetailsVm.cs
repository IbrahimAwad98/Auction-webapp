using AuctionApp.Core;
namespace AuctionApp.Models.Auctions;


// Egen VM för detaljsidan så vyn får pris, ägarskap och bud färdiga utan att känna till domänmodellen.
public class AuctionDetailsVm
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = ""; //kan vara null
    public string OwnerUserName { get; set; } = "";
    public decimal StartingPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsOwner { get; set; }
    public List<BidVm> Bids { get; set; } = new List<BidVm>();

    public static AuctionDetailsVm FromAuction(Auction auction, string userName)
    {
        AuctionDetailsVm vm = new AuctionDetailsVm();
        vm.Id = auction.Id;
        vm.Name = auction.Name;
        vm.Description = auction.Description;
        vm.OwnerUserName = auction.OwnerUserName;
        vm.StartingPrice = auction.StartingPrice;
        vm.CurrentPrice = auction.CurrentPrice();
        vm.EndTime = auction.EndTime;
        vm.IsOwner = auction.IsOwnedBy(userName);

        // översätt varje bid till en bidvm
        foreach (Bid bid in auction.GetBids())
        {
            vm.Bids.Add(BidVm.fromBid(bid));
        }

        // sortera högst bud först
        for (int i = 0; i < vm.Bids.Count - 1; i++)
        {
            for (int j = 0; j < vm.Bids.Count - 1 - i; j++)
            {
                if (vm.Bids[j].Amount < vm.Bids[j + 1].Amount)
                {
                    BidVm temp = vm.Bids[j];
                    vm.Bids[j] = vm.Bids[j + 1];
                    vm.Bids[j + 1] = temp;
                }
            }
        }

        return vm;
    }
}