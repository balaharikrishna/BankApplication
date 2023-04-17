using BankApplicationModels.Enums;

namespace API.ViewModels.Transactions
{
    public class AddFromAndToCustomerTransactionViewModel : AddCustomerTransactionViewModel
    {
        public string? ToCustomerBankId { get; set; }
        public string? ToCustomerBranchId { get; set; }
        public string? ToCustomerAccountId { get; set; }
        public decimal ToCustomerBalance { get; set; }
    }
}
