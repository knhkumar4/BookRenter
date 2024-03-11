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
        public async Task<Inventory> GetInventoryWithBookByBookIdAsync(int bookId)
        {
            return await _dbContext.Inventories
                .Include(inv => inv.Book) // Include the related Book entity
                .FirstOrDefaultAsync(i => i.BookId == bookId);
        }
        public async Task<IEnumerable<Inventory>> GetAllInventoriesWithBooksAsync()
        {
            return await _dbContext.Inventories
                .Include(inv => inv.Book) // Include the related Book entities
                .ToListAsync();
        }

        public async Task<Inventory> GetInventoryByBookIdAsync(int bookId)
        {
            try
            {
                return await _dbContext.Inventories.FirstOrDefaultAsync(i => i.BookId == bookId);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }


    }
}