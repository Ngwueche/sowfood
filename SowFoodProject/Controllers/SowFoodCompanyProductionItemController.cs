using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;

namespace SowFoodProject.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SowFoodCompanyProductionItemController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public SowFoodCompanyProductionItemController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateProductionItemDto dto)
        {
            var result = await _serviceManager.SowFoodCompanyProductionItemService.CreateProductionItemAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter, [FromQuery] string companyId, [FromQuery] string? search = null)
        {
            var result = await _serviceManager.SowFoodCompanyProductionItemService.GetAllProductionItemAsync(filter, companyId, search);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, [FromQuery] string companyId)
        {
            var result = await _serviceManager.SowFoodCompanyProductionItemService.GetProductionItemByIdAsync(id, companyId);
            return result.IsSuccessful ? Ok(result) : NotFound(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateProductionItemDto dto)
        {
            var result = await _serviceManager.SowFoodCompanyProductionItemService.UpdateProductionItemAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _serviceManager.SowFoodCompanyProductionItemService.DeleteProductionItemAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }

}
