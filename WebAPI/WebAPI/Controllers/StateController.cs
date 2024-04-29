using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DataAcess;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly StateDataAccess _stateDataAccess;

        public StateController(StateDataAccess stateDataAccess)
        {
            _stateDataAccess = stateDataAccess;
        }

        [HttpGet]
        public IActionResult Getcitybyfunction()
        {
            try
            {
                var std = _stateDataAccess.GetState();
                return Ok(std);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
