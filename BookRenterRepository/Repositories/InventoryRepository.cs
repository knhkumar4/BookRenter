using BookRenterData.Context;
using BookRenterData.Entities;
using BookRenterData.Repositories.Base;
using BookRenterData.Repositories.Interfaces;
using BookRenterRepository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace JC.Samples.AsyncRepository.Repository
{
    /// <summary>
    /// User Repository.
    /// </summary>
    public class InventoryRepository : BaseRepository<Inventory>, IInventoryRepository
    {
        private readonly BookRenterContext _dbContext;

        public InventoryRepository(BookRenterContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Inventory> GetInventoryByBookIdAsync(int bookId)
        {
            return await _dbContext.Inventories.FirstOrDefaultAsync(i => i.BookId == bookId);
        }
    }
}