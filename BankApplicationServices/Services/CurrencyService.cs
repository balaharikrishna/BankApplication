using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class CurrencyService : ICurrencyService
    {
        public static string defaultCurrencyCode = "INR";
        public static short defaultCurrencyValue = 1;

        IBankService _bankService;
        IFileService _fileService;
        List<Bank> banks;
        Message message = new Message();
        public CurrencyService(IFileService fileService, IBankService bankService) {
            _bankService = bankService;
            _fileService = fileService;
            banks = _fileService.GetData();
        }
        public Message AddCurrency(string bankId, string currencyCode, decimal exchangeRate)
        {
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

                banks[bankIndex].Currency.Add(currency);
                _fileService.WriteFile(banks);
                message.Result = true;
                message.ResultMessage = $"Added Currency Code:{currencyCode} with Exchange Rate:{exchangeRate}";
            }
            return message;
        }

        public Message UpdateCurrency(string bankId, string currencyCode, decimal exchangeRate)
        {
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
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<Currency> currencies = banks[banks.FindIndex(bk => bk.BankId == bankId)].Currency;
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
