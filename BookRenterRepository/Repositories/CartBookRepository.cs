using BookRenterData.Context;
using BookRenterData.Entities;
using BookRenterData.Repositories.Base;
using BookRenterData.Repositories.Base.Interfaces;
using BookRenterRepository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookRenterData.Repositories
{
    public class CartBookRepository : BaseRepository<CartBook>, ICartBookRepository
    {
        private readonly BookRenterContext _dbContext;

        public CartBookRepository(BookRenterContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<CartBook>> GetByUserIdAsync(int userId)
        {
            return await _dbContext.CartBooks
                .Where(cb => cb.UserId == userId)
                .ToListAsync();
        }

        public async Task<CartBook> GetByBookIdAndUserIdAsync(int bookId, int userId)
        {
            return await _dbContext.CartBooks
               .FirstOrDefaultAsync(cb => cb.BookId == bookId && cb.UserId == userId);            
           
        }

        public async Task<IEnumerable<CartBook>> GetCartBooksByUserIdAsync(int userId)
        {
            return await _dbContext.CartBooks
                .Where(cb => cb.UserId == userId)
                .ToListAsync();
        }


        public async Task<int> GetCartItemCountAsync(int userId)
        {
            return await _dbContext.CartBooks
                .CountAsync(cb => cb.UserId == userId);
        }
    }

}
