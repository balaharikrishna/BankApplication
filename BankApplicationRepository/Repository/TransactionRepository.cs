using BankApplicationModels;
using BankApplicationRepository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BankApplicationRepository.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankDBContext _context;
        public TransactionRepository(BankDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Transaction>> GetAllTransactions(string accountId)
        {
            return await _context.Transactions.Where(c => c.AccountId.Equals(accountId)).ToListAsync();
        }
        public async Task<Transaction?> GetTransactionById(string accountId, string transactionId)
        {
            return await _context.Transactions.FirstOrDefaultAsync(c => c.AccountId.Equals(accountId)
            && c.TransactionId.Equals(transactionId));
        }
        public async Task<bool> IsTransactionsExist(string accountId)
        {
            return await _context.Transactions.AnyAsync(c => c.AccountId.Equals(accountId));
        }
        public async Task<bool> AddTransaction(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            int rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
    }
}
