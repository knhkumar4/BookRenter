using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookRenter.Services.Interfaces;
using BookRenterData.Entities;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Folder.BookRenter.Models.Responses;

namespace BookRenter.Services
{
    public class BookService:IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
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

            // Convert BookResponse to Book entity
            var book = bookResponse;

            // Add the book to the database
            var addedBook = await _unitOfWork.BookRepository.AddAsync(book);

            //// Create inventory for the added book
            //var inventory = new Inventory
            //{
            //    BookId = addedBook.BookId,
            //    Quantity = bookResponse.Quantity, // Set the quantity provided by the user
            //    CreatedDate = DateTime.UtcNow
            //};

            // Add the inventory to the database
          //  await _unitOfWork.InventoryRepository.AddAsync(inventory);

            try
            {
                // Save changes to the database
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("An error occurred while adding the book and inventory.", ex);
            }

            return addedBook; // Implicit conversion from Book to BookResponse
        }

        public async Task<BookResponse> UpdateBookResponseAsync(int bookId, BookResponse bookResponse)
        {
            try
            {
                if (bookResponse == null)
            {
                throw new ArgumentNullException(nameof(bookResponse));
            }

            // Get the book from the database by its ID
            var existingBook = await _unitOfWork.BookRepository.GetByIdAsync(bookId);
            if (existingBook == null)
            {
                throw new ArgumentException($"Book with ID {bookId} not found.", nameof(bookId));
            }

            // Update the book details
            existingBook.Title = bookResponse.Title;
            existingBook.Description = bookResponse.Description;
            existingBook.Author = bookResponse.Author;
            existingBook.Genre = bookResponse.Genre;
            existingBook.Price = bookResponse.Price;
           

            // Update the book in the database
            var updatedBook = await _unitOfWork.BookRepository.UpdateAsync(existingBook);

            // Get the inventory for the book
            var inventory = await _unitOfWork.InventoryRepository.GetInventoryByBookIdAsync(bookId);
            if (inventory == null)
            {
                // If inventory does not exist, create a new one
                inventory = new Inventory
                {
                    BookId = bookId,
                    Quantity = bookResponse.Quantity, // Set the quantity provided by the user
                    CreatedDate = DateTime.UtcNow
                };
                await _unitOfWork.InventoryRepository.AddAsync(inventory);
            }
            else
            {
                // Update the quantity in the inventory
                inventory.Quantity = bookResponse.Quantity; // Set the quantity provided by the user
                await _unitOfWork.InventoryRepository.UpdateAsync(inventory);
            }

           
                // Save changes to the database
                await _unitOfWork.CompleteAsync();
                return updatedBook;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("An error occurred while updating the book and inventory.", ex);
            }

             // Implicit conversion from Book to BookResponse
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

        public async Task<IEnumerable<BookResponse>> SearchBooksAsync(string searchTerm)
        {
            // Search by book title or author
            var books = await _unitOfWork.BookRepository.GetManyAsync(
                b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm)
            );

            // Convert domain model to DTO
            var bookResponses = books.Select(book => (BookResponse)book);

            return bookResponses;
        }
        // Add other methods as needed
    }
}
