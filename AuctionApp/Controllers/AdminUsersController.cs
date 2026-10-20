using AuctionApp.Data.Identity;
using AuctionApp.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApp.Controllers;

// User administration. Only users in the Admin role get in.
[Authorize(Roles = IdentitySeeder.AdminRole)]
public class AdminUsersController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public AdminUsersController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<IActionResult> Index()
    {
       
        IList<IdentityUser> admins = await _userManager.GetUsersInRoleAsync(IdentitySeeder.AdminRole);
        HashSet<string> adminIds = admins.Select(a => a.Id).ToHashSet();
        
        var users = _userManager.Users
            .OrderBy(u => u.UserName)
            .Select(u => new { u.Id, u.UserName, u.Email })
            .ToList();

        List<AdminUserVm> vms = new List<AdminUserVm>();
        foreach (var u in users)
        {
            vms.Add(new AdminUserVm
            {
                Id = u.Id,
                UserName = u.UserName ?? "",
                Email = u.Email ?? "",
                IsAdmin = adminIds.Contains(u.Id)
            });
        }
        return View(vms);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        IdentityUser? user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        if (await _userManager.IsInRoleAsync(user, IdentitySeeder.AdminRole))
        {
            TempData["Error"] = "An admin account cannot be deleted.";
            return RedirectToAction(nameof(Index));
        }

        IdentityResult result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            TempData["Message"] = $"User {user.UserName} was deleted.";
        }
        else
        {
            TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
        }
        return RedirectToAction(nameof(Index));
    }
}