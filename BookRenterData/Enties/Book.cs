﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookRenterData.Entity
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public required Author Author { get; set; }

        public ICollection<CartBook>? CartBooks { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }

    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public ICollection<CartBook>? CartBooks { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }

    public class CartBook
    {
        [Key]
        public int CartBookId { get; set; }

        [Required]
        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart? Cart { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public Book? Book { get; set; }

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
