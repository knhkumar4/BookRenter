using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookRenterData.Entities
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Author { get; set; }

        public string Genre { get; set; }

        public double Price { get; set; }

        public double? RentPrice { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public ICollection<CartBook>? CartBooks { get; set; }
        public ICollection<Inventory>? Inventories { get; set; }
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<CartBook>? CartBooks { get; set; }
    }

    public class CartBook
    {
        [Key]
        public int CartBookId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public Book? Book { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }

    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public Book? Book { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
