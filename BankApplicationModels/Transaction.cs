using BankApplicationModels.Enums;
using System.Transactions;
using TransactionStatus = BankApplicationModels.Enums.TransactionStatus;

namespace BankApplicationModels
{
    public class Transaction
    {
        public string FromCustomerBankId { get; set; }
        public string ToCustomerBankId { get; set; }
        public string FromCustomerBranchId { get; set; }
        public string ToCustomerBranchId { get; set; }
        public string TransactionId { get; set; }
        public string FromCustomerAccountId { get; set; }
        public TransactionType TransactionType { get; set; }
        public string ToCustomerAccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }

        public override string ToString()
        {
            return $"{TransactionId}: {TransactionType} - {TransactionStatus} " +
         $"From BankId:{FromCustomerBankId}-BranchId:{FromCustomerBranchId}-AccountId:{FromCustomerAccountId} " +
         $"To BankId:{ToCustomerBankId}-BranchId:{ToCustomerBranchId}-AccountId:{ToCustomerAccountId} " +
         $"on {TransactionDate}: Debited Amount:{Debit}, Credited Amount:{Credit}, Balance:{Balance}";
        }
    }
}
