using BookRenterData.Context;
using BookRenterData.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BookRenterData.Tests
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly BookRenterContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            // You can use an in-memory database for testing purposes
            var options = new DbContextOptionsBuilder<BookRenterContext>()
                 .UseInMemoryDatabase(databaseName: "TestDatabase")
                 .Options;

            // Create an instance of the DbContext with the options
            _dbContext = new BookRenterContext(options);

            // Create the UnitOfWork with the configured DbContext
            _unitOfWork = new UnitOfWork.UnitOfWork(_dbContext);
        }

        [Fact]
        public void BookRepository_ShouldNotBeNull()
        {
            // Arrange
            var unitOfWork = new UnitOfWork.UnitOfWork(_dbContext);

            // Act
            var bookRepository = unitOfWork.BookRepository;

            // Assert
            Assert.NotNull(bookRepository);
        }

        [Fact]
        public void CartBookRepository_ShouldNotBeNull()
        {
            // Arrange
            var unitOfWork = new UnitOfWork.UnitOfWork(_dbContext);

            // Act
            var cartBookRepository = unitOfWork.CartBookRepository;

            // Assert
            Assert.NotNull(cartBookRepository);
        }

        [Fact]
        public void UserRepository_ShouldNotBeNull()
        {
            // Arrange
            var unitOfWork = new UnitOfWork.UnitOfWork(_dbContext);

            // Act
            var userRepository = unitOfWork.UserRepository;

            // Assert
            Assert.NotNull(userRepository);
        }

        [Fact]
        public void InventoryRepository_ShouldNotBeNull()
        {
            // Arrange
            var unitOfWork = new UnitOfWork.UnitOfWork(_dbContext);

            // Act
            var inventoryRepository = unitOfWork.InventoryRepository;

            // Assert
            Assert.NotNull(inventoryRepository);
        }

        [Fact]
        public async Task CompleteAsync_ShouldSaveChanges()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BookRenterContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            var mockDbContext = new Mock<BookRenterContext>(options);
            var unitOfWork = new UnitOfWork.UnitOfWork(mockDbContext.Object);

            // Act
            await unitOfWork.CompleteAsync();

            // Assert
            mockDbContext.Verify(db => db.SaveChangesAsync(default(CancellationToken)), Times.Once);
        }



        public void Dispose()
        {
            // Clean up resources, if needed
            _dbContext.Dispose();
            (_unitOfWork as IDisposable)?.Dispose();
        }
    }
}
