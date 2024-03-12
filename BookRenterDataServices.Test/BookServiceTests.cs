using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookRenter.Services;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Models;
using BookRenter.Models.Responses;
using BookRenterData.Entities;
namespace BookRenterDataServices.Test
{
    public class BookServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _bookService = new BookService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetBookResponseByIdAsync_ReturnsBook()
        {
            // Arrange
            var bookId = 1;
            var book = new Book { BookId = bookId, Title = "Test Book", Author = "Test Author" };

            _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(bookId))
                .ReturnsAsync(book);

            // Act
            var result = await _bookService.GetBookResponseByIdAsync(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(book.Title, result.Title);
            Assert.Equal(book.Author, result.Author);
        }

        [Fact]
        public async Task GetAllBookResponsesAsync_ReturnsAllBooks()
        {
            // Arrange
            var books = new List<Book>
        {
            new Book { BookId = 1, Title = "Test Book 1", Author = "Test Author 1" },
            new Book { BookId = 2, Title = "Test Book 2", Author = "Test Author 2" }
        };

            _mockUnitOfWork.Setup(uow => uow.BookRepository.GetAllAsync())
                .ReturnsAsync(books);

            // Act
            var results = await _bookService.GetAllBookResponsesAsync();

            // Assert
            Assert.NotNull(results);
            Assert.Equal(books.Count, results.Count());
        }

        [Fact]
        public async Task AddBookAsync_ThrowsArgumentNullExceptionForNullRequest()
        {
            // Arrange
            BookRequest request = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _bookService.AddBookAsync(request));
        }

        [Fact]
        public async Task AddBookAsync_AddsBookSuccessfully()
        {
            // Arrange
            var bookRequest = new BookRequest { Title = "New Book", Author = "New Author" };
            var book = new Book { BookId = 3, Title = bookRequest.Title, Author = bookRequest.Author };

            _mockUnitOfWork.Setup(uow => uow.BookRepository.AddAsync(It.IsAny<Book>()))
                .ReturnsAsync(book);
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _bookService.AddBookAsync(bookRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(book.Title, result.Title);
            Assert.Equal(book.Author, result.Author);
        }

        [Fact]
        public async Task UpdateBookAsync_BookNotFound_ThrowsArgumentException()
        {
            // Arrange
            int bookId = 99;
            var bookRequest = new BookRequest { Title = "Updated Title", Author = "Updated Author" };

            _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(bookId))
                .ReturnsAsync((Book)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _bookService.UpdateBookAsync(bookId, bookRequest));
        }

        [Fact]
        public async Task DeleteBookResponseAsync_BookNotFound_ThrowsArgumentException()
        {
            // Arrange
            int bookId = 99;

            _mockUnitOfWork.Setup(uow => uow.BookRepository.GetByIdAsync(bookId))
                .ReturnsAsync((Book)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _bookService.DeleteBookResponseAsync(bookId));
        }

        

    }
}