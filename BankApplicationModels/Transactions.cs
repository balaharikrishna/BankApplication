using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class Transaction
    {
        [Required]
        public string FromCustomerBankId { get; set; }
        [Required]
        public string ToCustomerBankId { get; set; }
        [Required]
        public string FromCustomerBranchId { get; set; }
        [Required]
        public string ToCustomerBranchId { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public string FromCustomerAccountId { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        [Required]
        public string ToCustomerAccountId { get; set; }
        [Required]
        public string TransactionDate { get; set; }
        [Required]
        public decimal Debit { get; set; }
        [Required]
        public decimal Credit { get; set; }
        [Required]
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
