using BookRenter.Controllers;
using BookRenter.Models.Responses;
using BookRenter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookRenterAPI.Test
{
    public class BookControllerTests
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _controller = new BookController(_bookServiceMock.Object);
        }

        [Fact]
        public async Task GetBookById_ExistingBookId_ReturnsOk()
        {
            // Arrange
            var bookId = 1;
            var expectedBookResponse = new BookResponse
            {
                BookId = 1,
                Title = "Sample Book",
                Author = "John Doe",
                Description = "A sample book description",
                Genre = "Fiction",
                Price = 29.99,
                RentPrice = 10.99
            };

            _bookServiceMock.Setup(x => x.GetBookResponseByIdAsync(bookId))
                           .ReturnsAsync(expectedBookResponse);

            // Act
            var result = await _controller.GetBookById(bookId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<BookResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var bookResponse = Assert.IsType<BookResponse>(okResult.Value);

            Assert.Equal(expectedBookResponse.BookId, bookResponse.BookId);
            Assert.Equal(expectedBookResponse.Title, bookResponse.Title);
            Assert.Equal(expectedBookResponse.Author, bookResponse.Author);
            Assert.Equal(expectedBookResponse.Description, bookResponse.Description);
            Assert.Equal(expectedBookResponse.Genre, bookResponse.Genre);
            Assert.Equal(expectedBookResponse.Price, bookResponse.Price);
            Assert.Equal(expectedBookResponse.RentPrice, bookResponse.RentPrice);
        }

        [Fact]
        public async Task GetBookById_NonExistingBookId_ReturnsNotFound()
        {
            // Arrange
            var bookId = 2;

            _bookServiceMock.Setup(x => x.GetBookResponseByIdAsync(bookId))
                           .ReturnsAsync((BookResponse)null);

            // Act
            var result = await _controller.GetBookById(bookId);

            // Assert
            Assert.IsType<ActionResult<BookResponse>>(result);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOk()
        {
            // Arrange
            var expectedBookResponses = new List<BookResponse>
            {
                new BookResponse { BookId = 1, Title = "Book 1", Author = "Author 1" },
                new BookResponse { BookId = 2, Title = "Book 2", Author = "Author 2" }
            };

            _bookServiceMock.Setup(x => x.GetAllBookResponsesAsync())
                           .ReturnsAsync(expectedBookResponses);

            // Act
            var result = await _controller.GetAllBooks();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<BookResponse>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var bookResponses = Assert.IsType<List<BookResponse>>(okResult.Value);

            Assert.Equal(expectedBookResponses.Count, bookResponses.Count);
            for (int i = 0; i < expectedBookResponses.Count; i++)
            {
                Assert.Equal(expectedBookResponses[i].BookId, bookResponses[i].BookId);
                Assert.Equal(expectedBookResponses[i].Title, bookResponses[i].Title);
                Assert.Equal(expectedBookResponses[i].Author, bookResponses[i].Author);
            }
        }

        [Fact]
        public async Task AddBook_ValidBookRequest_ReturnsCreatedAtAction()
        {
            // Arrange
            var bookRequest = new BookRequest
            {
                Title = "New Book",
                Author = "Jane Doe",
                Description = "A new book description",
                Genre = "Non-Fiction",
                Price = 19.99,
                RentPrice = 7.99
            };

            var expectedBookResponse = new BookResponse
            {
                BookId = 3,
                Title = "New Book",
                Author = "Jane Doe",
                Description = "A new book description",
                Genre = "Non-Fiction",
                Price = 19.99,
                RentPrice = 7.99
            };

            _bookServiceMock.Setup(x => x.AddBookAsync(bookRequest))
                           .ReturnsAsync(expectedBookResponse);

            // Act
            var result = await _controller.AddBook(bookRequest);

            // Assert
            var actionResult = Assert.IsType<ActionResult<BookResponse>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var bookResponse = Assert.IsType<BookResponse>(createdAtActionResult.Value);

            Assert.Equal(expectedBookResponse.BookId, bookResponse.BookId);
            Assert.Equal(expectedBookResponse.Title, bookResponse.Title);
            Assert.Equal(expectedBookResponse.Author, bookResponse.Author);
            Assert.Equal(expectedBookResponse.Description, bookResponse.Description);
            Assert.Equal(expectedBookResponse.Genre, bookResponse.Genre);
            Assert.Equal(expectedBookResponse.Price, bookResponse.Price);
            Assert.Equal(expectedBookResponse.RentPrice, bookResponse.RentPrice);
        }

        [Fact]
        public async Task AddBook_InvalidBookRequest_ReturnsBadRequest()
        {
            // Arrange
            var bookRequest = new BookRequest(); // Invalid request

            // Act
            var result = await _controller.AddBook(bookRequest);

            // Assert
            Assert.IsType<ActionResult<BookResponse>>(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateBook_ValidBookRequest_ReturnsOk()
        {
            // Arrange
            var bookId = 1;
            var bookRequest = new BookRequest
            {
                BookId = 1,
                Title = "Updated Book",
                Author = "Updated Author",
                Description = "Updated book description",
                Genre = "Updated Genre",
                Price = 25.99,
                RentPrice = 9.99
            };

            var expectedBookResponse = new BookResponse
            {
                BookId = 1,
                Title = "Updated Book",
                Author = "Updated Author",
                Description = "Updated book description",
                Genre = "Updated Genre",
                Price = 25.99,
                RentPrice = 9.99
            };

            _bookServiceMock.Setup(x => x.UpdateBookAsync(bookId, bookRequest))
                           .ReturnsAsync(expectedBookResponse);

            // Act
            var result = await _controller.UpdateBook(bookId, bookRequest);

            // Assert
            var actionResult = Assert.IsType<ActionResult<BookResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var updatedBookResponse = Assert.IsType<BookResponse>(okResult.Value);

            Assert.Equal(expectedBookResponse.BookId, updatedBookResponse.BookId);
            Assert.Equal(expectedBookResponse.Title, updatedBookResponse.Title);
            Assert.Equal(expectedBookResponse.Author, updatedBookResponse.Author);
            Assert.Equal(expectedBookResponse.Description, updatedBookResponse.Description);
            Assert.Equal(expectedBookResponse.Genre, updatedBookResponse.Genre);
            Assert.Equal(expectedBookResponse.Price, updatedBookResponse.Price);
            Assert.Equal(expectedBookResponse.RentPrice, updatedBookResponse.RentPrice);
        }

        [Fact]
        public async Task UpdateBook_InvalidBookRequest_ReturnsBadRequest()
        {
            // Arrange
            var bookId = 1;
            var bookRequest = new BookRequest(); // Invalid request

            // Act
            var result = await _controller.UpdateBook(bookId, bookRequest);

            // Assert
            Assert.IsType<ActionResult<BookResponse>>(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateBook_InvalidBookId_ReturnsBadRequest()
        {
            // Arrange
            var bookId = 1;
            var bookRequest = new BookRequest
            {
                BookId = 2, // Mismatched ID
                Title = "Updated Book",
                Author = "Updated Author",
                Description = "Updated book description",
                Genre = "Updated Genre",
                Price = 25.99,
                RentPrice = 9.99
            };

            // Act
            var result = await _controller.UpdateBook(bookId, bookRequest);

            // Assert
            Assert.IsType<ActionResult<BookResponse>>(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateBook_ErrorInService_ReturnsInternalServerError()
        {
            // Arrange
            var bookId = 1;
            var bookRequest = new BookRequest
            {
                BookId = 1,
                Title = "Updated Book",
                Author = "Updated Author",
                Description = "Updated book description",
                Genre = "Updated Genre",
                Price = 25.99,
                RentPrice = 9.99
            };

            _bookServiceMock.Setup(x => x.UpdateBookAsync(bookId, bookRequest))
                           .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.UpdateBook(bookId, bookRequest);

            // Assert
            var actionResult = Assert.IsType<ActionResult<BookResponse>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);

            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while updating the book.", statusCodeResult.Value);
        }
    }
}
