using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class CurrencyService : ICurrencyService
    {
        public static string defaultCurrencyCode = "INR";
        public static short defaultCurrencyValue = 1;
        public Message AddCurrency(string bankId, string currencyCode, decimal exchangeRate)
        {

            Currency currency = new Currency()
            {
                ExchangeRate = exchangeRate,
                CurrencyCode = currencyCode
            };

            if (banks[bankObjectIndex].Currency == null)
            {
                banks[bankObjectIndex].Currency = new List<Currency>();
            }

            banks[bankObjectIndex].Currency.Add(currency);
            _fileService.WriteFile(banks);
            message.Result = true;
            message.ResultMessage = $"Added Currency Code:{currencyCode} with Exchange Rate:{exchangeRate}";
            return message;
        }

        public Message UpdateCurrency(string bankId, string currencyCode, decimal exchangeRate)
        {
        }

        public Message DeleteCurrency(string bankId, string currencyCode)
        {

        }

        public static Message ValidateCurrency(string bankId, string currencyCode)
        {
            var bank = banks.FirstOrDefault(b => b.BankId == bankId);
            if (bank != null)
            {
                bool isValidCurrency = bank.Currency.Any(cr => cr.CurrencyCode == currencyCode);
                message.Result = true;
                message.ResultMessage = $"Currency Code:'{currencyCode}' is Existed";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Invalid Currency Code:'{currencyCode}'";
            }
            return message;
        }
    }
}
