using Microsoft.AspNetCore.Mvc;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;

namespace SowFoodProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SowFoodCompanyStaffAppraiserController : ControllerBase
    {
        private readonly ISowFoodCompanyStaffAppraiserService _appraiserService;

        public SowFoodCompanyStaffAppraiserController(IServiceManager serviceManager)
        {
            _appraiserService = serviceManager.SowFoodCompanyStaffAppraiserService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAppraiser([FromBody] CreateStaffAppraiserDto dto)
        {
            var result = await _appraiserService.CreateAppraiserAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("delete-appraiser/{id}")]
        public async Task<IActionResult> DeleteAppraiser(string id)
        {
            var result = await _appraiserService.DeleteAppraiserAsync(id);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("get-all-appraiser")]
        public async Task<IActionResult> GetAllAppraisers([FromQuery] PaginationFilter filter, [FromQuery] string staffId, [FromQuery] string? search)
        {
            var result = await _appraiserService.GetAllAppraisersAsync(filter, staffId, search);
            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("get-appraiser-by-id/{id}")]
        public async Task<IActionResult> GetAppraiserById(string id)
        {
            var result = await _appraiserService.GetAppraiserByIdAsync(id);
            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("update-appraiser")]
        public async Task<IActionResult> UpdateAppraiser([FromBody] UpdateStaffAppraiserDto dto)
        {
            var result = await _appraiserService.UpdateAppraiserAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }
    }

}
