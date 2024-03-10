using BookRenter.Models.Responses;
using BookRenter.Services.Interfaces;
using BookRenter.Services.Models;
using BookRenterData.Entities;
using BookRenterData.UnitOfWork.Interfaces;
using BookRenterService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookRenter.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<InventoryResponse> GetInventoryByBookIdAsync(int bookId)
        {
            var inventory = await _unitOfWork.InventoryRepository.GetInventoryByBookIdAsync(bookId);
            return (InventoryResponse)inventory; // Explicit conversion of Inventory to InventoryResponse
        }

        public async Task<IEnumerable<InventoryResponse>> GetAllInventoriesAsync()
        {
            var inventories = await _unitOfWork.InventoryRepository.GetAllAsync();
            return inventories.Select(inv => (InventoryResponse)inv); // Explicit conversion of each Inventory to InventoryResponse
        }

        public async Task<InventoryResponse> AddInventoryAsync(InventoryRequest inventoryRequest)
        {
            if (inventoryRequest == null)
            {
                throw new ArgumentNullException(nameof(inventoryRequest));
            }

            var existingInventory = await _unitOfWork.InventoryRepository.GetInventoryByBookIdAsync(inventoryRequest.BookId);
            if (existingInventory == null)
            {
                // If inventory does not exist, create a new one
                var newInventory = new Inventory
                {
                    BookId = inventoryRequest.BookId,
                    Quantity = inventoryRequest.Quantity,
                    CreatedDate = DateTime.UtcNow
                };
                await _unitOfWork.InventoryRepository.AddAsync(newInventory);
            }
            else
            {
                // Update the existing inventory
                existingInventory.Quantity = inventoryRequest.Quantity;
                await _unitOfWork.InventoryRepository.UpdateAsync(existingInventory);
            }

            await _unitOfWork.CompleteAsync();

            // Retrieve the updated inventory and return it
            return await GetInventoryByBookIdAsync(inventoryRequest.BookId);
        }
    }
}
