using Microsoft.AspNetCore.Mvc;
using SowFoodProject.Application.Interfaces.IServices;

namespace SowFoodProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoardService _service;
        public DashBoardController(IServiceManager serviceManager)
        {
            _service = serviceManager.DashBoardService;
        }
        [HttpGet("dashboard-details")]
        public async Task<IActionResult> GetCompanySummary()
        {
            var result = await _service.GetDashBoard();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }
}
