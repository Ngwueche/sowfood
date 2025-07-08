using Microsoft.AspNetCore.Mvc;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;
namespace SowFoodProject.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminCompanyController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AdminCompanyController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("create-company")]
        public async Task<IActionResult> CreateCompany([FromBody] CreateSowFoodCompanyDto dto)
        {
            var result = await _serviceManager.AdminSowFoodCompanyService.CreateAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("update-company")]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateSowFoodCompanyDto dto)
        {
            var result = await _serviceManager.AdminSowFoodCompanyService.UpdateAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("get-all-company")]
        public async Task<IActionResult> GetAllCompanies()
        {
            var result = await _serviceManager.AdminSowFoodCompanyService.GetAllCompaniesAsync();
            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("get-company-by-id/{id}")]
        public async Task<IActionResult> GetCompanyById(string id)
        {
            var result = await _serviceManager.AdminSowFoodCompanyService.GetByIdAsync(id);
            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPatch("deactivate-company/{id}")]
        public async Task<IActionResult> DeactivateCompany(string id)
        {
            var result = await _serviceManager.AdminSowFoodCompanyService.DeactivateAsync(id);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("delete-company{id}")]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            var result = await _serviceManager.AdminSowFoodCompanyService.DeleteAsync(id);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("create-company-admin")]
        public async Task<IActionResult> CreateCompanyAdmin(CreateSowFoodCompanyStaffDto dto)
        {
            var result = await _serviceManager.AdminSowFoodCompanyService.CreateCompanyAdminAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }
    }

}