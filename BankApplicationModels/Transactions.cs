using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string CustomerAccountId { get; set; }
        [Required]
        public string CustomerBankId { get; set; }
        [Required]
        public string CustomerBranchId { get; set; }
        public string FromCustomerBankId { get; set; }
        public string ToCustomerBankId { get; set; }
        public string FromCustomerBranchId { get; set; }
        public string ToCustomerBranchId { get; set; }
        public string FromCustomerAccountId { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        public string ToCustomerAccountId { get; set; }
        [Required]
        public string TransactionDate { get; set; }
        [Required]
        public decimal Debit { get; set; }
        [Required]
        public decimal Credit { get; set; }
        [Required]
        public decimal Balance { get; set; }

        [ForeignKey("Customer")]
        public string AccountId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
