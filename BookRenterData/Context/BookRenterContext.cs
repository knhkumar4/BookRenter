using BookRenterData.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookRenterData.Context
{
    public class BookRenterContext : DbContext
    {
        public BookRenterContext(DbContextOptions<BookRenterContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<CartBook> CartBooks { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// Configuring one-to-one relationship between Book and Inventory
            modelBuilder.Entity<Book>()
                .HasOne<Inventory>(b => b.Inventory)
                .WithOne(i => i.Book)
                .HasForeignKey<Inventory>(i => i.BookId);

            // Configure concurrency token for Inventory entity
            modelBuilder.Entity<Inventory>()
                .Property(i => i.RowVersion)
                .IsConcurrencyToken();


        }
    }
}
