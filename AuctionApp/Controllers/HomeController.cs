using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuctionApp.Models;

namespace AuctionApp.Controllers;

// Static pages only. Based on the ASP.NET MVC template, with Privacy replaced by About.
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
