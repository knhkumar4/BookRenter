
using System.Linq.Expressions;
using BookRenterData.Entities;

namespace BookRenter.Services.Interfaces
{
    public interface IBookService
    {
        Task<Book> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> AddBookAsync(Book book);
        Task DeleteBookAsync(Book book);
        Task DeleteBooksAsync(Expression<Func<Book, bool>> filter);
        Task<IEnumerable<Book>> GetBooksAsync(Expression<Func<Book, bool>> filter = null,
                                               Func<IQueryable<Book>, IOrderedQueryable<Book>> orderBy = null,
                                               int? top = null,
                                               int? skip = null,
                                               params string[] includeProperties);
    }
}
