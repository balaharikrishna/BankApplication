using API.Models;
using API.ViewModels.Bank;
using API.ViewModels.Branch;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly ILogger<BranchController> _logger;
        private readonly IMapper _mapper;
        private readonly IBranchService _branchService;

        public BranchController(ILogger<BranchController> logger, IMapper mapper, IBranchService branchService)
        {
            _logger = logger;
            _mapper = mapper;
            _branchService = branchService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetAllBranches/{bankId}")]
        public async Task<IActionResult> GetAllBranches([FromRoute] string bankId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the Branches");
                IEnumerable<Branch> branches = await _branchService.GetAllBranchesAsync(bankId);
                List<BranchDto> branchDtos = _mapper.Map<List<BranchDto>>(branches);
                return Ok(branchDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Branches Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the branches.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetBranchById/{branchId}")]
        public async Task<IActionResult> GetBranchById([FromRoute] string branchId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Branch with id {branchId}");
                Branch branch = await _branchService.GetBranchByIdAsync(branchId);
                if (branch is null)
                {
                    return NotFound();
                }
                BranchDto branchDto = _mapper.Map<BranchDto>(branch);
                return Ok(branchDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Branch with id {branchId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Branch.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetBranchByName/{branchName}")]
        public async Task<IActionResult> GetBranchByName([FromRoute] string branchName)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching Branch with Name {branchName}");
                Branch branch = await _branchService.GetBranchByNameAsync(branchName);
                if (branch is null)
                {
                    return NotFound();
                }
                BranchDto branchDto = _mapper.Map<BranchDto>(branch);
                return Ok(branchDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching Branch with Name {branchName} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Fetching the Branch.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("CreateBranch")]
        public async Task<IActionResult> CreateBranch([FromBody] AddBranchViewModel addBranchViewModel)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Creating a new Branch");
                Message message = await _branchService.CreateBranchAsync(addBranchViewModel.BankId, addBranchViewModel.BranchName, addBranchViewModel.BranchPhoneNumber, addBranchViewModel.BranchAddress);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Creating a new Branch Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the Branch.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("UpdateBranch")]
        public async Task<IActionResult> UpdateBranch([FromBody] UpdateBranchViewModel updateBranchViewModel)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Updating Branch with Id {updateBranchViewModel.BranchId}");
                Message message = await _branchService.UpdateBranchAsync(updateBranchViewModel.BranchId, updateBranchViewModel.BranchName, updateBranchViewModel.BranchPhoneNumber, updateBranchViewModel.BranchAddress);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating Branch with Id {updateBranchViewModel.BranchId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the Branch.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("DeleteBranch/{branchId}")]
        public async Task<IActionResult> DeleteBranch([FromRoute] string branchId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting Branch with Id {branchId}");
                Message message = await _branchService.DeleteBranchAsync(branchId);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting Branch with Id {branchId} Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the Branch.");
            }
        }
    }
}
