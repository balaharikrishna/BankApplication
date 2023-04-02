using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface ITransactionService
    {
        public List<Bank> GetBankData();
        Message IsTransactionsAvailable(string fromBankId, string fromBranchId, string fromCustomerAccountId);
        List<string> GetTransactionHistory(string fromBankId, string fromBranchId, string fromCustomerAccountId);
        Message RevertTransaction(string transactionId, string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId, string toBranchId, string toCustomerAccountId);
        void TransactionHistory(string fromBankId, string fromBranchId, string fromCustomerAccountId, decimal debitAmount, decimal creditAmount, decimal fromCustomerbalance, int transactionType, int transactionStatus);
        void TransactionHistory(string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId, string toBranchId, string toCustomerAccountId, decimal debitAmount, decimal creditAmount, decimal fromCustomerbalance, decimal toCustomerBalance, int transactionType, int transactionStatus);
    }
}

