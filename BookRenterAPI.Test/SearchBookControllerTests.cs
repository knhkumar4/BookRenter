using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookRenter.Controllers;
using BookRenter.Models.Responses;
using BookRenter.Services.Interfaces;
using BookRenterService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BookRenterAPI.Test
{
    public class SearchBookControllerTests
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly SearchBookController _controller;

        public SearchBookControllerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _controller = new SearchBookController(_bookServiceMock.Object);
        }

        [Fact]
        public async Task SearchBooksAsync_ValidSearchTerm_ReturnsOk()
        {
            // Arrange
            var searchTerm = "Harry Potter";
            var expectedSearchResults = new List<SearchBookResponse>
            {
                new SearchBookResponse { BookId = 1, Title = "Harry Potter and the Sorcerer's Stone", Author = "J.K. Rowling" },
                new SearchBookResponse { BookId = 2, Title = "Harry Potter and the Chamber of Secrets", Author = "J.K. Rowling" }
            };

            _bookServiceMock.Setup(x => x.SearchBooksAsync(searchTerm))
                           .ReturnsAsync(expectedSearchResults);

            // Act
            var result = await _controller.SearchBooksAsync(searchTerm);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<SearchBookResponse>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var searchResults = Assert.IsType<List<SearchBookResponse>>(okResult.Value);

            Assert.Equal(expectedSearchResults.Count, searchResults.Count);
            for (int i = 0; i < expectedSearchResults.Count; i++)
            {
                Assert.Equal(expectedSearchResults[i].BookId, searchResults[i].BookId);
                Assert.Equal(expectedSearchResults[i].Title, searchResults[i].Title);
                Assert.Equal(expectedSearchResults[i].Author, searchResults[i].Author);
            }
        }

        [Fact]
        public async Task SearchBooksAsync_EmptySearchTerm_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.SearchBooksAsync(null);

            // Assert
            Assert.IsType<ActionResult<IEnumerable<SearchBookResponse>>>(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task SearchBooksAsync_ErrorInService_ReturnsInternalServerError()
        {
            // Arrange
            var searchTerm = "Error";
            _bookServiceMock.Setup(x => x.SearchBooksAsync(searchTerm))
                           .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.SearchBooksAsync(searchTerm);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<SearchBookResponse>>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);

            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while searching for books.", statusCodeResult.Value);
        }
    }
}
