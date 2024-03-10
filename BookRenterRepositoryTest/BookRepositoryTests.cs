using BookRenterData.Context;
using BookRenterData.Entities;
using JC.Samples.AsyncRepository.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BookRenterData.Tests.Repositories
{
    public class BookRepositoryTests
    {
        private readonly DbContextOptions<BookRenterContext> _dbContextOptions;

        public BookRepositoryTests()
        {
            // Use a unique name for each test method to ensure isolation
            _dbContextOptions = new DbContextOptionsBuilder<BookRenterContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private Book CreateSampleBook() => new Book
        {
            Title = "Sample Book",
            Description = "A sample book for testing purposes.",
            Author = "John Doe",
            Genre = "Fiction",
            Price = 15.99,
            CreatedDate = DateTime.UtcNow
        };

        [Fact]
        public async Task AddAsync_ShouldAddBook()
        {
            using (var context = new BookRenterContext(_dbContextOptions))
            {
                var repository = new BookRepository(context);
                var sampleBook = CreateSampleBook();

                await repository.AddAsync(sampleBook);
                await context.SaveChangesAsync();
            }

            // Verify the book was added
            using (var context = new BookRenterContext(_dbContextOptions))
            {
                Assert.Equal(1, await context.Books.CountAsync());
                var book = await context.Books.FirstOrDefaultAsync();
                Assert.NotNull(book);
                Assert.Equal("Sample Book", book.Title);
            }
        }

        [Fact]
        public async Task FindByIdAsync_ShouldReturnBook()
        {
            // Seed a book to find
            using (var context = new BookRenterContext(_dbContextOptions))
            {
                var repository = new BookRepository(context);
                var sampleBook = CreateSampleBook();

                await repository.AddAsync(sampleBook);
                await context.SaveChangesAsync();
            }

            // Attempt to find the seeded book
            using (var context = new BookRenterContext(_dbContextOptions))
            {
                var repository = new BookRepository(context);
                var book = await repository.GetByIdAsync(1); // Assuming the ID is 1

                Assert.NotNull(book);
                Assert.Equal("Sample Book", book.Title);
            }
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateBookDetails()
        {
            using (var context = new BookRenterContext(_dbContextOptions))
            {
                // Setup - Add a book to update
                var repository = new BookRepository(context);
                var book = CreateSampleBook();
                await repository.AddAsync(book);
                await context.SaveChangesAsync();

                // Act - Update the book
                book.Title = "Updated Title";
                await repository.UpdateAsync(book);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new BookRenterContext(_dbContextOptions))
            {
                var updatedBook = await context.Books.FindAsync(1); // Assuming ID is 1
                Assert.NotNull(updatedBook);
                Assert.Equal("Updated Title", updatedBook.Title);
            }
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBook()
        {
            using (var context = new BookRenterContext(_dbContextOptions))
            {
                // Setup - Add a book to delete
                var repository = new BookRepository(context);
                var book = CreateSampleBook();
                await repository.AddAsync(book);
                await context.SaveChangesAsync();

                // Act - Delete the book
                await repository.DeleteAsync(book);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = new BookRenterContext(_dbContextOptions))
            {
                var bookExists = await context.Books.AnyAsync();
                Assert.False(bookExists);
            }
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBooks()
        {
            using (var context = new BookRenterContext(_dbContextOptions))
            {
                // Setup - Add multiple books
                var repository = new BookRepository(context);
                var book1 = CreateSampleBook();
                var book2 = CreateSampleBook();
                book2.Title = "Another Sample Book";

                await repository.AddAsync(book1);
                await repository.AddAsync(book2);
                await context.SaveChangesAsync();

                // Act
                var books = await repository.GetAllAsync();

                // Assert
                Assert.NotNull(books);
                Assert.Equal(2, books.Count());
            }
        }

        [Fact]
        public async Task FindByConditionAsync_ShouldReturnFilteredBooks()
        {
            using (var context = new BookRenterContext(_dbContextOptions))
            {
                // Setup - Add books with different genres
                var repository = new BookRepository(context);
                var book1 = CreateSampleBook(); // Default genre: "Fiction"
                var book2 = CreateSampleBook();
                book2.Genre = "Non-Fiction";

                await repository.AddAsync(book1);
                await repository.AddAsync(book2);
                await context.SaveChangesAsync();

                // Act - Attempt to find books by a specific genre
                var fictionBooks = await repository.GetManyAsync(b => b.Genre == "Fiction");

                // Assert
                Assert.NotNull(fictionBooks);
                Assert.Single(fictionBooks);
                Assert.All(fictionBooks, book => Assert.Equal("Fiction", book.Genre));
            }
        }
    }
}
