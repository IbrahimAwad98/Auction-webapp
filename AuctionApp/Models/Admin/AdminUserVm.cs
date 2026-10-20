namespace AuctionApp.Models.Admin;

// One row in the admin's user list.
public class AdminUserVm
{
    public string Id { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsAdmin { get; set; }
}