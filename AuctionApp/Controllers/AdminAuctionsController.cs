using AuctionApp.Core;
using AuctionApp.Core.Interfaces;
using AuctionApp.Data.Identity;
using AuctionApp.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApp.Controllers;

// Auction administration. Talks only to IAuctionService, same as AuctionsController.
[Authorize(Roles = IdentitySeeder.AdminRole)]
public class AdminAuctionsController : Controller
{
    private readonly IAuctionService _service;

    public AdminAuctionsController(IAuctionService service)
    {
        _service = service;
    }
    
    public IActionResult ByUser(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return RedirectToAction("Index", "AdminUsers");
        }

        DateTime now = DateTime.Now;
        List<Auction> auctions = _service.ListAuctionsOwnedBy(userName);

        AdminUserAuctionsVm vm = new AdminUserAuctionsVm
        {
            UserName = userName,
            Auctions = auctions.Select(a => AdminAuctionVm.FromAuction(a, now)).ToList()
        };
        return View(vm);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id, string userName)
    {
        try
        {
            _service.DeleteAuction(id);
            TempData["Message"] = "The auction was deleted.";
        }
        catch (AuctionException ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(ByUser), new { userName });
    }
}