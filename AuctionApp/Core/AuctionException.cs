namespace AuctionApp.Core;

// Own type so the controller can tell expected rule violations (shown to the user) from real errors.
public class AuctionException : Exception
{
    public AuctionException(string message) : base(message)
    {
    }
}
