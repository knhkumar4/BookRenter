using BookRenter.Services.Models;
using BookRenterService.Models;

namespace BookRenterService.Interfaces
{

    public interface IInventoryService
    {
        Task<InventoryResponse> GetInventoryByBookIdAsync(int bookId);
        Task<IEnumerable<InventoryResponse>> GetAllInventoriesAsync();
        Task<InventoryResponse> AddInventoryAsync(InventoryRequest inventoryRequest);
    }
}

