using API.Models;
using API.ViewModels.ReserveBankManager;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReserveBankManagerController : ControllerBase
    {
        private readonly ILogger<ReserveBankManagerController> _logger;
        private readonly IMapper _mapper;
        private readonly IReserveBankManagerService _reserveBankManagerService;

        public ReserveBankManagerController(ILogger<ReserveBankManagerController> logger, IMapper mapper, IReserveBankManagerService reserveBankManagerService)
        {
            _logger = logger;
            _mapper = mapper;
            _reserveBankManagerService = reserveBankManagerService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetAllReserveBankManagers()
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the Reserve Bank Managers");
                IEnumerable<ReserveBankManager> reserveBankManagers = await _reserveBankManagerService.GetAllReserveBankManagersAsync();
                List<ReserveBankManagerDto> reserveBankManagerDtos = _mapper.Map<List<ReserveBankManagerDto>>(reserveBankManagers);
                return Ok(reserveBankManagerDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Reserve Bank Managers Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Reserve Bank Managers.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{reserveBankManagerAccountId}")]
        public async Task<IActionResult> GetReserveBankManagerById([FromRoute] string reserveBankManagerAccountId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Reserve Bank Manager Account with Id {reserveBankManagerAccountId}");
                ReserveBankManager reserveBankManager = await _reserveBankManagerService.GetReserveBankManagerByIdAsync(reserveBankManagerAccountId);
                if (reserveBankManager is null)
                {
                    return NotFound();
                }
                ReserveBankManagerDto managerDto = _mapper.Map<ReserveBankManagerDto>(reserveBankManager);
                return Ok(managerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Reserve Bank Manager with id {reserveBankManagerAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Reserve Bank Manager Details.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("reserveBankManagerName")]
        public async Task<IActionResult> GetReserveBankManagerByName([FromRoute] string reserveBankManagerName)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Reserve Bank Manager Account with Name {reserveBankManagerName}");
                ReserveBankManager reserveBankManager = await _reserveBankManagerService.GetReserveBankManagerByNameAsync(reserveBankManagerName);
                if (reserveBankManager is null)
                {
                    return NotFound();
                }
                ReserveBankManagerDto reserveBankManagerDto = _mapper.Map<ReserveBankManagerDto>(reserveBankManager);
                return Ok(reserveBankManagerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Reserve Bank Manager with Name {reserveBankManagerName} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Reserve Bank Manager Details.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> OpenReserveBankManagerAccount([FromBody] AddReserveBankManagerAccountViewModel addReserveBankManagerAccountViewModel)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Opening Reserve Bank Manager Account");
                Message message = await _reserveBankManagerService.OpenReserveBankManagerAccountAsync(addReserveBankManagerAccountViewModel.ReserveBankManagerName,
                addReserveBankManagerAccountViewModel.ReserveBankManagerPassword);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Opening a new Reserve Bank Manager Account Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating Reserve Bank Manager Account.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<IActionResult> UpdateReserveBankManagerAccount([FromBody] UpdateReserveBankManagerAccount updateReserveBankManagerAccount)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Updating Reserve Bank Manager Account with Id {updateReserveBankManagerAccount.ReserveBankManagerAccountId}");
                Message message = await _reserveBankManagerService.UpdateReserveBankManagerAccountAsync(updateReserveBankManagerAccount.ReserveBankManagerAccountId,
                    updateReserveBankManagerAccount.ReserveBankManagerName, updateReserveBankManagerAccount.ReserveBankManagerPassword);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating Reserve Bank Manager Account Account with Id {updateReserveBankManagerAccount.ReserveBankManagerAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating Reserve Bank Manager Account Account.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{reserveBankManagerAccountId}")]
        public async Task<IActionResult> DeleteReserveBankManagerAccount([FromRoute] string reserveBankManagerAccountId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting Reserve Bank Manager Account Account with Id {reserveBankManagerAccountId}");
                Message message = await _reserveBankManagerService.DeleteReserveBankManagerAccountAsync(reserveBankManagerAccountId);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting Reserve Bank Manager Account Account with Id {reserveBankManagerAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the Reserve Bank Manager Account Account.");
            }
        }
    }
}

