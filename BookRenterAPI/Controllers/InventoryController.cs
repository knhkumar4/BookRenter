using BookRenter.Services.Models;
using BookRenterService.FluentValidator;
using BookRenterService.Interfaces;
using BookRenterService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookRenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(typeof(InventoryResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<InventoryResponse>> GetInventoryByBookId(int bookId)
        {
            var inventory = await _inventoryService.GetInventoryByBookIdAsync(bookId);
            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InventoryResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<InventoryResponse>>> GetAllInventories()
        {
            var inventories = await _inventoryService.GetAllInventoriesAsync();
            return Ok(inventories);
        }

        [HttpPost]
        [ProducesResponseType(typeof(InventoryResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<InventoryResponse>> AddInventory(InventoryRequest inventoryRequest)
        {
            // Validate the request
            var validationResult = await new InventoryRequestValidator().ValidateAsync(inventoryRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.First().ErrorMessage);
            }

            try
            {
                var addedInventory = await _inventoryService.AddInventoryAsync(inventoryRequest);
                return CreatedAtAction(nameof(GetInventoryByBookId), new { bookId = addedInventory.BookId }, addedInventory);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while adding the inventory.");
            }
        }
    }
}
