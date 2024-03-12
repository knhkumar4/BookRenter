using BookRenter.Services;
using BookRenterData.Entities;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

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
            var user = new User { UserId = 1 }; 
            _mockUserClaimService.Setup(x => x.GetUserFromClaimAsync()).ReturnsAsync(user);

            var bookIdToAdd = 123; 

            var existingCartItem = (CartBook)null;
            _mockUnitOfWork.Setup(x => x.CartBookRepository.GetByBookIdAndUserIdAsync(bookIdToAdd, user.UserId))
                           .ReturnsAsync(existingCartItem);

            var cartItemCount = 3; 
            _mockUnitOfWork.Setup(x => x.CartBookRepository.GetCartItemCountAsync(user.UserId))
                           .ReturnsAsync(cartItemCount);

            _mockUnitOfWork.Setup(x => x.CartBookRepository.AddAsync(It.IsAny<CartBook>()))
                           .ReturnsAsync(new CartBook()); 

            // Act
            var (success, _) = await _cartService.AddBookToCartAsync(bookIdToAdd);

            // Assert
            Assert.True(success);
            _mockUnitOfWork.Verify(x => x.CartBookRepository.AddAsync(It.IsAny<CartBook>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddBookToCartAsync_BookAlreadyInCart_Failure()
        {
            // Arrange
            var user = new User { UserId = 1 }; 
            _mockUserClaimService.Setup(x => x.GetUserFromClaimAsync()).ReturnsAsync(user);

            var bookIdToAdd = 123; 

            var existingCartItem = new CartBook(); 
            _mockUnitOfWork.Setup(x => x.CartBookRepository.GetByBookIdAndUserIdAsync(bookIdToAdd, user.UserId))
                           .ReturnsAsync(existingCartItem);

            // Act
            var (success, _) = await _cartService.AddBookToCartAsync(bookIdToAdd);

            // Assert
            Assert.False(success);
            _mockUnitOfWork.Verify(x => x.CartBookRepository.AddAsync(It.IsAny<CartBook>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task AddBookToCartAsync_CartFull_Failure()
        {
            // Arrange
            var user = new User { UserId = 1 }; 
            _mockUserClaimService.Setup(x => x.GetUserFromClaimAsync()).ReturnsAsync(user);

            var bookIdToAdd = 123; 

            var existingCartItem = (CartBook)null;
            _mockUnitOfWork.Setup(x => x.CartBookRepository.GetByBookIdAndUserIdAsync(bookIdToAdd, user.UserId))
                           .ReturnsAsync(existingCartItem);

            var cartItemCount = 5; 
            _mockUnitOfWork.Setup(x => x.CartBookRepository.GetCartItemCountAsync(user.UserId))
                           .ReturnsAsync(cartItemCount);

            // Act
            var (success, _) = await _cartService.AddBookToCartAsync(bookIdToAdd);

            // Assert
            Assert.False(success);
            _mockUnitOfWork.Verify(x => x.CartBookRepository.AddAsync(It.IsAny<CartBook>()), Times.Never);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task RemoveBookFromCartAsync_BookInCart_Success()
        {
            // Arrange
            var user = new User { UserId = 1 }; 
            _mockUserClaimService.Setup(x => x.GetUserFromClaimAsync()).ReturnsAsync(user);

            var bookIdToRemove = 123; 

            var cartItemToRemove = new CartBook(); 
            _mockUnitOfWork.Setup(x => x.CartBookRepository.GetByBookIdAndUserIdAsync(bookIdToRemove, user.UserId))
                           .ReturnsAsync(cartItemToRemove);

            _mockUnitOfWork.Setup(x => x.CartBookRepository.DeleteAsync(It.IsAny<CartBook>()))
                           .Returns(Task.CompletedTask);

            // Act
            var success = await _cartService.RemoveBookFromCartAsync(bookIdToRemove);

            // Assert
            Assert.True(success);
            _mockUnitOfWork.Verify(x => x.CartBookRepository.DeleteAsync(It.IsAny<CartBook>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllCartItemsAsync_ReturnsCartBookResponses()
        {
            // Arrange
            var user = new User { UserId = 1 }; 
            _mockUserClaimService.Setup(x => x.GetUserFromClaimAsync()).ReturnsAsync(user);

            var cartBooks = new List<CartBook> 
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
