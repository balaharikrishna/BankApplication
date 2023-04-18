using BankApplicationModels;

namespace BankApplicationRepository.IRepository
{
    public interface ITransactionRepository
    {
        Task<bool> AddTransaction(Transaction transaction);
        Task<IEnumerable<Transaction>> GetAllTransactions(string fromCustomerAccountId);
        Task<Transaction?> GetTransactionById(string fromCustomerAccountId, string transactionId);
        Task<bool> IsTransactionsExist(string fromCustomerAccountId);
    }
}