using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IBankService _bankService;
        private readonly IFileService _fileService;
        List<Bank> banks;
        public CurrencyService(IFileService fileService, IBankService bankService) {
            _bankService = bankService;
            _fileService = fileService;
        }
        public List<Bank> GetBankData()
        {
            if (_fileService.GetData() != null)
            {
                banks = _fileService.GetData();
            }
            return banks;
        }
        Message message = new Message();
        public Message AddCurrency(string bankId, string currencyCode, decimal exchangeRate)
        {
            GetBankData();
           message =  _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                Currency currency = new Currency()
                {
                    ExchangeRate = exchangeRate,
                    CurrencyCode = currencyCode
                };
                int bankIndex = banks.FindIndex(bk => bk.BankId == bankId);
                List<Currency> currencies = banks[bankIndex].Currency;
                if (currencies == null)
                {
                    currencies = new List<Currency>();
                }
                currencies.Add(currency);
                banks[bankIndex].Currency = currencies;
                _fileService.WriteFile(banks);
                message.Result = true;
                message.ResultMessage = $"Added Currency Code:{currencyCode} with Exchange Rate:{exchangeRate}";
            }
            return message;
        }

        public Message UpdateCurrency(string bankId, string currencyCode, decimal exchangeRate)
        {
            GetBankData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<Currency> currencies = banks[banks.FindIndex(bk => bk.BankId == bankId)].Currency;
                var currency = currencies.Find(ck => ck.CurrencyCode == currencyCode);
                if (currency != null)
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
            return message;
        }
        public Message DeleteCurrency(string bankId, string currencyCode)
        {
            GetBankData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<Currency> currencies = banks[banks.FindIndex(bk => bk.BankId == bankId)].Currency;
                var currency = currencies.Find(ck => ck.CurrencyCode == currencyCode);
                if (currency != null)
                {
                    currency.IsDeleted = 1;
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
            return message;
        }

        public  Message ValidateCurrency(string bankId, string currencyCode)
        {
            GetBankData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<Currency> currencies = banks[banks.FindIndex(bk => bk.BankId == bankId)].Currency.FindAll(cr=>cr.IsDeleted == 0);
                if(currencies == null)
                {
                    currencies= new List<Currency>();
                }
                bool currency = currencies.Any(ck => ck.CurrencyCode == currencyCode);
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
            return message;
        }
    }
}
