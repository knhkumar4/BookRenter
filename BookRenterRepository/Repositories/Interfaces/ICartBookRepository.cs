using BookRenterData.Entities;
using BookRenterData.Repositories.Base.Interfaces;

namespace BookRenterRepository.Repositories.Interfaces
{
    public interface ICartBookRepository : IBaseRepository<CartBook>
    {
        Task<CartBook> GetByBookIdAndUserIdAsync(int bookId, int userId);
        Task<IEnumerable<CartBook>> GetCartBooksByUserIdAsync(int userId);
        Task<int> GetCartItemCountAsync(int userId);
    }
}