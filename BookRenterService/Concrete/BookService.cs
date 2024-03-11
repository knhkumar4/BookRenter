using BookRenter.Models.Responses;
using BookRenter.Services.Interfaces;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Models;

namespace BookRenter.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<BookResponse> GetBookResponseByIdAsync(int id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            return book; // Assumes implicit conversion from Book to BookResponse is defined
        }

        public async Task<IEnumerable<BookResponse>> GetAllBookResponsesAsync()
        {
            var books = await _unitOfWork.BookRepository.GetAllAsync();
            return books.Select(book => (BookResponse)book); // Assumes explicit conversion from Book to BookResponse is defined
        }

        public async Task<BookResponse> AddBookAsync(BookRequest bookRequest)
        {
            if (bookRequest == null)
            {
                throw new ArgumentNullException(nameof(bookRequest));
            }

            var book = bookRequest; // Assumes implicit conversion from BookRequest to Book entity is defined

            var addedBook = await _unitOfWork.BookRepository.AddAsync(book);

            try
            {
                await _unitOfWork.CompleteAsync();
                return addedBook; // Assumes implicit conversion from Book to BookResponse is defined
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the book.", ex);
            }
        }

        public async Task<BookResponse> UpdateBookAsync(int bookId, BookRequest bookRequest)
        {
            if (bookRequest == null)
            {
                throw new ArgumentNullException(nameof(bookRequest));
            }

            var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
            if (existingBook == null)
            {
                throw new ArgumentException($"Book with ID {bookId} not found.", nameof(bookId));
            }

            // Update the book details
            existingBook.Title = bookRequest.Title;
            existingBook.Description = bookRequest.Description;
            existingBook.Author = bookRequest.Author;
            existingBook.Genre = bookRequest.Genre;
            existingBook.Price = bookRequest.Price;
            existingBook.RentPrice = bookRequest.RentPrice;

            var updatedBook = await _unitOfWork.BookRepository.UpdateAsync(existingBook);
            await _unitOfWork.CompleteAsync();
            return updatedBook; // Assumes implicit conversion from Book to BookResponse is defined
        }

        public async Task DeleteBookResponseAsync(int id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (book == null)
            {
                throw new ArgumentException($"Book with ID {id} not found.", nameof(id));
            }

            await _unitOfWork.BookRepository.DeleteAsync(book);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<SearchBookResponse>> SearchBooksAsync(string searchTerm)
        {
            var books = await _unitOfWork.BookRepository.GetManyAsync(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm));
            return books.Select(book => (SearchBookResponse)book); // Assumes explicit conversion from Book to SearchBookResponse is defined
        }
    }
}
