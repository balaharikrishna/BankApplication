using API.Models;
using API.ViewModels.HeadManager;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeadManagerController : ControllerBase
    {
        private readonly ILogger<HeadManagerController> _logger;
        private readonly IMapper _mapper;
        private readonly IHeadManagerService _headManagerService;

        public HeadManagerController(ILogger<HeadManagerController> logger, IMapper mapper, IHeadManagerService headManagerService)
        {
            _logger = logger;
            _mapper = mapper;
            _headManagerService = headManagerService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetAllHeadManagers/{branchId}")]
        public async Task<IActionResult> GetAllHeadManagers([FromRoute] string branchId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching all HeadManagers");
                IEnumerable<HeadManager> headManagers = await _headManagerService.GetAllHeadManagersAsync(branchId);
                List<HeadManagerDto> headManagerDtos = _mapper.Map<List<HeadManagerDto>>(headManagers);
                return Ok(headManagerDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the HeadManagers Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the HeadManagers.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetHeadManagerById")]
        public async Task<IActionResult> GetHeadManagerById([FromQuery] string bankId, [FromQuery] string headManagerAccountId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching HeadManager Account with id {headManagerAccountId}");
                HeadManager headManager = await _headManagerService.GetHeadManagerByIdAsync(bankId, headManagerAccountId);
                if (headManager is null)
                {
                    return NotFound();
                }
                HeadManagerDto headManagerDto = _mapper.Map<HeadManagerDto>(headManager);
                return Ok(headManagerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching HeadManager Account with id {headManagerAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the headManager Details.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetHeadManagerByName")]
        public async Task<IActionResult> GetHeadManagerByName([FromQuery] string bankId, [FromQuery] string headManagerName)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching headManager Account with Name {headManagerName}");
                HeadManager headManager = await _headManagerService.GetHeadManagerByNameAsync(bankId, headManagerName);
                if (headManager is null)
                {
                    return NotFound();
                }
                HeadManagerDto headManagerDto = _mapper.Map<HeadManagerDto>(headManager);
                return Ok(headManagerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching headManager Account with Name {headManagerName} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Head Manager Details.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("OpenHeadManagerAccount")]
        public async Task<IActionResult> OpenHeadManagerAccount([FromBody] AddHeadManagerViewModel HeadManagerViewModel)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Creating HeadManager Account");
                Message message = await _headManagerService.OpenHeadManagerAccountAsync(HeadManagerViewModel.BankId, HeadManagerViewModel.HeadManagerName, HeadManagerViewModel.HeadManagerPassword);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Creating HeadManager Account Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating Head Manager Account.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("UpdateHeadManagerAccount")]
        public async Task<IActionResult> UpdateHeadManagerAccount([FromBody] UpdateHeadManagerViewModel updateHeadManagerViewModel)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Updating HeadManager with Account Id {updateHeadManagerViewModel.HeadManagerAccountId}");
                Message message = await _headManagerService.UpdateHeadManagerAccountAsync(updateHeadManagerViewModel.BankId, updateHeadManagerViewModel.HeadManagerAccountId,
                updateHeadManagerViewModel.HeadManagerName,updateHeadManagerViewModel.HeadManagerPassword);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating HeadManager with Account Id {updateHeadManagerViewModel.HeadManagerAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the Head Manager Account.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("DeleteHeadManagerAccount")]
        public async Task<IActionResult> DeleteHeadManagerAccount([FromQuery] string branchId, [FromQuery] string HeadManagerAccountId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting HeadManager Account with Id {HeadManagerAccountId}");
                Message message = await _headManagerService.DeleteHeadManagerAccountAsync(branchId, HeadManagerAccountId);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting HeadManager Account with Id {HeadManagerAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the HeadManager Account.");
            }
        }
    }
}
