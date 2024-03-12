using BookRenter.Controllers;
using BookRenter.Models.Requests;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace BookRenterAPI.Test
{
    public class CartControllerTests
    {
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly CartController _cartController;

        public CartControllerTests()
        {
            _cartServiceMock = new Mock<ICartService>();
            _cartController = new CartController(_cartServiceMock.Object);
        }

        [Fact]
        public async Task GetAllCartItemsAsync_ReturnsCartItems()
        {
            // Arrange
            var expectedCartItems = new List<CartBookResponse>
            {
                new CartBookResponse { BookId = 1, BookName = "Book 1" },
                new CartBookResponse { BookId = 2, BookName = "Book 2" }
            };
            _cartServiceMock.Setup(x => x.GetAllCartItemsAsync()).ReturnsAsync(expectedCartItems);

            // Act
            var result = await _cartController.GetAllCartItemsAsync();

            // Assert CartBookResponse
            var actionResult = Assert.IsType<ActionResult<IEnumerable<CartBookResponse>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualCartItems = Assert.IsAssignableFrom<IEnumerable<CartBookResponse>>(okResult.Value);

            Assert.Equal(expectedCartItems.Count, actualCartItems.Count());
            // Add more specific assertions based on your requirements
        }

        [Fact]
        public async Task AddToCart_ValidRequest_Returns201Created()
        {
            // Arrange
            _cartServiceMock.Setup(x => x.AddBookToCartAsync(It.IsAny<int>())).ReturnsAsync((true, "Book added to cart successfully."));

            var addToCartRequest = new AddToCartRequest { BookId = 1 };

            // Act
            var result = await _cartController.AddToCart(addToCartRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);
            Assert.True((bool)((ApiResponse<string>)result.Value).Success);
        }

        [Fact]
        public async Task AddToCart_BookAlreadyInCart_Returns400BadRequest()
        {
            // Arrange
            _cartServiceMock.Setup(x => x.AddBookToCartAsync(It.IsAny<int>())).ReturnsAsync((false, "The book is already in the cart."));

            var addToCartRequest = new AddToCartRequest { BookId = 1 };

            // Act
            var result = await _cartController.AddToCart(addToCartRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.False((bool)((ApiResponse<string>)result.Value).Success);
        }

        // Add more test methods for different scenarios with AddToCart method

        [Fact]
        public async Task RemoveBookFromCartAsync_ValidBookId_Returns204NoContent()
        {
            // Arrange
            _cartServiceMock.Setup(x => x.RemoveBookFromCartAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _cartController.RemoveBookFromCartAsync(1) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [Fact]
        public async Task RemoveBookFromCartAsync_BookNotFound_Returns404NotFound()
        {
            // Arrange
            _cartServiceMock.Setup(x => x.RemoveBookFromCartAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _cartController.RemoveBookFromCartAsync(1) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.False((bool)((ApiResponse<string>)result.Value).Success);
        }
    }
}
