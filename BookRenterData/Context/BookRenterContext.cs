using BookRenterData.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookRenterData.Context
{
    public class BookRenterContext : DbContext
    {
        public BookRenterContext(DbContextOptions<BookRenterContext> options) : base(options)
        {
        }

        // DbSet for each entity

        public DbSet<Book> Books { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartBook> CartBooks { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships, keys, and other entity configurations
            // Configure many-to-many relationship between Book and Cart
            modelBuilder.Entity<CartBook>()
                .HasKey(cb => new { cb.CartId, cb.BookId });

            modelBuilder.Entity<CartBook>()
                .HasOne(cb => cb.Cart)
                .WithMany(c => c.CartBooks)
                .HasForeignKey(cb => cb.CartId);

            modelBuilder.Entity<CartBook>()
                .HasOne(cb => cb.Book)
                .WithMany(b => b.CartBooks)
                .HasForeignKey(cb => cb.BookId);

            // Configure one-to-many relationship between User and Cart
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId);
        }
    }
}
