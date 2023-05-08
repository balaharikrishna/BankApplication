using BankApplication.Models;
using BankApplication.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BankApplication.Repository.Repository
{
    public class BankRepository : IBankRepository
    {
        private readonly BankDBContext _context;

        public BankRepository(BankDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bank>> GetAllBanks()
        {
            IEnumerable<Bank> banks = await _context.Banks.Where(b => b.IsActive).ToListAsync();
            if (banks.Any())
            {
                return banks;
            }
            else
            {
                return Enumerable.Empty<Bank>();
            }
        }

        public async Task<Bank?> GetBankById(string id)
        {
            Bank? bank = await _context.Banks.FirstOrDefaultAsync(b => b.BankId.Equals(id) && b.IsActive);
            if (bank is not null)
            {
                return bank;
            }
            else
            {
                return null;
            }
        }

        public async Task<Bank?> GetBankByName(string bankName)
        {
            Bank? bank = await _context.Banks.FirstOrDefaultAsync(b => b.BankName.Equals(bankName) && b.IsActive);
            if (bank is not null)
            {
                return bank;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IsBankExist(string bankId)
        {
            return await _context.Banks.AnyAsync(b => b.BankId.Equals(bankId) && b.IsActive);
        }

        public async Task<bool> AddBank(Bank bank)
        {
            await _context.Banks.AddAsync(bank);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateBank(Bank bank)
        {
            Bank? bankObj = await GetBankById(bank.BankId);
            if (bankObj!.BankName is not null)
            {
                bankObj.BankName = bankObj.BankName;
            }
            _context.Banks.Update(bankObj);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteBank(string id)
        {

            Bank? bank = await GetBankById(id);
            bank!.IsActive = false;
            _context.Banks.Update(bank);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Currency>> GetAllCurrencies(string bankId)
        {
            IEnumerable<Currency>? currencies = await _context.Currencies.Where(c => c.BankId.Equals(bankId) && c.IsActive).ToListAsync();
            if (currencies.Any())
            {
                return currencies;
            }
            else
            {
                return Enumerable.Empty<Currency>();
            }
        }
    }
}
