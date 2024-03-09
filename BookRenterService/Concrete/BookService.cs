using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Folder.BookRenter.Models.Responses;

namespace BookRenter.Services
{
    public class BookResponseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookResponseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<BookResponse> GetBookResponseByIdAsync(int id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            return book; // Implicit conversion from Book to BookResponse
        }

        public async Task<IEnumerable<BookResponse>> GetAllBookResponsesAsync()
        {
            var books = await _unitOfWork.BookRepository.GetAllAsync();
            return books.Select(book => (BookResponse)book); // Explicit conversion of each Book to BookResponse
        }

        public async Task<BookResponse> AddBookResponseAsync(BookResponse bookResponse)
        {
            if (bookResponse == null)
            {
                throw new ArgumentNullException(nameof(bookResponse));
            }

            var book = bookResponse; // Implicit conversion from BookResponse to Book
            var addedBook = await _unitOfWork.BookRepository.AddAsync(book);
            return addedBook; // Implicit conversion from Book to BookResponse
        }

        public async Task DeleteBookResponseAsync(int id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id);
            if (book == null)
            {
                throw new ArgumentException($"Book with ID {id} not found.", nameof(id));
            }

            await _unitOfWork.BookRepository.DeleteAsync(book);
        }

        // Add other methods as needed
    }
}
