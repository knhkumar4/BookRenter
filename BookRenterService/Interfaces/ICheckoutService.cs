namespace BookRenterService.Interfaces
{
    public interface ICheckoutService
    {     
        Task<string> CheckoutBooksAsync();
    }
}