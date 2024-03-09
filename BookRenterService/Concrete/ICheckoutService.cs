
namespace BookRenter.Services
{
    public interface ICheckoutService
    {
        Task<bool> AddBookToCartAsync(int bookId, int cartId);
    }
}