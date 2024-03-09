
using System.Linq.Expressions;
using BookRenterData.Entities;
using BookRenterService.Folder.BookRenter.Models.Responses;

namespace BookRenter.Services.Interfaces
{
    public interface IBookService
    {
        Task<BookResponse> AddBookResponseAsync(BookResponse bookResponse);
        Task DeleteBookResponseAsync(int id);
        Task<IEnumerable<BookResponse>> GetAllBookResponsesAsync();
        Task<BookResponse> GetBookResponseByIdAsync(int id);
    }
}
