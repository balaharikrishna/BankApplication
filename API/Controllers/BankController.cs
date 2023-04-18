using API.Models;
using API.ViewModels.Bank;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly ILogger<BankController> _logger;
        private readonly IMapper _mapper;
        private readonly IBankService _bankService;

        public BankController(ILogger<BankController> logger, IMapper mapper, IBankService bankService)
        {
            _logger = logger;
            _mapper = mapper;
            _bankService = bankService;
        }

        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetAllBanks")]
        public async Task<IActionResult> GetAllBanks()
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the banks");
                IEnumerable<Bank> banks = await _bankService.GetAllBanksAsync();
                List<BankDto> bankDtos = _mapper.Map<List<BankDto>>(banks);
                return Ok(bankDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the banks failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the banks.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetBankById/{bankId}")]
        public async Task<IActionResult> GetBankById([FromRoute] string bankId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching bank with id {bankId}");
                Bank bank = await _bankService.GetBankByIdAsync(bankId);
                if (bank is null)
                {
                    return NotFound();
                }
                BankDto bankDto = _mapper.Map<BankDto>(bank);
                return Ok(bankDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching bank with id {bankId} failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the bank.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetBankByName/{bankName}")]
        public async Task<IActionResult> GetBankByName([FromRoute] string bankName)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching bank with name {bankName}");
                Bank bank = await _bankService.GetBankByNameAsync(bankName);
                if (bank is null)
                {
                    return NotFound();
                }
                BankDto bankDto = _mapper.Map<BankDto>(bank);
                return Ok(bankDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching bank with name {bankName} failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the bank.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("CreateBank")]
        public async Task<IActionResult> CreateBank([FromBody] AddBankViewModel addBankViewModel)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Creating a new bank");
                Message message = await _bankService.CreateBankAsync(addBankViewModel.BankName);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Creating a new bank failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the bank.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("UpdateBank")]
        public async Task<IActionResult> UpdateBank([FromBody] UpdateBankViewModel updateBankViewModel)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Updating bank with id {updateBankViewModel.BankId}");
                Message message = await _bankService.UpdateBankAsync(updateBankViewModel.BankId, updateBankViewModel.BankName);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating bank with id {updateBankViewModel.BankId} failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the bank.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("DeleteBank/{bankId}")]
        public async Task<IActionResult> DeleteBank([FromRoute] string bankId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting bank with id {bankId}");
                Message message = await _bankService.DeleteBankAsync(bankId);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting bank with id {bankId} failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the bank.");
            }
        }
    }
}
