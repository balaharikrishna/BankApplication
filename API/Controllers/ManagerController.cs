using API.Models;
using API.ViewModels.manager;
using API.ViewModels.Manager;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly ILogger<ManagerController> _logger;
        private readonly IMapper _mapper;
        private readonly IManagerService _managerService;

        public ManagerController(ILogger<ManagerController> logger, IMapper mapper, IManagerService managerService)
        {
            _logger = logger;
            _mapper = mapper;
            _managerService = managerService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{branchId}")]
        public async Task<IActionResult> GetAllManagers([FromRoute] string branchId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the Managers");
                IEnumerable<Manager> Managers = await _managerService.GetAllManagersAsync(branchId);
                List<ManagerDto> managerDtos = _mapper.Map<List<ManagerDto>>(Managers);
                return Ok(managerDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Managers Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the branches.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetManagerById")]
        public async Task<IActionResult> GetManagerById([FromQuery] string branchId, [FromQuery] string managerAccountId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching manager Account with id {managerAccountId}");
                Manager manager = await _managerService.GetManagerByIdAsync(branchId,managerAccountId);
                if (manager is null)
                {
                    return NotFound();
                }
                ManagerDto managerDto = _mapper.Map<ManagerDto>(manager);
                return Ok(managerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching manager with id {managerAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the manager Details.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetManagerByName")]
        public async Task<IActionResult> GetManagerByName([FromQuery] string branchId, [FromQuery] string managerName)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Manager Account with Name {managerName}");
                Manager manager = await _managerService.GetManagerByNameAsync(branchId, managerName);
                if (manager is null)
                {
                    return NotFound();
                }
                ManagerDto managerDto = _mapper.Map<ManagerDto>(manager);
                return Ok(managerDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching manager with Name {managerName} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the manager Details.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> OpenManagerAccount([FromBody] AddManagerViewModel ManagerViewModel)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Creating manager Account");
                Message message = await _Managerservice.OpenmanagerAccountAsync(managerViewModel.BranchId, managerViewModel.managerName,
                managerViewModel.managerPassword, managerViewModel.managerPhoneNumber, managerViewModel.managerEmailId, managerViewModel.managerAccountType,
                managerViewModel.managerAddress, managerViewModel.managerDateOfBirth, managerViewModel.managerGender);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Creating a new manager Account Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating an Account.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<IActionResult> UpdateManagerAccount([FromBody] UpdatemanagerViewModel updatemanagerViewModel)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Updating manager with Id {updatemanagerViewModel.managerAccountId}");
                Message message = await _Managerservice.UpdatemanagerAccountAsync(updatemanagerViewModel.BranchId, updatemanagerViewModel.managerAccountId, updatemanagerViewModel.managerName,
                updatemanagerViewModel.managerPassword, updatemanagerViewModel.managerPhoneNumber, updatemanagerViewModel.managerEmailId, updatemanagerViewModel.managerAccountType,
                updatemanagerViewModel.managerAddress, updatemanagerViewModel.managerDateOfBirth, updatemanagerViewModel.managerGender);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating Branch with Id {updatemanagerViewModel.managerAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the Branch.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("DeleteManagerAccount")]
        public async Task<IActionResult> DeleteManagerAccount([FromQuery] string branchId, [FromQuery] string managerAccountId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting manager Account with Id {managerAccountId}");
                Message message = await _Managerservice.DeletemanagerAccountAsync(branchId, managerAccountId);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting manager Account with Id {managerAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the manager Account.");
            }
        }
    }
}
