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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{bankId}")]
        public async Task<ActionResult<List<HeadManagerDto>>> GetAllHeadManagers([FromRoute] string bankId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching all HeadManagers");
                IEnumerable<HeadManager> headManagers = await _headManagerService.GetAllHeadManagersAsync(bankId);
                List<HeadManagerDto> headManagerDtos = _mapper.Map<List<HeadManagerDto>>(headManagers);
                if(headManagerDtos is not null)
                {
                    return Ok(headManagerDtos);
                }
                else
                {
                    return NotFound("No HeadManagers Available");
                }
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the HeadManagers Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the HeadManagers.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{bankId}/{headManagerAccountId}")]
        public async Task<ActionResult<HeadManagerDto>> GetHeadManagerById([FromRoute] string bankId, [FromRoute] string headManagerAccountId)
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetHeadManagerByName")]
        public async Task<ActionResult<HeadManagerDto>> GetHeadManagerByName([FromQuery] string bankId, [FromQuery] string headManagerName)
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

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("OpenHeadManagerAccount")]
        public async Task<ActionResult<Message>> OpenHeadManagerAccount([FromBody] AddHeadManagerViewModel HeadManagerViewModel)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("UpdateHeadManagerAccount")]
        public async Task<ActionResult<Message>> UpdateHeadManagerAccount([FromBody] UpdateHeadManagerViewModel updateHeadManagerViewModel)
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("DeleteHeadManagerAccount")]
        public async Task<ActionResult<Message>> DeleteHeadManagerAccount([FromQuery] string branchId, [FromQuery] string HeadManagerAccountId)
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
