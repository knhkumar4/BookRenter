using BookRenterData.Context;
using Moq;

namespace BookRenterData.Tests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenDbContextIsNull()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => new UnitOfWork.UnitOfWork(null));
        }

        [Fact]
        public void BookRepository_ShouldReturnNonNullRepositoryInstance()
        {
            // Arrange
            var dbContextMock = new Mock<BookRenterContext>();
            var unitOfWork = new UnitOfWork.UnitOfWork(dbContextMock.Object);

            // Act
            var bookRepository = unitOfWork.BookRepository;

            // Assert
            Assert.NotNull(bookRepository);
        }

        [Fact]
        public void CartBookRepository_ShouldReturnNonNullRepositoryInstance()
        {
            // Arrange
            var dbContextMock = new Mock<BookRenterContext>();
            var unitOfWork = new UnitOfWork.UnitOfWork(dbContextMock.Object);

            // Act
            var cartBookRepository = unitOfWork.CartBookRepository;

            // Assert
            Assert.NotNull(cartBookRepository);
        }

        [Fact]
        public void UserRepository_ShouldReturnNonNullRepositoryInstance()
        {
            // Arrange
            var dbContextMock = new Mock<BookRenterContext>();
            var unitOfWork = new UnitOfWork.UnitOfWork(dbContextMock.Object);

            // Act
            var userRepository = unitOfWork.UserRepository;

            // Assert
            Assert.NotNull(userRepository);
        }

        [Fact]
        public void InventoryRepository_ShouldReturnNonNullRepositoryInstance()
        {
            // Arrange
            var dbContextMock = new Mock<BookRenterContext>();
            var unitOfWork = new UnitOfWork.UnitOfWork(dbContextMock.Object);

            // Act
            var inventoryRepository = unitOfWork.InventoryRepository;

            // Assert
            Assert.NotNull(inventoryRepository);
        }

        [Fact]
        public async Task CompleteAsync_ShouldSaveChanges()
        {
            // Arrange
            var dbContextMock = new Mock<BookRenterContext>();
            var unitOfWork = new UnitOfWork.UnitOfWork(dbContextMock.Object);

            // Act
            await unitOfWork.CompleteAsync();

            // Assert
            dbContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DisposeAsync_ShouldDisposeDbContext()
        {
            // Arrange
            var dbContextMock = new Mock<BookRenterContext>();
            var unitOfWork = new UnitOfWork.UnitOfWork(dbContextMock.Object);

            // Act
            await unitOfWork.DisposeAsync();

            // Assert
            dbContextMock.Verify(x => x.DisposeAsync(), Times.Once);
        }

        [Fact]
        public void Dispose_ShouldDisposeDbContext()
        {
            // Arrange
            var dbContextMock = new Mock<BookRenterContext>();
            var unitOfWork = new UnitOfWork.UnitOfWork(dbContextMock.Object);

            // Act
            unitOfWork.Dispose();

            // Assert
            dbContextMock.Verify(x => x.DisposeAsync(), Times.Once);
        }
    }
}
