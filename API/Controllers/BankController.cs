using API.Models;
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

        public BankController(ILogger<BankController> logger,IMapper mapper,IBankService bankService) { 
           _logger = logger;
            _mapper = mapper;
            _bankService = bankService;
        }

        [HttpGet]
        public IActionResult GetAllBanks()
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching the banks");
                var banks = _bankService.GetAllBanksAsync();
                var bankDtos = _mapper.Map<List<BankDto>>(banks);
                return Ok(bankDtos);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the banks failed");
                return StatusCode(500, "An error occurred while fetching the banks.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBank(string id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Fetching bank with id {id}");
                var bank = _bankService.GetBankById(id);
                if (bank == null)
                {
                    return NotFound();
                }
                var bankDto = _mapper.Map<BankDto>(bank);
                return Ok(bankDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Fetching bank with id {id} failed");
                return StatusCode(500, "An error occurred while fetching the bank.");
            }
        }

        [HttpPost]
        public IActionResult CreateBank(string bankName)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Creating a new bank");
              var data =  _bankService.CreateBank(bankName);
                var bankDto = _mapper.Map<BankDto>(bank);
                return Ok(bankDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Creating a new bank failed");
                return StatusCode(500, "An error occurred while creating the bank.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBank(string id, BankDto bankUpdateDto)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Updating bank with id {id}");
                var bank = _bankService.GetBankById(id);
                if (bank == null)
                {
                    return NotFound();
                }
                _mapper.Map(bankUpdateDto, bank);
                _bankService.UpdateBank(bank);
                var bankDto = _mapper.Map<BankDto>(bank);
                return Ok(bankDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating bank with id {id} failed");
                return StatusCode(500, "An error occurred while updating the bank.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBank(int id)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting bank with id {id}");
                var bank = _bankService.GetBankById(id);
                if (bank == null)
                {
                    return NotFound();
                }
                _bankService.DeleteBank(bank);
                return NoContent();
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting bank with id {id} failed");
                return StatusCode(500, "An error occurred while deleting the bank.");
            }
        }
    }
}
