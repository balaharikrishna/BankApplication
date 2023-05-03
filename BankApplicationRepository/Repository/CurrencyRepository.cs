using BankApplicationModels;
using BankApplicationRepository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BankApplicationRepository.Repository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly BankDBContext _context;
        public CurrencyRepository(BankDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Currency>> GetAllCurrency(string bankId)
        {
            return await _context.Currencies.Where(c => c.BankId.Equals(bankId) && c.IsActive.Equals(true)).ToListAsync();
        }
        public async Task<bool> AddCurrency(Currency currency, string bankId)
        {
            currency.BankId = bankId;
            await _context.Currencies.AddAsync(currency);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateCurrency(Currency currency, string bankId)
        {
            Currency? currencyToUpdate = await _context.Currencies.FirstOrDefaultAsync(c => c.BankId.Equals(bankId) &&
            c.CurrencyCode.Equals(currency.CurrencyCode) && c.IsActive.Equals(true));

            if (currency.CurrencyCode is not null)
            {
                currencyToUpdate!.CurrencyCode = currency.CurrencyCode;
                currencyToUpdate.ExchangeRate = currency.ExchangeRate;
            }
            _context.Currencies.Update(currencyToUpdate!);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteCurrency(string currencyCode, string bankId)
        {
            IEnumerable<Currency> currencies = await GetAllCurrency(bankId);
            Currency? currency = currencies.FirstOrDefault(c => c.CurrencyCode.Equals(currencyCode));
            currency!.IsActive = false;
            _context.Currencies.Update(currency);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> IsCurrencyExist(string currencyCode, string bankId)
        {
            return await _context.Currencies.AnyAsync(c => c.CurrencyCode.Equals(currencyCode) && c.BankId.Equals(bankId) && c.IsActive.Equals(true));
        }

        public async Task<Currency?> GetCurrencyByCode(string currencyCode, string bankId)
        {
            return await _context.Currencies.FirstOrDefaultAsync(c => c.CurrencyCode.Equals(currencyCode) && c.IsActive.Equals(true));
        }
    }
}
