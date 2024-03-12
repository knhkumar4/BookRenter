using BookRenter.Controllers;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookRenterAPI.Test
{
    public class CheckoutControllerTests
    {
        [Fact]
        public async Task CheckoutBooksAsync_SuccessfulCheckout_ReturnsOk()
        {
            // Arrange
            var checkoutServiceMock = new Mock<ICheckoutService>();
            checkoutServiceMock.Setup(x => x.CheckoutBooksAsync())
                              .ReturnsAsync(new CheckoutResponse { Success = true, Message = "Checkout successful." });

            var controller = new CheckoutController(checkoutServiceMock.Object);

            // Act
            var result = await controller.CheckoutBooksAsync();

            // Assert
            var actionResult = Assert.IsType<ActionResult<ApiResponse<string>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsType<ApiResponse<string>>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Checkout successful.", response.Message);
        }

        [Fact]
        public async Task CheckoutBooksAsync_NoBooksInCart_ReturnsBadRequest()
        {
            // Arrange
            var checkoutServiceMock = new Mock<ICheckoutService>();
            checkoutServiceMock.Setup(x => x.CheckoutBooksAsync())
                              .ReturnsAsync(new CheckoutResponse { Success = false, Message = "No books found in the cart." });

            var controller = new CheckoutController(checkoutServiceMock.Object);

            // Act
            var result = await controller.CheckoutBooksAsync();

            // Assert
            var actionResult = Assert.IsType<ActionResult<ApiResponse<string>>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No books found in the cart.", response.Message);
        }

        [Fact]
        public async Task CheckoutBooksAsync_BookNotAvailableInInventory_ReturnsBadRequest()
        {
            // Arrange
            var checkoutServiceMock = new Mock<ICheckoutService>();
            checkoutServiceMock.Setup(x => x.CheckoutBooksAsync())
                              .ReturnsAsync(new CheckoutResponse { Success = false, Message = "Book with ID 1 is not available in inventory." });

            var controller = new CheckoutController(checkoutServiceMock.Object);

            // Act
            var result = await controller.CheckoutBooksAsync();

            // Assert
            var actionResult = Assert.IsType<ActionResult<ApiResponse<string>>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Book with ID 1 is not available in inventory.", response.Message);
        }
    }
}
