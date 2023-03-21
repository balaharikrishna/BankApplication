using BankApplicationModels;
namespace BankApplicationServices.Interfaces
{
    public interface IBranchStaffService
    {
        Message DeleteCustomerAccount(string customerAccountId);
        Message DepositAmount(string customerAccountId, decimal depositAmount, string currencyCode);
        Message OpenCustomerAccount(string customerName, string customerPassword, string customerPhoneNumber, string customerEmailId, int customerAccountType, string customerAddress, string customerDateOfBirth, int customerGender);
        Message RevertTransaction(string transactionId, string fromBankId, string fromBranchId, string fromCustomerAccountId, string toBankId, string toBranchId, string toCustomerAccountId);
        Message UpdateCustomerAccount(string customerAccountId, string customerName, string customerPassword, string customerPhoneNumber, string customerEmailId, int customerAccountType, string customerAddress, string customerDateOfBirth, int customerGender);
        Message ValidateBranchStaffAccount(string bankId, string branchid, string staffAccountId, string staffAccountPassword);
    }
}