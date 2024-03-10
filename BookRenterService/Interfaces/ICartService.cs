using BookRenterService.Models;

namespace BookRenterService.Interfaces
{
    public interface ICartService
    {
        Task<bool> AddBookToCartAsync(int bookId);
        Task<IEnumerable<CartBookResponse>> GetAllCartItemsAsync();
        Task<bool> RemoveBookFromCartAsync(int bookId);
    }
}