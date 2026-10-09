using System.ComponentModel.DataAnnotations;
using AuctionApp.Core;

namespace AuctionApp.Models.Auctions;

public class EditAuctionVm
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    [StringLength(2000)]
    public string Description { get; set; } = "";

    public EditAuctionVm FromAuction(Auction auction)
    {
        EditAuctionVm vm = new EditAuctionVm();
        vm.Id = auction.Id;
        vm.Name = auction.Name;
        vm.Description = auction.Description;
        return vm;
    }
}