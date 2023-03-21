using BankApplicationModels;
using BankApplicationServices.Interfaces;

namespace BankApplicationServices.Services
{
    public class BankService : IBankService
    {
        IFileService _fileService;
        List<Bank> banks;
        public BankService(IFileService fileService)
        {
            _fileService = fileService;
            this.banks = _fileService.GetData();
        }
        public Dictionary<string, decimal> GetExchangeRates(string bankId)
        {
            Dictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>();

            int bankIndex = banks.FindIndex(bank => bank.BankId == bankId);
            if (bankIndex > -1)
            {
                List<Currency> rates = banks[bankIndex].Currency;
                for (int i = 0; i < rates.Count; i++)
                {
                    exchangeRates.Add(rates[i].CurrencyCode, rates[i].ExchangeRate);
                }
            }
            else
            {
                exchangeRates = null;
            }

            return exchangeRates;
        }


    }
}
