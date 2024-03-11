using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookRenter.Services;
using BookRenterData.Entities;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Models;
using BookRenter.Services.Models;

namespace BookRenterDataServices.Test
{
    public class InventoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly InventoryService _inventoryService;

        public InventoryServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _inventoryService = new InventoryService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetInventoryByBookIdAsync_ReturnsInventoryResponse()
        {
            // Arrange
            var bookId = 1;
            var inventory = new Inventory { BookId = bookId, Quantity = 10 };
            var expectedResponse = new InventoryResponse { BookId = bookId, Quantity = 10 };

            _mockUnitOfWork.Setup(uow => uow.InventoryRepository.GetInventoryWithBookByBookIdAsync(bookId))
                .ReturnsAsync(inventory);

            // Act
            var result = await _inventoryService.GetInventoryByBookIdAsync(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.BookId, result.BookId);
            Assert.Equal(expectedResponse.Quantity, result.Quantity);
        }

        [Fact]
        public async Task GetAllInventoriesAsync_ReturnsInventoryResponses()
        {
            // Arrange
            var inventories = new List<Inventory>
            {
                new Inventory { BookId = 1, Quantity = 5 },
                new Inventory { BookId = 2, Quantity = 8 }
            };

            var expectedResponses = inventories.Select(inv => new InventoryResponse { BookId = inv.BookId, Quantity = inv.Quantity });

            _mockUnitOfWork.Setup(uow => uow.InventoryRepository.GetAllInventoriesWithBooksAsync())
                .ReturnsAsync(inventories);

            // Act
            var results = await _inventoryService.GetAllInventoriesAsync();

            // Assert
            Assert.NotNull(results);
            Assert.Equal(expectedResponses.Count(), results.Count());
            Assert.Equal(expectedResponses, results, new InventoryResponseComparer());
        }
    }

    // Custom comparer to compare InventoryResponse objects
    public class InventoryResponseComparer : IEqualityComparer<InventoryResponse>
    {
        public bool Equals(InventoryResponse x, InventoryResponse y)
        {
            return x.BookId == y.BookId && x.Quantity == y.Quantity;
        }

        public int GetHashCode(InventoryResponse obj)
        {
            return obj.BookId.GetHashCode() ^ obj.Quantity.GetHashCode();
        }
    }
}
