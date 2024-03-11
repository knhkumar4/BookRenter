using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookRenterData.Entities
{
    public class Inventory
    {
        [Key, ForeignKey("Book")]
        public int InventoryId { get; set; }
        [Required]
        public int BookId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation property
        public virtual Book Book { get; set; }
    }
}
