using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookRenter.Services;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Interfaces;
using BookRenterData.Entities;

namespace BookRenterDataServices.Test
{
    public class CheckoutServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserClaimService> _mockUserClaimService;
        private readonly CheckoutService _checkoutService;

        public CheckoutServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserClaimService = new Mock<IUserClaimService>();
            _checkoutService = new CheckoutService(_mockUnitOfWork.Object, _mockUserClaimService.Object);
        }

        //[Fact]
        //public async Task CheckoutBooksAsync_AllBooksAvailable_SuccessfulCheckout()
        //{
        //    // Arrange
        //    var user = new User { UserId = 1 }; // Adjust user details as needed
        //    var cartBooks = new List<CartBook>
        //    {
        //        new CartBook { BookId = 1, Quantity = 2 },
        //        new CartBook { BookId = 2, Quantity = 1 }
        //    };

        //    _mockUserClaimService.Setup(x => x.GetUserFromClaimAsync()).ReturnsAsync(user);
        //    _mockUnitOfWork.Setup(x => x.CartBookRepository.GetCartBooksByUserIdAsync(user.UserId))
        //        .ReturnsAsync(cartBooks);

        //    foreach (var cartBook in cartBooks)
        //    {
        //        _mockUnitOfWork.Setup(x => x.InventoryRepository.GetInventoryByBookIdAsync(cartBook.BookId))
        //            .ReturnsAsync(new Inventory { Quantity = cartBook.Quantity });
        //    }

        //    _mockUnitOfWork.Setup(x => x.CompleteAsync())
        //                    .Returns(Task.CompletedTask);
        
        //    // Act
        //    var result = await _checkoutService.CheckoutBooksAsync();

        //    // Assert
        //    Assert.Equal("Checkout successful.", result);
        //    _mockUnitOfWork.Verify(x => x.InventoryRepository.UpdateAsync(It.IsAny<Inventory>()), Times.Exactly(cartBooks.Count));
        //    _mockUnitOfWork.Verify(x => x.CartBookRepository.DeleteAsync(It.IsAny<CartBook>()), Times.Exactly(cartBooks.Count));
        //    _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        //}

        //[Fact]
        //public async Task CheckoutBooksAsync_BookNotAvailable_Failure()
        //{
        //    // Arrange
        //    var user = new User { UserId = 1 }; // Adjust user details as needed
        //    var cartBooks = new List<CartBook>
        //    {
        //        new CartBook { BookId = 1, Quantity = 2 },
        //        new CartBook { BookId = 2, Quantity = 1 }
        //    };

        //    _mockUserClaimService.Setup(x => x.GetUserFromClaimAsync()).ReturnsAsync(user);
        //    _mockUnitOfWork.Setup(x => x.CartBookRepository.GetCartBooksByUserIdAsync(user.UserId))
        //        .ReturnsAsync(cartBooks);

        //    // Simulate a book not being available in the inventory
        //    _mockUnitOfWork.Setup(x => x.InventoryRepository.GetInventoryByBookIdAsync(1))
        //        .ReturnsAsync(new Inventory { Quantity = 0 });

        //    // Act
        //    var result = await _checkoutService.CheckoutBooksAsync();

        //    // Assert
        //    Assert.Equal("Book with ID 1 is not available in inventory.", result);
        //    _mockUnitOfWork.Verify(x => x.InventoryRepository.UpdateAsync(It.IsAny<Inventory>()), Times.Never);
        //    _mockUnitOfWork.Verify(x => x.CartBookRepository.DeleteAsync(It.IsAny<CartBook>()), Times.Never);
        //    _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Never);
        //}

    }
}
