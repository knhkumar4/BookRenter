using BookRenter.Controllers;
using BookRenter.Models.Responses;
using BookRenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRenterAPI.Test
{
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _controller = new BookController(_mockBookService.Object);
        }

        [Fact]
        public async Task GetBookById_BookExists_ReturnsOkResult()
        {
            // Arrange
            var bookId = 1;
            var bookResponse = new BookResponse { BookId = bookId, Title = "Test Book", Author = "Test Author" };
            _mockBookService.Setup(service => service.GetBookResponseByIdAsync(bookId))
                .ReturnsAsync(bookResponse);

            // Act
            var result = await _controller.GetBookById(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBook = Assert.IsType<BookResponse>(okResult.Value);
            Assert.Equal(bookId, returnBook.BookId);
        }
        [Fact]
        public async Task GetBookById_BookDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockBookService.Setup(service => service.GetBookResponseByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((BookResponse)null);

            // Act
            var result = await _controller.GetBookById(999); // assuming 999 does not exist

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsAllBooks()
        {
            // Arrange
            var bookResponses = new List<BookResponse>
    {
        new BookResponse { BookId = 1, Title = "Test Book 1", Author = "Test Author 1" },
        new BookResponse { BookId = 2, Title = "Test Book 2", Author = "Test Author 2" }
    };
            _mockBookService.Setup(service => service.GetAllBookResponsesAsync())
                .ReturnsAsync(bookResponses);

            // Act
            var result = await _controller.GetAllBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBooks = Assert.IsType<List<BookResponse>>(okResult.Value);
            Assert.Equal(2, returnedBooks.Count);
        }

        [Fact]
        public async Task AddBook_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var bookRequest = new BookRequest { Title = "New Book", Author = "New Author" };
            var bookResponse = new BookResponse { BookId = 3, Title = "New Book", Author = "New Author" };
            _mockBookService.Setup(service => service.AddBookAsync(bookRequest))
                .ReturnsAsync(bookResponse);

            // Act
            var result = await _controller.AddBook(bookRequest);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedBook = Assert.IsType<BookResponse>(createdAtActionResult.Value);
            Assert.Equal(bookResponse.Title, returnedBook.Title);
        }


        [Fact]
        public async Task UpdateBook_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var bookRequest = new BookRequest { BookId = 1, Title = "Updated Book", Author = "Updated Author" };

            // Act
            var result = await _controller.UpdateBook(2, bookRequest); // Simulating an ID mismatch

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

    }

}
