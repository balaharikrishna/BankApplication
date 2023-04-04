using BankApplicationModels.Enums;


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
        public string TransactionDate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }

        public override string ToString()
        {
            return $"{TransactionId}: {TransactionType} " +
         $"From BankId:{FromCustomerBankId}-BranchId:{FromCustomerBranchId}-AccountId:{FromCustomerAccountId} " +
         $"To BankId:{ToCustomerBankId}-BranchId:{ToCustomerBranchId}-AccountId:{ToCustomerAccountId} " +
         $"on {TransactionDate}: Debited Amount:{Debit}, Credited Amount:{Credit}, Balance:{Balance}";
        }
    }
}
