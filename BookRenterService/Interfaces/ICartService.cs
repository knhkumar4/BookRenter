using BookRenterService.Models;

namespace BookRenterService.Interfaces
{
    public interface ICartService
    {
        Task<(bool Success, string Message)> AddBookToCartAsync(int bookId);
        Task<IEnumerable<CartBookResponse>> GetAllCartItemsAsync();
        Task<bool> RemoveBookFromCartAsync(int bookId);
    }
}