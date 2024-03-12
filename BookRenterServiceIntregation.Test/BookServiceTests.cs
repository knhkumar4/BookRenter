using Xunit;
using BookRenter.Services;
using BookRenterData.UnitOfWork;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookRenterData;
using System.Linq;
using BookRenterData.Context;
using BookRenterData.Entities;

namespace BookRenterServiceIntregation.Test
{
        public class BookServiceTests
        {
            [Fact]
            public async Task GetBookResponseByIdAsync_ReturnsCorrectBook()
            {
                // Arrange
                var options = new DbContextOptionsBuilder<BookRenterContext>().UseInMemoryDatabase(databaseName: "BookRenterTestDb")
                    .Options;

                // Using a fresh context to mimic a clean test environment
                using (var context = new BookRenterContext(options))
                {
                    context.Books.Add(new Book { BookId = 1, Title = "Test Book", Author = "Test Author", Description="Des",Genre="My gener",Price=20.00, RentPrice=1.80,CreatedDate=DateTime.Now });
                    context.SaveChanges();
                }

                using (var context = new BookRenterContext(options))
                {
                    var unitOfWork = new UnitOfWork(context);
                    var bookService = new BookService(unitOfWork);

                    // Act
                    var result = await bookService.GetBookResponseByIdAsync(1);

                    // Assert
                    Assert.NotNull(result);
                    Assert.Equal("Test Book", result.Title);
                    Assert.Equal("Test Author", result.Author);
                }
            }
        }
    }

