using Microsoft.AspNetCore.Mvc;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;

namespace SowFoodProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SowFoodCompanyCustomerController : ControllerBase
    {
        private readonly ISowFoodCompanyCustomerService _customerService;

        public SowFoodCompanyCustomerController(IServiceManager serviceManager)
        {
            _customerService = serviceManager.SowFoodCompanyCustomerService;
        }

        [HttpPost("create-customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateSowFoodCompanyCustomerDto dto)
        {
            var result = await _customerService.CreateCustomerAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("delete-customer/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(string customerId)
        {
            var result = await _customerService.DeleteCustomerAsync(customerId);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("get-all-customers")]
        public async Task<IActionResult> GetAllCustomers([FromQuery] PaginationFilter filter, [FromQuery] string companyId, [FromQuery] string? search = null)
        {
            var result = await _customerService.GetAllCustomersAsync(filter, companyId, search);
            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("get-customer-by-id/{customerId}")]
        public async Task<IActionResult> GetCustomerById(string customerId, [FromQuery] string companyId)
        {
            var result = await _customerService.GetCustomerByIdAsync(customerId, companyId);
            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("update-customer")]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateSowFoodCompanyCustomerDto dto)
        {
            var result = await _customerService.UpdateCustomerAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }
    }

}
