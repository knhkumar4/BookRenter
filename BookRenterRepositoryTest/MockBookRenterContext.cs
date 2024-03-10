using BookRenterData.Context;
using BookRenterData.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BookRenterData.Tests.Repositories
{
    public class MockBookRenterContext : BookRenterContext
    {
        public MockBookRenterContext(DbContextOptions<BookRenterContext> options) : base(options)
        {
        }

        public DbSet<Book> MockBooks { get; set; }
        public DbSet<CartBook> MockCartBooks { get; set; }
        public DbSet<Inventory> MockInventories { get; set; }
        public DbSet<User> MockUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customize the model building process for testing purposes if needed
        }
    }

    public static class MockBookRenterContextFactory
    {
        public static MockBookRenterContext Create()
        {
            var options = new DbContextOptionsBuilder<BookRenterContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            var context = new MockBookRenterContext(options);

            // You can customize the context setup further if needed

            return context;
        }
    }
}
