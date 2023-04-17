using API.Models;
using API.ViewModels.Staff;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly ILogger<StaffController> _logger;
        private readonly IMapper _mapper;
        private readonly IStaffService _StaffService;

        public StaffController(ILogger<StaffController> logger, IMapper mapper, IStaffService StaffService)
        {
            _logger = logger;
            _mapper = mapper;
            _StaffService = StaffService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{branchId}")]
        public async Task<IActionResult> GetAllStaffs([FromRoute] string branchId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the Staffs");
                IEnumerable<Staff> staffs = await _StaffService.GetAllStaffAsync(branchId);
                List<StaffDto> StaffDtos = _mapper.Map<List<StaffDto>>(staffs);
                return Ok(StaffDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Staffs Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Staff.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetStaffById")]
        public async Task<IActionResult> GetStaffById([FromQuery] string branchId, [FromQuery] string StaffAccountId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Staff Account with id {StaffAccountId}");
                Staff Staff = await _StaffService.GetStaffByIdAsync(branchId, StaffAccountId);
                if (Staff is null)
                {
                    return NotFound();
                }
                StaffDto StaffDto = _mapper.Map<StaffDto>(Staff);
                return Ok(StaffDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Staff with id {StaffAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Staff Details.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetStaffByName")]
        public async Task<IActionResult> GetStaffByName([FromQuery] string branchId, [FromQuery] string StaffName)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Staff Account with Name {StaffName}");
                Staff Staff = await _StaffService.GetStaffByNameAsync(branchId, StaffName);
                if (Staff is null)
                {
                    return NotFound();
                }
                StaffDto StaffDto = _mapper.Map<StaffDto>(Staff);
                return Ok(StaffDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Staff with Name {StaffName} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Staff Details.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> OpenStaffAccount([FromBody] AddStaffViewModel staffViewModel)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Opening Staff Account");
                Message message = await _StaffService.OpenStaffAccountAsync(staffViewModel.BranchId, staffViewModel.StaffName,
                staffViewModel.StaffPassword, staffViewModel.StaffRole);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Opening a new Staff Account Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating an Account.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<IActionResult> UpdateStaffAccount([FromBody] UpdateStaffViewModel updateStaffViewModel)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Updating Staff with Id {updateStaffViewModel.StaffAccountId}");
                Message message = await _StaffService.UpdateStaffAccountAsync(updateStaffViewModel.BranchId, updateStaffViewModel.StaffAccountId,
                    updateStaffViewModel.StaffName, updateStaffViewModel.StaffPassword, updateStaffViewModel.StaffRole);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating Staff Account with Id {updateStaffViewModel.StaffAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating Staff Account.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("DeleteStaffAccount")]
        public async Task<IActionResult> DeleteStaffAccount([FromQuery] string branchId, [FromQuery] string StaffAccountId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting Staff Account with Id {StaffAccountId}");
                Message message = await _StaffService.DeleteStaffAccountAsync(branchId, StaffAccountId);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting Staff Account with Id {StaffAccountId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the Staff Account.");
            }
        }
    }
}
