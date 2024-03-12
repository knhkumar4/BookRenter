using BookRenter.Controllers;
using BookRenter.Services.Models;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookRenterAPI.Test
{
    public class InventoryControllerTests
    {
        private readonly Mock<IInventoryService> _inventoryServiceMock;
        private readonly InventoryController _controller;

        public InventoryControllerTests()
        {
            _inventoryServiceMock = new Mock<IInventoryService>();
            _controller = new InventoryController(_inventoryServiceMock.Object);
        }

        [Fact]
        public async Task GetInventoryByBookId_ExistingBookId_ReturnsOk()
        {
            // Arrange
            var bookId = 1;
            var expectedInventory = new InventoryResponse { InventoryId = 1, BookId = 1, Quantity = 10, BookTitle = "Sample Book" };

            _inventoryServiceMock.Setup(x => x.GetInventoryByBookIdAsync(bookId))
                                 .ReturnsAsync(expectedInventory);

            // Act
            var result = await _controller.GetInventoryByBookId(bookId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InventoryResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var inventory = Assert.IsType<InventoryResponse>(okResult.Value);

            Assert.Equal(expectedInventory.InventoryId, inventory.InventoryId);
            Assert.Equal(expectedInventory.BookId, inventory.BookId);
            Assert.Equal(expectedInventory.Quantity, inventory.Quantity);
            Assert.Equal(expectedInventory.BookTitle, inventory.BookTitle);
        }

        [Fact]
        public async Task GetInventoryByBookId_NonExistingBookId_ReturnsNotFound()
        {
            // Arrange
            var bookId = 2;

            _inventoryServiceMock.Setup(x => x.GetInventoryByBookIdAsync(bookId))
                                 .ReturnsAsync((InventoryResponse)null);

            // Act
            var result = await _controller.GetInventoryByBookId(bookId);

            // Assert
            Assert.IsType<ActionResult<InventoryResponse>>(result);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllInventories_ReturnsOk()
        {
            // Arrange
            var expectedInventories = new List<InventoryResponse>
            {
                new InventoryResponse { InventoryId = 1, BookId = 1, Quantity = 10, BookTitle = "Sample Book 1" },
                new InventoryResponse { InventoryId = 2, BookId = 2, Quantity = 5, BookTitle = "Sample Book 2" }
            };

            _inventoryServiceMock.Setup(x => x.GetAllInventoriesAsync())
                                 .ReturnsAsync(expectedInventories);

            // Act
            var result = await _controller.GetAllInventories();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<InventoryResponse>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var inventories = Assert.IsType<List<InventoryResponse>>(okResult.Value);

            Assert.Equal(expectedInventories.Count, inventories.Count);
            for (int i = 0; i < expectedInventories.Count; i++)
            {
                Assert.Equal(expectedInventories[i].InventoryId, inventories[i].InventoryId);
                Assert.Equal(expectedInventories[i].BookId, inventories[i].BookId);
                Assert.Equal(expectedInventories[i].Quantity, inventories[i].Quantity);
                Assert.Equal(expectedInventories[i].BookTitle, inventories[i].BookTitle);
            }
        }

        [Fact]
        public async Task AddInventory_ValidInventoryRequest_ReturnsCreatedAtAction()
        {
            // Arrange
            var inventoryRequest = new InventoryRequest { BookId = 1, Quantity = 10 };
            var expectedInventoryResponse = new InventoryResponse { InventoryId = 1, BookId = 1, Quantity = 10, BookTitle = "Sample Book" };

            _inventoryServiceMock.Setup(x => x.AddInventoryAsync(inventoryRequest))
                                 .ReturnsAsync(expectedInventoryResponse);

            // Act
            var result = await _controller.AddInventory(inventoryRequest);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InventoryResponse>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var inventory = Assert.IsType<InventoryResponse>(createdAtActionResult.Value);

            Assert.Equal(expectedInventoryResponse.InventoryId, inventory.InventoryId);
            Assert.Equal(expectedInventoryResponse.BookId, inventory.BookId);
            Assert.Equal(expectedInventoryResponse.Quantity, inventory.Quantity);
            Assert.Equal(expectedInventoryResponse.BookTitle, inventory.BookTitle);
        }

        [Fact]
        public async Task AddInventory_InvalidInventoryRequest_ReturnsBadRequest()
        {
            // Arrange
            var inventoryRequest = new InventoryRequest(); // Invalid request

            // Act
            var result = await _controller.AddInventory(inventoryRequest);

            // Assert
            Assert.IsType<ActionResult<InventoryResponse>>(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task AddInventory_ErrorInService_ReturnsInternalServerError()
        {
            // Arrange
            var inventoryRequest = new InventoryRequest { BookId = 1, Quantity = 10 };

            _inventoryServiceMock.Setup(x => x.AddInventoryAsync(inventoryRequest))
                                 .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _controller.AddInventory(inventoryRequest);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InventoryResponse>>(result);
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult.Result);

            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while adding the inventory.", statusCodeResult.Value);
        }
    }
}
