using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookRenter.Controllers;
using BookRenter.Models.Responses;
using BookRenter.Services.Interfaces;
using BookRenter.Services.Models;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BookRenterAPI.Test
{
    public class InventoryControllerTests
    {
        private readonly Mock<IInventoryService> _inventoryServiceMock;
        private readonly InventoryController _inventoryController;

        public InventoryControllerTests()
        {
            _inventoryServiceMock = new Mock<IInventoryService>();
            _inventoryController = new InventoryController(_inventoryServiceMock.Object);
        }

        [Fact]
        public async Task GetInventoryByBookId_ExistingBook_ReturnsOk()
        {
            // Arrange
            var bookId = 1;
            var expectedInventory = new InventoryResponse
            {
                InventoryId = 1,
                BookId = bookId,
                Quantity = 5,
                BookTitle = "Sample Book"
            };

            _inventoryServiceMock.Setup(x => x.GetInventoryByBookIdAsync(bookId))
                               .ReturnsAsync(expectedInventory);

            // Act
            var result = await _inventoryController.GetInventoryByBookId(bookId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InventoryResponse>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var inventoryResponse = Assert.IsType<InventoryResponse>(okResult.Value);

            Assert.Equal(expectedInventory.InventoryId, inventoryResponse.InventoryId);
            Assert.Equal(expectedInventory.BookId, inventoryResponse.BookId);
            Assert.Equal(expectedInventory.Quantity, inventoryResponse.Quantity);
            Assert.Equal(expectedInventory.BookTitle, inventoryResponse.BookTitle);
        }

        [Fact]
        public async Task GetInventoryByBookId_NonExistingBook_ReturnsNotFound()
        {
            // Arrange
            var bookId = 2;

            _inventoryServiceMock.Setup(x => x.GetInventoryByBookIdAsync(bookId))
                               .ReturnsAsync((InventoryResponse)null);

            // Act
            var result = await _inventoryController.GetInventoryByBookId(bookId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<InventoryResponse>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetAllInventories_ReturnsOk()
        {
            // Arrange
            var expectedInventories = new List<InventoryResponse>
            {
                new InventoryResponse { InventoryId = 1, BookId = 101, Quantity = 3, BookTitle = "Book A" },
                new InventoryResponse { InventoryId = 2, BookId = 102, Quantity = 5, BookTitle = "Book B" }
            };

            _inventoryServiceMock.Setup(x => x.GetAllInventoriesAsync())
                               .ReturnsAsync(expectedInventories);

            // Act
            var result = await _inventoryController.GetAllInventories();

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
        public async Task AddInventory_ValidRequest_ReturnsCreatedAtAction()
        {
            // Arrange
            _inventoryServiceMock.Setup(x => x.AddInventoryAsync(It.IsAny<InventoryRequest>()))
                               .ReturnsAsync(new InventoryResponse { InventoryId = 1, BookId = 123, Quantity = 5, BookTitle = "Sample Book" });

            // Act
            var result = await _inventoryController.AddInventory(new InventoryRequest());

            // Assert
            Assert.IsType<ActionResult<InventoryResponse>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);

            Assert.Equal(201, createdAtActionResult.StatusCode);

            var inventoryResponse = Assert.IsType<InventoryResponse>(createdAtActionResult.Value);
            Assert.Equal(1, inventoryResponse.InventoryId);
            Assert.Equal(123, inventoryResponse.BookId);
            Assert.Equal(5, inventoryResponse.Quantity);
            Assert.Equal("Sample Book", inventoryResponse.BookTitle);
        }

        [Fact]
        public async Task AddInventory_NullRequest_ReturnsBadRequest()
        {
            // Act
            var result = await _inventoryController.AddInventory(null);

            // Assert
            Assert.IsType<ActionResult<InventoryResponse>>(result);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task AddInventory_ServiceError_ReturnsInternalServerError()
        {
            // Arrange
            _inventoryServiceMock.Setup(x => x.AddInventoryAsync(It.IsAny<InventoryRequest>()))
                               .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await _inventoryController.AddInventory(new InventoryRequest());

            // Assert
            var actionResult = Assert.IsType<ActionResult<InventoryResponse>>(result);
            var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);

            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("An error occurred while adding the inventory.", objectResult.Value);
        }
    }
}
