using BookRenterData.Entity;
using BookRenterData.Repositories.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookRenterService.Concrete
{
    public class BookService
    {
        private readonly IBaseRepository<Book> _bookRepository;

        public BookService(IBaseRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            return await _bookRepository.AddAsync(book);
        }

        public async Task DeleteBookAsync(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            await _bookRepository.DeleteAsync(book);
        }

        public async Task DeleteBooksAsync(Expression<Func<Book, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            await _bookRepository.DeleteManyAsync(filter);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(Expression<Func<Book, bool>> filter = null,
                                                            Func<IQueryable<Book>, IOrderedQueryable<Book>> orderBy = null,
                                                            int? top = null,
                                                            int? skip = null,
                                                            params string[] includeProperties)
        {
            return await _bookRepository.GetManyAsync(filter, orderBy, top, skip, includeProperties);
        }
    }
}
