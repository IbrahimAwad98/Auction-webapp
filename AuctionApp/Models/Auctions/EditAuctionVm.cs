using System.ComponentModel.DataAnnotations;
using AuctionApp.Core;

namespace AuctionApp.Models.Auctions;

// Own VM for editing because only the description may change once the auction exists.
public class EditAuctionVm
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    [StringLength(2000)]
    public string Description { get; set; } = "";

    public static EditAuctionVm FromAuction(Auction auction)
    {
        return new EditAuctionVm
        {
            Id = auction.Id,
            Name = auction.Name,
            Description = auction.Description
        };
    }
}
