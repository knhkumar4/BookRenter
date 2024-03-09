
using BookRenterData.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookRenterData.Contexts
{
    public class BookRenterContext : DbContext
    {
        public BookRenterContext(DbContextOptions<BookRenterContext> options) : base(options)
        {
        }

        // DbSet for each entity
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartBook> CartBooks { get; set; }
        public DbSet<Inventory> Inventories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships, keys, and other entity configurations
            modelBuilder.Entity<CartBook>()
                .HasKey(cb => new { cb.CartId, cb.BookId });
        }
    }
}
