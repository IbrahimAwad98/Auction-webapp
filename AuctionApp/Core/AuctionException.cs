namespace AuctionApp.Core;
// Egen typ så controllern kan skilja väntade regelbrott (visas för användaren) från riktiga fel.
public class AuctionException: Exception
{
    public AuctionException(string message) : base(message)
    {
    }
}