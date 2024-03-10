using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookRenterData.Entities
{
    public class CartBook
    {
        [Key]
        public int CartBookId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
