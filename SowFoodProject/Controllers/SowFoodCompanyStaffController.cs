using Microsoft.AspNetCore.Mvc;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;

namespace SowFoodProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SowFoodCompanyStaffController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public SowFoodCompanyStaffController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateStaff([FromBody] CreateSowFoodCompanyStaffDto dto)
        {
            var result = await _serviceManager.SowFoodCompanyStaffService.CreateAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById([FromQuery] string staffId, [FromQuery] string companyId)
        {
            var result = await _serviceManager.SowFoodCompanyStaffService.GetByIdAsync(staffId, companyId);
            return result.IsSuccessful ? Ok(result) : NotFound(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllStaff([FromQuery] PaginationFilter filter, [FromQuery] string companyId, string? searchString)
        {
            var result = await _serviceManager.SowFoodCompanyStaffService.GetAllStaffAsync(filter, companyId, searchString);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateStaff([FromBody] UpdateSowFoodCompanyStaffDto dto)
        {
            var result = await _serviceManager.SowFoodCompanyStaffService.UpdateStaffAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteStaff([FromQuery] string staffId, [FromQuery] string companyId)
        {
            var result = await _serviceManager.SowFoodCompanyStaffService.DeleteStaffAsync(staffId, companyId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }

}
