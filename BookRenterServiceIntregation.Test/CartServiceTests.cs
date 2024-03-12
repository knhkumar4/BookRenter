using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookRenter.Services;
using BookRenterData.Entities;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterRepository.Repositories.Interfaces;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using Moq;
using Xunit;

namespace BookRenterServiceIntregation.Test
{
    public class CartServiceTests
    {
        [Fact]
        public async Task AddBookToCartAsync_BookNotInCart_Success()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userClaimServiceMock = new Mock<IUserClaimService>();
            var userId = 1;
            userClaimServiceMock.Setup(u => u.GetUserFromClaimAsync()).ReturnsAsync(new User { UserId = userId });
            var cartBookRepositoryMock = new Mock<ICartBookRepository>();
            cartBookRepositoryMock.Setup(c => c.GetByBookIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((CartBook)null);
            cartBookRepositoryMock.Setup(c => c.GetCartItemCountAsync(It.IsAny<int>())).ReturnsAsync(4); // Simulate a cart with 4 items
            unitOfWorkMock.Setup(u => u.CartBookRepository).Returns(cartBookRepositoryMock.Object);
            var cartService = new CartService(unitOfWorkMock.Object, userClaimServiceMock.Object);
            var bookId = 123;

            // Act
            var result = await cartService.AddBookToCartAsync(bookId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Book added to cart successfully.", result.Message);
        }

        [Fact]
        public async Task AddBookToCartAsync_BookAlreadyInCart_Failure()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userClaimServiceMock = new Mock<IUserClaimService>();
            var userId = 1;
            userClaimServiceMock.Setup(u => u.GetUserFromClaimAsync()).ReturnsAsync(new User { UserId = userId });
            var cartBookRepositoryMock = new Mock<ICartBookRepository>();
            cartBookRepositoryMock.Setup(c => c.GetByBookIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new CartBook());
            unitOfWorkMock.Setup(u => u.CartBookRepository).Returns(cartBookRepositoryMock.Object);
            var cartService = new CartService(unitOfWorkMock.Object, userClaimServiceMock.Object);
            var bookId = 123;

            // Act
            var result = await cartService.AddBookToCartAsync(bookId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("The book is already in the cart.", result.Message);
        }

        [Fact]
        public async Task AddBookToCartAsync_CartFull_Failure()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userClaimServiceMock = new Mock<IUserClaimService>();
            var userId = 1;
            userClaimServiceMock.Setup(u => u.GetUserFromClaimAsync()).ReturnsAsync(new User { UserId = userId });
            var cartBookRepositoryMock = new Mock<ICartBookRepository>();
            cartBookRepositoryMock.Setup(c => c.GetByBookIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((CartBook)null);
            cartBookRepositoryMock.Setup(c => c.GetCartItemCountAsync(It.IsAny<int>())).ReturnsAsync(5); // Simulate a cart with 5 items
            unitOfWorkMock.Setup(u => u.CartBookRepository).Returns(cartBookRepositoryMock.Object);
            var cartService = new CartService(unitOfWorkMock.Object, userClaimServiceMock.Object);
            var bookId = 123;

            // Act
            var result = await cartService.AddBookToCartAsync(bookId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("A maximum of 5 books can be added to the cart.", result.Message);
        }

        [Fact]
        public async Task RemoveBookFromCartAsync_BookInCart_Success()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userClaimServiceMock = new Mock<IUserClaimService>();
            var userId = 1;
            userClaimServiceMock.Setup(u => u.GetUserFromClaimAsync()).ReturnsAsync(new User { UserId = userId });
            var cartBookRepositoryMock = new Mock<ICartBookRepository>();
            cartBookRepositoryMock.Setup(c => c.GetByBookIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new CartBook());
            unitOfWorkMock.Setup(u => u.CartBookRepository).Returns(cartBookRepositoryMock.Object);
            var cartService = new CartService(unitOfWorkMock.Object, userClaimServiceMock.Object);
            var bookId = 123;

            // Act
            var result = await cartService.RemoveBookFromCartAsync(bookId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveBookFromCartAsync_BookNotInCart_Failure()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userClaimServiceMock = new Mock<IUserClaimService>();
            var userId = 1;
            userClaimServiceMock.Setup(u => u.GetUserFromClaimAsync()).ReturnsAsync(new User { UserId = userId });
            var cartBookRepositoryMock = new Mock<ICartBookRepository>();
            cartBookRepositoryMock.Setup(c => c.GetByBookIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((CartBook)null);
            unitOfWorkMock.Setup(u => u.CartBookRepository).Returns(cartBookRepositoryMock.Object);
            var cartService = new CartService(unitOfWorkMock.Object, userClaimServiceMock.Object);
            var bookId = 123;

            // Act
            var result = await cartService.RemoveBookFromCartAsync(bookId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllCartItemsAsync_ReturnsCartItems()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userClaimServiceMock = new Mock<IUserClaimService>();
            var userId = 1;
            userClaimServiceMock.Setup(u => u.GetUserFromClaimAsync()).ReturnsAsync(new User { UserId = userId });
            var cartBooks = new List<CartBook>
            {
                new CartBook { UserId = userId, Book = new Book { BookId = 1, Title = "Book 1" } },
                new CartBook { UserId = userId, Book = new Book { BookId = 2, Title = "Book 2" } }
            };
            var cartBookRepositoryMock = new Mock<ICartBookRepository>();
            cartBookRepositoryMock.Setup(c => c.GetByUserIdAsync(It.IsAny<int>())).ReturnsAsync(cartBooks);
            unitOfWorkMock.Setup(u => u.CartBookRepository).Returns(cartBookRepositoryMock.Object);
            var cartService = new CartService(unitOfWorkMock.Object, userClaimServiceMock.Object);

            // Act
            var result = await cartService.GetAllCartItemsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Book 1", result.ElementAt(0).BookName);
            Assert.Equal("Book 2", result.ElementAt(1).BookName);
        }
    }
}
