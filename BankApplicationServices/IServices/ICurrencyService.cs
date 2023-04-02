using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface ICurrencyService
    {
        Message AddCurrency(string bankId, string currencyCode, decimal exchangeRate);
        Message DeleteCurrency(string bankId, string currencyCode);
        Message UpdateCurrency(string bankId, string currencyCode, decimal exchangeRate);
        Message ValidateCurrency(string bankId, string currencyCode);

        public static string? defaultCurrencyCode;
        public static short defaultCurrencyValue;
    }
}