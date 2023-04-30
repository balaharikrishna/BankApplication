using BankApplicationModels;
using BankApplicationRepository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.Text;

namespace BankApplicationRepository.Repository
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
            return await _context.Banks.Where(b=>b.IsActive.Equals(true)).ToListAsync();
        }

        public async Task<Bank?> GetBankById(string id)
        {
            return await _context.Banks.FirstOrDefaultAsync(b => b.BankId.Equals(id) && b.IsActive.Equals(true));
        }

        public async Task<Bank?> GetBankByName(string bankName)
        {
            return await _context.Banks.FirstOrDefaultAsync(b => b.BankName.Equals(bankName) && b.IsActive.Equals(true));
        }

        public async Task<bool> IsBankExist(string bankId)
        {
            return await _context.Banks.AnyAsync(b => b.BankId.Equals(bankId) && b.IsActive.Equals(true));
        }

        public async Task<bool> AddBank(Bank bank)
        {
            await _context.Banks.AddAsync(bank);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateBank(Bank bank)
        {
            _context.Banks.Update(bank);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }


        public async Task<bool> DeleteBank(string id)
        {
            Bank bank = await GetBankById(id);
            bank.IsActive = false;
            _context.Banks.Update(bank);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Currency>> GetAllCurrencies(string bankId)
        {
            return await _context.Currencies.Where(c => c.BankId.Equals(bankId) && c.IsActive.Equals(true)).ToListAsync();
        }
    }
}
