using Microsoft.AspNetCore.Mvc;
using AuctionApp.Core;
using AuctionApp.Core.Interfaces;
using AuctionApp.Models.Auctions;

namespace AuctionApp.Controllers;

public class AuctionsController : Controller
{
    private readonly IAuctionService _auctionService;

    public AuctionsController(IAuctionService auctionService)
    {
        _auctionService = auctionService;
    }

    // GET: /Auctions
    public IActionResult Index()
    {
        List<Auction> auctions = _auctionService.ListActiveAuctions();

        List<AuctionVm> auctionVms = new List<AuctionVm>();
        foreach (Auction auction in auctions)
        {
            auctionVms.Add(AuctionVm.FromAuction(auction));
        }

        return View(auctionVms);
    }
}
