using API.Models;
using API.ViewModels.Transactions;
using AutoMapper;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        public TransactionController(ILogger<TransactionController> logger, IMapper mapper, ITransactionService transactionService)
        {
            _logger = logger;
            _mapper = mapper;
            _transactionService = transactionService;
        }


        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpGet("{fromCustomerAccountId}")]
        //public async Task<IActionResult> GetAllTransactions([FromRoute] string fromCustomerAccountId)
        //{
        //    try
        //    {
        //        _logger.Log(LogLevel.Information, message: "Fetching All Transactions");
        //        IEnumerable<Transaction> transactions = await _transactionService.GetAllTransactionHistory(fromCustomerAccountId);
        //        List<TransactionDto> transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);
        //        return Ok(transactionDtos);
        //    }
        //    catch (Exception)
        //    {
        //        _logger.Log(LogLevel.Error, message: "Fetching the Transactions Failed");
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An Error occurred while fetching the Transactions.");
        //    }
        //}

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpGet("GetTransactionById")]
        //public async Task<IActionResult> GetTransactionById([FromQuery] string fromCustomerAccountId, [FromQuery] string transactionId)
        //{
        //    try
        //    {
        //        _logger.Log(LogLevel.Information, message: "Fetching the Transaction");
        //        Transaction transaction = await _transactionService.GetTransactionById(fromCustomerAccountId, transactionId);
        //        TransactionDto transactionDto = _mapper.Map<TransactionDto>(transaction);
        //        return Ok(transactionDto);
        //    }
        //    catch (Exception)
        //    {
        //        _logger.Log(LogLevel.Error, message: "Fetching the Transaction Failed");
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An Error occurred while fetching the Transaction.");
        //    }
        //}

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpPost]
        //public async Task<IActionResult> AddCustomerTransaction([FromBody] AddCustomerTransactionViewModel addCustomerTransactionViewModel)
        //{
        //    try
        //    {
        //        _logger.Log(LogLevel.Information, message: $"Adding new Transaction");
        //        Message message = await _transactionService.TransactionHistoryAsync(addCustomerTransactionViewModel.FromCustomerBankId, addCustomerTransactionViewModel.FromCustomerBranchId,
        //        addCustomerTransactionViewModel.FromCustomerAccountId, addCustomerTransactionViewModel.Debit, addCustomerTransactionViewModel.Credit,
        //        addCustomerTransactionViewModel.Balance, addCustomerTransactionViewModel.TransactionType);
        //        return Ok(message.ResultMessage);
        //    }
        //    catch (Exception)
        //    {
        //        _logger.Log(LogLevel.Error, message: $"Adding new Transaction Failed");
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Adding the Transaction.");
        //    }
        //}

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpPost]
        //public async Task<IActionResult> AddFromAndToCustomerTransaction([FromBody] AddFromAndToCustomerTransactionViewModel addFromAndToCustomerTransactionViewModel)
        //{
        //    try
        //    {
        //        _logger.Log(LogLevel.Information, message: $"Adding new Transaction");
        //        Message message = await _transactionService.TransactionHistoryFromAndToAsync(addFromAndToCustomerTransactionViewModel.FromCustomerBankId,
        //        addFromAndToCustomerTransactionViewModel.FromCustomerBranchId, addFromAndToCustomerTransactionViewModel.FromCustomerAccountId,
        //        addFromAndToCustomerTransactionViewModel.ToCustomerBankId, addFromAndToCustomerTransactionViewModel.ToCustomerBranchId,
        //        addFromAndToCustomerTransactionViewModel.ToCustomerAccountId, addFromAndToCustomerTransactionViewModel.Debit,
        //        addFromAndToCustomerTransactionViewModel.Credit, addFromAndToCustomerTransactionViewModel.Balance, addFromAndToCustomerTransactionViewModel.ToCustomerBalance,
        //        addFromAndToCustomerTransactionViewModel.TransactionType);
        //        return Ok(message.ResultMessage);
        //    }
        //    catch (Exception)
        //    {
        //        _logger.Log(LogLevel.Error, message: $"Adding new Transaction Failed");
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Adding the Transaction.");
        //    }
        //}

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[HttpPut]
        //public async Task<IActionResult> RevertTransaction([FromBody] RevertTransactionViewModel revertTransactionViewModel)
        //{
        //    try
        //    {
        //        _logger.Log(LogLevel.Information, message: $"Reverting Transaction.");
        //        Message message = await _transactionService.RevertTransactionAsync(revertTransactionViewModel.TransactionId, revertTransactionViewModel.FromCustomerBankId,
        //        revertTransactionViewModel.FromCustomerBranchId, revertTransactionViewModel.FromCustomerAccountId, revertTransactionViewModel.ToCustomerBankId,
        //        revertTransactionViewModel.ToCustomerBranchId, revertTransactionViewModel.ToCustomerAccountId);
        //        return Ok(message.ResultMessage);
        //    }
        //    catch (Exception)
        //    {
        //        _logger.Log(LogLevel.Error, message: $"Reverting Transaction Failed");
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while Reverting Transaction.");
        //    }
        //}
    }
}
