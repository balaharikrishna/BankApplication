using BankApplication.Models;
using BankApplication.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BankApplication.Repository.Repository
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
            IEnumerable<Transaction> transactions = await _context.Transactions.Where(c => c.AccountId.Equals(accountId)).ToListAsync();
            if (transactions.Any())
            {
                return transactions;
            }
            else
            {
                return Enumerable.Empty<Transaction>();
            }
        }
        public async Task<Transaction?> GetTransactionById(string accountId, string transactionId)
        {
            Transaction? transaction =  await _context.Transactions.FirstOrDefaultAsync(c => c.AccountId.Equals(accountId)
            && c.TransactionId.Equals(transactionId));
            if (transaction is not null)
            {
                return transaction;
            }
            else
            {
                return null;
            }
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
