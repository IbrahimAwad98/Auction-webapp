using Microsoft.AspNetCore.Mvc;
using AuctionApp.Core;
using AuctionApp.Core.Interfaces;
using AuctionApp.Models.Auctions;

namespace AuctionApp.Controllers;

public class AuctionsController : Controller
{
    // Interface istället för konkret klass så mocken kan bytas mot riktig service i DI utan att röra controllern.
    private readonly IAuctionService _auctionService;

    public AuctionsController(IAuctionService auctionService)
    {
        _auctionService = auctionService;
    }

    private string CurrentUserName()
    {
        return "iawad@kth.se";
    }

    // GET: /Auctions
    public IActionResult Index()
    {
        List<Auction> auctions = _auctionService.ListActiveAuctions();

        // Vm mindre risk för over posting
        List<AuctionVm> auctionVms = new List<AuctionVm>();
        foreach (Auction auction in auctions)
        {
            auctionVms.Add(AuctionVm.FromAuction(auction));
        }
        return View(auctionVms);
    }

    // GET: /Auctions/MyBids
    public IActionResult MyBids()
    {
        List<Auction> auctions = _auctionService.ListActiveAuctionsWithBidBy(CurrentUserName());
        List<AuctionVm> auctionVms = new List<AuctionVm>();
        foreach (Auction auction in auctions)
        {
            auctionVms.Add(AuctionVm.FromAuction(auction));
        }

        return View(auctionVms);
    }

    public IActionResult Won()
    {
        List<Auction> auctions = _auctionService.ListAuctionsWonBy(CurrentUserName());
        List<AuctionVm> auctionVms = new List<AuctionVm>();
        foreach (Auction auction in auctions)
        {
            auctionVms.Add(AuctionVm.FromAuction(auction));
        }

        return View(auctionVms);
    }

    public IActionResult Details(int Id)
    {
        try
        {
            Auction auction = _auctionService.GetAuctionDetails(Id);
            AuctionDetailsVm auctionDetailsVm = AuctionDetailsVm.FromAuction(auction, CurrentUserName());
            return View(auctionDetailsVm);
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
            _auctionService.CreateAuction(vm.Name, vm.Description, vm.StartingPrice,
                vm.EndTime, CurrentUserName());
            return Redirect("/Auctions");
        }
        catch (AuctionException e)
        {
            ViewData["Error"] = e.Message;
            return View(vm);
        }
    }
    
    public IActionResult Edit(int Id)
    {
        try
        {
            Auction auction = _auctionService.GetAuctionForOwner(Id, CurrentUserName());
            return View(EditAuctionVm.FromAuction(auction));
        }
        catch (AuctionException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int Id, EditAuctionVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        
        try
        {
            _auctionService.UpdateDescription(Id,vm.Description,CurrentUserName());
            return Redirect("/Auctions/Details/" + Id);
        }
        catch (AuctionException)
        {
            return NotFound();
        }
    }
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceBid(PlaceBidVm vm)
    {
        try
        {
            _auctionService.PlaceBid(vm.AuctionId, vm.Amount, CurrentUserName());
        }
        catch (AuctionException e)
        {
            TempData["Error"] = e.Message;
        }

        return Redirect("/Auctions/Details/" + vm.AuctionId);
    }
    
}
