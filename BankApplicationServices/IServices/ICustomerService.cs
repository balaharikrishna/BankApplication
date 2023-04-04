using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface ICustomerService
    {
        Message IsCustomersExist(string bankId, string branchId);
        Message AuthenticateCustomerAccount(string bankId, string branchId, string customerAccountId, string customerPassword);
        Message IsAccountExist(string bankId, string branchId, string customerAccountId);
        Message AuthenticateToCustomerAccount(string bankId, string branchId, string customerAccountId);
        Message CheckAccountBalance(string bankId, string branchId, string customerAccountId);
        Message CheckToCustomerAccountBalance(string bankId, string branchId, string customerAccountId);
        Message DeleteCustomerAccount(string bankId, string branchId, string customerAccountId);
        Message DepositAmount(string bankId, string branchId, string customerAccountId, decimal depositAmount, string currencyCode);
        string GetPassbook(string bankId, string branchId, string customerAccountId);
        Message OpenCustomerAccount(string bankId, string branchId, string customerName, string customerPassword, string customerPhoneNumber, string customerEmailId, int customerAccountType, string customerAddress, string customerDateOfBirth, int customerGender);
        Message TransferAmount(string bankId, string branchId, string customerAccountId, string toBankId,
            string toBranchId, string toCustomerAccountId, decimal transferAmount, int transferMethod);
        Message UpdateCustomerAccount(string bankId, string branchId, string customerAccountId, string customerName, string customerPassword, string customerPhoneNumber, string customerEmailId, int customerAccountType, string customerAddress, string customerDateOfBirth, int customerGender);
        Message WithdrawAmount(string bankId, string branchId, string customerAccountId,decimal withDrawAmount);
    }
}