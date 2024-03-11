using BookRenter.Services;
using BookRenterData.Entities;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Interfaces;
using Moq;

namespace BookRenterDataServices.Test
{
    public class CartServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserClaimService> _mockUserClaimService;
        private readonly CartService _cartService;

        public CartServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserClaimService = new Mock<IUserClaimService>();
            _cartService = new CartService(_mockUnitOfWork.Object, _mockUserClaimService.Object);
        }

        [Fact]
        public async Task AddBookToCartAsync_BookNotInCart_Success()
        {
            // Arrange
            var user = new User { UserId = 1 }; // Adjust user details as needed
            _mockUserClaimService.Setup(x => x.GetUserFromClaimAsync()).ReturnsAsync(user);

            var bookIdToAdd = 123; // Replace with a valid book ID

            var existingCartItem = (CartBook)null;
            _mockUnitOfWork.Setup(x => x.CartBookRepository.GetByBookIdAndUserIdAsync(bookIdToAdd, user.UserId))
                          .ReturnsAsync(existingCartItem);

            var cartItemCount = 3; // Set a value less than the limit for successful addition
            _mockUnitOfWork.Setup(x => x.CartBookRepository.GetCartItemCountAsync(user.UserId))
                          .ReturnsAsync(cartItemCount);

            _mockUnitOfWork.Setup(x => x.CartBookRepository.AddAsync(It.IsAny<CartBook>()))
                          .ReturnsAsync(new CartBook()); // Adjust as needed

            //_mockUnitOfWork.Setup(x => x.CompleteAsync())
            //              .ReturnsAsync(1); // Adjust as needed

            // Act
            var result = await _cartService.AddBookToCartAsync(bookIdToAdd);

            // Assert
            Assert.True(result);
            _mockUnitOfWork.Verify(x => x.CartBookRepository.AddAsync(It.IsAny<CartBook>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddBookToCartAsync_BookAlreadyInCart_Failure()
        {
            // Arrange
            var user = new User { UserId = 1 }; // Adjust user details as needed
            _mockUserClaimService.Setup(x => x.GetUserFromClaimAsync()).ReturnsAsync(user);

            var bookIdToAdd = 123; // Replace with a valid book ID

            var existingCartItem = new CartBook(); // Simulate that the book is already in the cart
            _mockUnitOfWork.Setup(x => x.CartBookRepository.GetByBookIdAndUserIdAsync(bookIdToAdd, user.UserId))
                          .ReturnsAsync(existingCartItem);

            // Act
            var result = await _cartService.AddBookToCartAsync(bookIdToAdd);

            // Assert
            Assert.False(result);
            _mockUnitOfWork.Verify(x => x.CartBookRepository.AddAsync(It.IsAny<CartBook>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task AddBookToCartAsync_CartFull_ThrowsException()
        {
            // Arrange
            var user = new User { UserId = 1 }; // Adjust user details as needed
            _mockUserClaimService.Setup(x => x.GetUserFromClaimAsync()).ReturnsAsync(user);

            var bookIdToAdd = 123; // Replace with a valid book ID

            var existingCartItem = (CartBook)null;
            _mockUnitOfWork.Setup(x => x.CartBookRepository.GetByBookIdAndUserIdAsync(bookIdToAdd, user.UserId))
                          .ReturnsAsync(existingCartItem);

            var cartItemCount = 5; // Set a value greater than or equal to the limit for cart full scenario
            _mockUnitOfWork.Setup(x => x.CartBookRepository.GetCartItemCountAsync(user.UserId))
                          .ReturnsAsync(cartItemCount);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _cartService.AddBookToCartAsync(bookIdToAdd));

            _mockUnitOfWork.Verify(x => x.CartBookRepository.AddAsync(It.IsAny<CartBook>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task RemoveBookFromCartAsync_BookInCart_Success()
        {
            // Arrange
            var user = new User { UserId = 1 }; // Adjust user details as needed
            _mockUserClaimService.Setup(x => x.GetUserFromClaimAsync()).ReturnsAsync(user);

            var bookIdToRemove = 123; // Replace with a valid book ID

            var cartItemToRemove = new CartBook(); // Simulate that the book is in the cart
            _mockUnitOfWork.Setup(x => x.CartBookRepository.GetByBookIdAndUserIdAsync(bookIdToRemove, user.UserId))
                          .ReturnsAsync(cartItemToRemove);

            _mockUnitOfWork.Setup(x => x.CartBookRepository.DeleteAsync(It.IsAny<CartBook>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await _cartService.RemoveBookFromCartAsync(bookIdToRemove);

            // Assert
            Assert.True(result);
            _mockUnitOfWork.Verify(x => x.CartBookRepository.DeleteAsync(It.IsAny<CartBook>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        }

        // ... (other test methods remain the same)

        [Fact]
        public async Task GetAllCartItemsAsync_ReturnsCartBookResponses()
        {
            // Arrange
            var user = new User { UserId = 1 }; // Adjust user details as needed
            _mockUserClaimService.Setup(x => x.GetUserFromClaimAsync()).ReturnsAsync(user);

            var cartBooks = new List<CartBook> // Adjust as needed
            {
                new CartBook { Book = new Book { BookId = 1, Title = "Book1" }, Quantity = 2 },
                new CartBook { Book = new Book { BookId = 2, Title = "Book2" }, Quantity = 1 }
            };
            _mockUnitOfWork.Setup(x => x.CartBookRepository.GetByUserIdAsync(user.UserId))
                          .ReturnsAsync(cartBooks);

            // Act
            var result = await _cartService.GetAllCartItemsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cartBooks.Count, result.Count());

            // Verify that the mapping is done correctly
            Assert.Equal(cartBooks[0].Book.BookId, result.ElementAt(0).BookId);
            Assert.Equal(cartBooks[0].Book.Title, result.ElementAt(0).BookName);
            Assert.Equal(cartBooks[1].Book.BookId, result.ElementAt(1).BookId);
            Assert.Equal(cartBooks[1].Book.Title, result.ElementAt(1).BookName);
        }
    }
}
