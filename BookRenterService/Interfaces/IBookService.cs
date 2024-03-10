using BookRenter.Models.Responses;
using BookRenterService.Models;

namespace BookRenter.Services.Interfaces
{
    public interface IBookService
    {
        Task<BookResponse> AddBookAsync(BookRequest bookResponse);
        Task DeleteBookResponseAsync(int id);
        Task<IEnumerable<BookResponse>> GetAllBookResponsesAsync();
        Task<BookResponse> GetBookResponseByIdAsync(int id);
        Task<IEnumerable<SearchBookResponse>> SearchBooksAsync(string searchTerm);
        Task<BookResponse> UpdateBookAsync(int bookId, BookRequest bookResponse);
    }
}
