namespace AuctionApp.Data.Repositories;

// Generic repository: one contract that works for any EF entity. 
// Internal to the data layer: only AuctionPersistence uses it.
internal interface IRepository<T> where T: class
{
    IQueryable<T> Query();
    T? GetById(int id);
    void Add(T entity);
    void Remove(T entity);
    void SaveChanges();
}