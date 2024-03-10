using System.ComponentModel.DataAnnotations;

namespace BookRenterData.Entities
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required, MaxLength(255)]
        public string Author { get; set; }

        [MaxLength(100)]
        public string Genre { get; set; }

        public double Price { get; set; }

        public double? RentPrice { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public virtual Inventory Inventory { get; set; }
    }
}
