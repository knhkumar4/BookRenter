using BookRenter.Services.Models;
using BookRenterService.Models;

namespace BookRenter.Services
{

    public interface IInventoryService
    {
        Task<InventoryResponse> GetInventoryByBookIdAsync(int bookId);
        Task<IEnumerable<InventoryResponse>> GetAllInventoriesAsync();
        Task<InventoryResponse> AddInventoryAsync(InventoryRequest inventoryRequest);
    }
}

