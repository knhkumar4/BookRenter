using BookRenterData.Entities;
using BookRenterData.Repositories.Base.Interfaces;

namespace BookRenterRepository.Repositories.Interfaces
{
    public interface IInventoryRepository : IBaseRepository<Inventory>
    {
        Task<IEnumerable<Inventory>> GetAllInventoriesWithBooksAsync();
        Task<Inventory> GetInventoryByBookIdAsync(int bookId);
        Task<Inventory> GetInventoryWithBookByBookIdAsync(int bookId);
    }
}