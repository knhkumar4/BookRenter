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

            //// Configure the primary key for Inventory to be the same as BookId
            //modelBuilder.Entity<Inventory>()
            //    .HasKey(i => i.BookId);

            //// Configure many-to-many relationship between Book and User (via CartBook)
            //modelBuilder.Entity<CartBook>()
            //    .HasKey(cb => new { cb.UserId, cb.BookId });

            //modelBuilder.Entity<CartBook>()
            //    .HasOne(cb => cb.User)
            //    .WithMany(u => u.CartBooks)
            //    .HasForeignKey(cb => cb.UserId);


        }
    }
}
