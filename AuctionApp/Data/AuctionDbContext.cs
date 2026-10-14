using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Data;

public class AuctionDbContext : DbContext
{
    public DbSet<AuctionDb> Auctions { get; set; }
    public DbSet<BidDb> Bids { get; set; }

    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
    {
    }
}
