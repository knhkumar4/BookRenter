using System.ComponentModel.DataAnnotations;

namespace BookRenterData.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(255)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        // Navigation properties
        public virtual ICollection<CartBook> CartBooks { get; set; } 
    }
}
