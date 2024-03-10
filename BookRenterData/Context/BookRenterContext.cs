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
        public DbSet<CartBook> CartBooks { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships, keys, and other entity configurations

            // Configure many-to-many relationship between Book and User (via CartBook)
            //modelBuilder.Entity<CartBook>()
            //    .HasKey(cb => new { cb.UserId, cb.BookId });

            //modelBuilder.Entity<CartBook>()
            //    .HasOne(cb => cb.User)
            //    .WithMany(u => u.CartBooks)
            //    .HasForeignKey(cb => cb.UserId);

            //modelBuilder.Entity<CartBook>()
            //    .HasOne(cb => cb.Book)
            //    .WithMany()
            //    .HasForeignKey(cb => cb.BookId);

            //// Configure one-to-many relationship between Book and Inventory
            //modelBuilder.Entity<Inventory>()
            //    .HasOne(i => i.Book)
            //    .WithMany(b => b.Inventories)
            //    .HasForeignKey(i => i.BookId);
        }
    }
}
