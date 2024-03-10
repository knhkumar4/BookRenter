
using BookRenterData.Entities;

namespace BookRenter.Services.Models
{
    public class InventoryResponse
    {
        public int InventoryId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public string BookTitle { get; set; } // Include book title in the response

        // Operator to map Inventory entity to InventoryResponse
        public static explicit operator InventoryResponse(Inventory inventory)
        {
            if (inventory == null)
                return null;

            return new InventoryResponse
            {
                InventoryId = inventory.InventoryId,
                BookId = inventory.BookId,
                Quantity = inventory.Quantity,
                BookTitle = inventory.Book?.Title // Access book title from the related Book entity
            };
        }
    }
}
