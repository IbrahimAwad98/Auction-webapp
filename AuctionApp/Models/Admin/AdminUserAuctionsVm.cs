namespace AuctionApp.Models.Admin;

// The page that lists all auctions of one user.
public class AdminUserAuctionsVm
{
    public string UserName { get; set; } = "";
    public List<AdminAuctionVm> Auctions { get; set; } = new List<AdminAuctionVm>();
}