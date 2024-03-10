namespace BookRenterService.Interfaces
{
    public interface ICheckoutService
    {
        Task<bool> AddBookToCartAsync(int bookId);
        Task<string> CheckoutBooksAsync();
    }
}