using AuctionApp.Core;
using AuctionApp.Core.Interfaces;
using AuctionApp.Models.Auctions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApp.Controllers;

// Every action requires login because every page is tied to the current user.
[Authorize]
public class AuctionsController : Controller
{
    // Depends on the interface so the service can be swapped in DI without touching the controller.
    private readonly IAuctionService auctionService;

    public AuctionsController(IAuctionService auctionService)
    {
        this.auctionService = auctionService;
    }

    private string CurrentUserName()
    {
        return User.Identity?.Name
               ?? throw new InvalidOperationException("User is not authenticated.");
    }

    // The views get view models, not domain objects, so they only see what they display.
    private ViewResult ListView(List<Auction> auctions)
    {
        return View(auctions.Select(AuctionVm.FromAuction).ToList());
    }

    public IActionResult Index()
    {
        return ListView(auctionService.ListActiveAuctions());
    }

    public IActionResult MyBids()
    {
        return ListView(auctionService.ListActiveAuctionsWithBidBy(CurrentUserName()));
    }

    public IActionResult Won()
    {
        return ListView(auctionService.ListAuctionsWonBy(CurrentUserName()));
    }

    public IActionResult Details(int id)
    {
        try
        {
            Auction auction = auctionService.GetAuctionDetails(id);
            return View(AuctionDetailsVm.FromAuction(auction, CurrentUserName()));
        }
        catch (AuctionException)
        {
            return NotFound();
        }
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateAuctionVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        try
        {
            auctionService.CreateAuction(vm.Name, vm.Description, vm.StartingPrice, vm.EndTime, CurrentUserName());
            return RedirectToAction(nameof(Index));
        }
        catch (AuctionException e)
        {
            ViewData["Error"] = e.Message;
            return View(vm);
        }
    }

    public IActionResult Edit(int id)
    {
        try
        {
            Auction auction = auctionService.GetAuctionForOwner(id, CurrentUserName());
            return View(EditAuctionVm.FromAuction(auction));
        }
        catch (AuctionException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, EditAuctionVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        try
        {
            auctionService.UpdateDescription(id, vm.Description, CurrentUserName());
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (AuctionException)
        {
            return NotFound();
        }
    }

    // Rule violations are shown on the details page via TempData, since we redirect back to it.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceBid(PlaceBidVm vm)
    {
        try
        {
            auctionService.PlaceBid(vm.AuctionId, vm.Amount, CurrentUserName());
        }
        catch (AuctionException e)
        {
            TempData["Error"] = e.Message;
        }
        return RedirectToAction(nameof(Details), new { id = vm.AuctionId });
    }
}
