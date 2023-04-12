using System.Transactions;

namespace BankApplicationRepository.IRepository
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllTransactions(string fromCustomerAccountId);
        Task<Transaction> GetTransactionById(string fromCustomerAccountId, string transactionId);
        Task<bool> IsTransactionsExist(string fromCustomerAccountId);
        Task<bool> AddTransaction(string fromCustomerAccountId);
    }
}
