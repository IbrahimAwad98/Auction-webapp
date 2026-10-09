namespace AuctionApp.Core;
// Oföränderlig eftersom ett lagt bud aldrig får ändras i efterhand.
public class Bid
{
    public int Id { get; }
    public string UserName { get; }
    public decimal Amount { get; }
    public DateTime PlacedAt { get; }

    public Bid(string userName, decimal amount, DateTime placedAt)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("A bid must have a bidder.", nameof(userName));
        }
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "A bid must be greater than zero.");
        }
        UserName = userName;
        Amount = amount;
        PlacedAt = placedAt;
    }

    // Bud från databasen som redan har ett id
    public Bid(int id, string userName, decimal amount, DateTime placedAt)
        : this(userName, amount, placedAt)
    {
        Id = id;
    }

    public override string ToString()
    {
        return $"{Id}: {UserName}, amount = {Amount}, time: {PlacedAt}";
    }
}