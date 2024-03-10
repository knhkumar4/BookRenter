using BookRenterData.Entities;
using BookRenterData.Repositories.Base.Interfaces;

namespace BookRenterRepository.Repositories.Interfaces
{
    public interface IInventoryRepository : IBaseRepository<Inventory>
    {
        Task<Inventory> GetInventoryByBookIdAsync(int bookId);
    }
}