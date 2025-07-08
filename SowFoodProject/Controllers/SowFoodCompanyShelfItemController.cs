using Microsoft.AspNetCore.Mvc;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;

namespace SowFoodProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SowFoodCompanyShelfItemController : ControllerBase
    {
        private readonly ISowFoodCompanyShelfItemService _shelfItemService;

        public SowFoodCompanyShelfItemController(IServiceManager serviceManager)
        {
            _shelfItemService = serviceManager.SowFoodCompanyShelfItemService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateShelfItem([FromBody] CreateShelfItemDto dto)
        {
            var result = await _shelfItemService.CreateShelfItemAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("delete-shelf-item/{id}")]
        public async Task<IActionResult> DeleteShelfItem(string id)
        {
            var result = await _shelfItemService.DeleteShelfItemAsync(id);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("get-all-shelf-items")]
        public async Task<IActionResult> GetAllShelfItems([FromQuery] PaginationFilter filter, [FromQuery] string companyId, [FromQuery] string? search)
        {
            var result = await _shelfItemService.GetAllShelfItemsAsync(filter, companyId, search);
            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("get-shelf-item-by-id/{id}")]
        public async Task<IActionResult> GetShelfItemById(string id, [FromQuery] string companyId)
        {
            var result = await _shelfItemService.GetShelfItemByIdAsync(id, companyId);
            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("update-shelf-item")]
        public async Task<IActionResult> UpdateShelfItem([FromBody] UpdateShelfItemDto dto)
        {
            var result = await _shelfItemService.UpdateShelfItemAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }
    }

}
