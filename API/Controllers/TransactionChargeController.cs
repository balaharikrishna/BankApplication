using API.Models;
using API.ViewModels.TransactionCharges;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionChargeController : ControllerBase
    {
        private readonly ILogger<TransactionChargeController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionChargeService _transactionChargeService;

        public TransactionChargeController(ILogger<TransactionChargeController> logger, IMapper mapper, ITransactionChargeService transactionChargeService)
        {
            _logger = logger;
            _mapper = mapper;
            _transactionChargeService = transactionChargeService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetTransactionCharges/{branchId}")]
        public async Task<IActionResult> GetTransactionCharges([FromRoute] string branchId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: "Fetching Transaction Charges");
                TransactionCharges transactionCharges = await _transactionChargeService.GetTransactionCharges(branchId);
                TransactionChargesDto transactionChargesDto = _mapper.Map<TransactionChargesDto>(transactionCharges);
                return Ok(transactionChargesDto);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: "Fetching the Transaction Charges Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the Transaction Charges.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("AddTransactionCharges")]
        public async Task<IActionResult> AddTransactionCharges([FromBody] TransactionChargesViewModel transactionChargesViewModel)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Adding new Transaction Charges");
                Message message = await _transactionChargeService.AddTransactionChargesAsync(transactionChargesViewModel.BranchId, transactionChargesViewModel.RtgsSameBank,
                transactionChargesViewModel.RtgsOtherBank, transactionChargesViewModel.ImpsSameBank, transactionChargesViewModel.ImpsOtherBank);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Adding new Transaction Charges Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the Transaction Charges.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("UpdateTransactionCharges")]
        public async Task<IActionResult> UpdateTransactionCharges([FromBody] TransactionChargesViewModel transactionChargesViewModel)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Updating Transaction Charges");
                Message message = await _transactionChargeService.UpdateTransactionChargesAsync(transactionChargesViewModel.BranchId, transactionChargesViewModel.RtgsSameBank,
                transactionChargesViewModel.RtgsOtherBank, transactionChargesViewModel.ImpsSameBank, transactionChargesViewModel.ImpsOtherBank);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Updating Transaction Charges Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the Transaction Charges.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("DeleteTransactionCharges/{branchId}")]
        public async Task<IActionResult> DeleteTransactionCharges([FromRoute] string branchId)
        {
            try
            {
                _logger.Log(LogLevel.Information, message: $"Deleting Transaction Charges");
                Message message = await _transactionChargeService.DeleteTransactionChargesAsync(branchId);
                return Ok(message.ResultMessage);
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, message: $"Deleting Transaction Charges Failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Deleting the Transaction Charges.");
            }
        }
    }
}
