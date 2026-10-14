using AuctionApp.Core;
using AuctionApp.Core.Interfaces;
using AuctionApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Two databases: one for the auctions, one for Identity. Passwords come from user-secrets.
builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("AuctionDbConnection")
                     ?? throw new InvalidOperationException("AuctionDbConnection is missing.")));
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("IdentityDbConnection")
                     ?? throw new InvalidOperationException("IdentityDbConnection is missing.")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppIdentityDbContext>();

// Dependency injection: which implementation is used for each interface.
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IAuctionPersistence, AuctionPersistence>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
