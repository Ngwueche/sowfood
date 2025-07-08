using Microsoft.AspNetCore.Mvc;
using SowFoodProject.Application.DTOs;
using SowFoodProject.Application.Interfaces.IServices;

namespace SowFoodProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SowFoodCompanyStaffAttendanceController : ControllerBase
    {
        private readonly ISowFoodCompanyStaffAttendanceService _attendanceService;

        public SowFoodCompanyStaffAttendanceController(IServiceManager serviceManager)
        {
            _attendanceService = serviceManager.SowFoodCompanyStaffAttendanceService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAttendance([FromBody] CreateStaffAttendanceDto dto)
        {
            var result = await _attendanceService.CreateAttendanceAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("delete-attendance/{id}")]
        public async Task<IActionResult> DeleteAttendance(string id)
        {
            var result = await _attendanceService.DeleteAttendanceAsync(id);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("get-all-attendance")]
        public async Task<IActionResult> GetAllAttendances([FromQuery] PaginationFilter filter, [FromQuery] string? staffId = null)
        {
            var result = await _attendanceService.GetAllAttendancesAsync(filter, staffId);
            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("get-attendance-by-id/{id}")]
        public async Task<IActionResult> GetAttendanceById(string id)
        {
            var result = await _attendanceService.GetAttendanceByIdAsync(id);
            if (!result.IsSuccessful)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("update-attendance")]
        public async Task<IActionResult> UpdateAttendance([FromBody] UpdateStaffAttendanceDto dto)
        {
            var result = await _attendanceService.UpdateAttendanceAsync(dto);
            if (!result.IsSuccessful)
                return BadRequest(result);

            return Ok(result);
        }
    }

}
