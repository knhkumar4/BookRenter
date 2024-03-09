using BookRenterData.Entities;
using BookRenterData.Repositories.Base.Interfaces;

namespace BookRenterRepository.Repositories.Interfaces
{
    public interface ICartBookRepository : IBaseRepository<CartBook>
    {
        Task<CartBook> GetByBookIdAndCartIdAsync(int bookId, int cartId);
        Task<int> GetCartItemCountAsync(int cartId);
    }
}