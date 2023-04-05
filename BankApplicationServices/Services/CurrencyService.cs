using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IBankService _bankService;
        private readonly IFileService _fileService;
        List<Bank> banks;
        public CurrencyService(IFileService fileService, IBankService bankService)
        {
            _bankService = bankService;
            _fileService = fileService;
            banks = new List<Bank>();
        }

        public Message AddCurrency(string bankId, string currencyCode, decimal exchangeRate)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                Currency currency = new()
                {
                    ExchangeRate = exchangeRate,
                    CurrencyCode = currencyCode
                };
                int bankIndex = banks.FindIndex(bk => bk.BankId.Equals(bankId));
                List<Currency> currencies = banks[bankIndex].Currency;
                currencies ??= new List<Currency>();
                currencies.Add(currency);
                banks[bankIndex].Currency = currencies;
                _fileService.WriteFile(banks);
                message.Result = true;
                message.ResultMessage = $"Added Currency Code:{currencyCode} with Exchange Rate:{exchangeRate}";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }

        public Message UpdateCurrency(string bankId, string currencyCode, decimal exchangeRate)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<Currency> currencies = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Currency;
                var currency = currencies.Find(ck => ck.CurrencyCode.Equals(currencyCode));
                if (currency is not null)
                {
                    currency.ExchangeRate = exchangeRate;
                    message.Result = true;
                    message.ResultMessage = $"Currency Code :{currencyCode} updated with Exchange Rate :{exchangeRate}";
                    _fileService.WriteFile(banks);
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Currency Code :{currencyCode} not Found";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }
        public Message DeleteCurrency(string bankId, string currencyCode)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<Currency> currencies = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Currency;
                var currency = currencies.Find(ck => ck.CurrencyCode.Equals(currencyCode));
                if (currency is not null)
                {
                    currency.IsActive = 0;
                    message.Result = true;
                    message.ResultMessage = $"Currency Code :{currencyCode} Deleted Successfully.";
                    _fileService.WriteFile(banks);
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Currency Code :{currencyCode} not Found";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public Message ValidateCurrency(string bankId, string currencyCode)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<Currency> currencies = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Currency.FindAll(cr => cr.IsActive == 1);
                currencies ??= new List<Currency>();
                bool currency = currencies.Any(ck => ck.CurrencyCode.Equals(currencyCode));
                if (currency)
                {
                    message.Result = true;
                    message.ResultMessage = $"Currency Code:'{currencyCode}' is Exist";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Currency Code:'{currencyCode}' doesn't Exist";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }
    }
}
