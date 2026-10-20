using Microsoft.EntityFrameworkCore;
namespace AuctionApp.Data.Repositories;

// The only class that talks to DbSet<T>. Works for every entity type.
internal class Repository<T> : IRepository<T> where T: class
{
    private readonly AuctionDbContext _context;
    private readonly DbSet<T> _set;

    public Repository(AuctionDbContext context)
    {
        _context = context;
        _set = context.Set<T>(); // DbSet<AuctionDb> or DbSet<BidDb>, depending on T
    }

    public IQueryable<T> Query()
    {
        return _set;
    }

    public T? GetById(int id)
    {
        return _set.Find(id);
    }

    public void Add(T entity)
    {
        _set.Add(entity);
    }

    public void Remove(T entity)
    {
        _set.Remove(entity);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}