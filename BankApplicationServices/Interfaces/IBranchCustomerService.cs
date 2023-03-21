using BankApplicationModels;
namespace BankApplicationServices.Interfaces
{
    public interface IBranchCustomerService
    {
        Message CheckAccountBalance();
        Message CheckToCustomerAccountBalance();
        string GetPassbook();
        List<string> GetTransactionHistory();
        void TransactionHistory(string fromBankId, string fromBranchId, string fromCustomerAccountId, decimal debitAmount, decimal creditAmount, decimal fromCustomerbalance, int transactionType, int transactionStatus);
        void TransactionHistory(string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId, string toBranchId, string toCustomerAccountId, decimal debitAmount, decimal creditAmount, decimal fromCustomerbalance, decimal toCustomerBalance, int transactionType, int transactionStatus);
        Message TransferAmount(string toBankId, string toBankBranchId, string toCustomerAccountId, decimal transferAmount, int transferMethod);
        bool ValidateCustomerAccount(string bankId, string branchid, string customerAccountId);
        Message ValidateCustomerLogin(string bankId, string branchid, string customerAccountId, string customerAccountPassword);
        bool ValidateToCustomerAccount(string bankId, string branchid, string customerAccountId);
        Message WithdrawAmount(decimal withDrawAmount);
    }
}