using BookRenter.Services;
using BookRenterData.Context;
using BookRenterData.Entities;
using BookRenterData.UnitOfWork;
using BookRenterService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BookRenterServiceIntregation.Test
{
    public class CheckoutServiceTests
    {
        [Fact]
        public async Task CheckoutBooksAsync_SuccessfulCheckout()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BookRenterContext>()
                .UseInMemoryDatabase(databaseName: "BookRenterTestDb")
                .Options;

            using (var context = new BookRenterContext(options))
            {
                // Populate the database with sample data
                context.CartBooks.Add(new CartBook { UserId = 1, BookId = 1 });
                context.CartBooks.Add(new CartBook { UserId = 1, BookId = 2 });
                context.Inventories.Add(new Inventory { BookId = 1, Quantity = 1, RowVersion = GenerateRandomByteArray() });
                context.Inventories.Add(new Inventory { BookId = 2, Quantity = 1, RowVersion = GenerateRandomByteArray() });
                context.SaveChanges();
            }

            // Mock IHttpContextAccessor
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            // Mock IUserClaimService
            var userClaimServiceMock = new Mock<IUserClaimService>();
            userClaimServiceMock.Setup(u => u.GetUserFromClaimAsync())
                                .ReturnsAsync(new User { UserId = 1 });

            using (var context = new BookRenterContext(options))
            {
                var unitOfWork = new UnitOfWork(context);
                var checkoutService = new CheckoutService(unitOfWork, userClaimServiceMock.Object);

                // Act
                var result = await checkoutService.CheckoutBooksAsync();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("Checkout successful.", result.Message);

                // Check if cart is empty after successful checkout
                var cartBooks = context.CartBooks.Where(cb => cb.UserId == 1).ToList();
                Assert.Empty(cartBooks);

                // Check if inventory is updated after successful checkout
                var inventories = context.Inventories.ToList();
                foreach (var inventory in inventories)
                {
                    Assert.Equal(0, inventory.Quantity);
                }
            }
        }

        private byte[] GenerateRandomByteArray()
        {
            var byteArray = new byte[8];
            new Random().NextBytes(byteArray);
            return byteArray;
        }
    }
}
