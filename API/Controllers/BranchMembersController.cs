using AutoMapper;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "BranchMembersOnly")]
    public class BranchMembersController : ControllerBase
    {
        private readonly ILogger<BranchController> _logger;
        private readonly IBranchMembersService _branchMembersService;

        public BranchMembersController(ILogger<BranchController> logger, IBranchMembersService branchMembersService)
        {
            _logger = logger;
            _branchMembersService = branchMembersService;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("branchId/{id}")]
        public async Task<ActionResult<List<string>>> GetAllBranchMemberNames([FromRoute] string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the Branches");
                IEnumerable<string> names = await _branchMembersService.GetAllBranchesAsync(id);
                if (names is null || !names.Any())
                {
                    return NotFound("Memebers Not Found.");
                }

                return Ok(names);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Branch Memebers Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Branch Memebers.");
            }
        }
    }
}
