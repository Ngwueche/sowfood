using Microsoft.AspNetCore.Mvc;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;

namespace SowFoodProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SowFoodCompanySalesRecordController : ControllerBase
    {
        private readonly ISowFoodCompanySalesRecordService _salesRecordService;

        public SowFoodCompanySalesRecordController(IServiceManager serviceManager)
        {
            _salesRecordService = serviceManager.SowFoodCompanySalesRecordService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSalesRecord([FromBody] CreateSalesRecordDto dto)
        {
            var result = await _salesRecordService.CreateSalesRecordAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("delete-sales-record/{id}")]
        public async Task<IActionResult> DeleteSalesRecord(string id)
        {
            var result = await _salesRecordService.DeleteSalesRecordAsync(id);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("get-all-sales-records")]
        public async Task<IActionResult> GetAllSalesRecords([FromQuery] PaginationFilter filter, [FromQuery] string companyId)
        {
            var result = await _salesRecordService.GetAllSalesRecordAsync(filter, companyId);
            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("get-sales-record-by-id/{id}")]
        public async Task<IActionResult> GetSalesRecordById(string id, [FromQuery] string companyId)
        {
            var result = await _salesRecordService.GetSalesRecordByIdAsync(id, companyId);
            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("update-sales-record")]
        public async Task<IActionResult> UpdateSalesRecord([FromBody] UpdateSalesRecordDto dto)
        {
            var result = await _salesRecordService.UpdateSalesRecordAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }
    }

}
