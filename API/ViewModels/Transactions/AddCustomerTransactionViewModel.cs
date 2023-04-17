using BankApplicationModels.Enums;

namespace API.ViewModels.Transactions
{
    public class AddCustomerTransactionViewModel
    {
        public string? FromCustomerBankId { get; set; }
        public string? FromCustomerBranchId { get; set; }
        public string? FromCustomerAccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string? TransactionDate { get; set; }
        public decimal Balance { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
