using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface ICustomerService
    {
        bool AuthenticateCustomerAccount(string bankId, string branchId, string customerAccountId);
        Message AuthenticateCustomerLogin(string bankId, string branchId, string customerAccountId, string customerAccountPassword);
        bool AuthenticateToCustomerAccount(string bankId, string branchId, string customerAccountId);
        Message CheckAccountBalance(string bankId, string branchId, string customerAccountId);
        Message CheckToCustomerAccountBalance();
        Message DeleteCustomerAccount(string bankId, string branchId, string customerAccountId);
        Message DepositAmount(string bankId, string branchId, string customerAccountId, decimal depositAmount, string currencyCode);
        string GetPassbook();
        Message OpenCustomerAccount(string bankId, string branchId, string customerName, string customerPassword, string customerPhoneNumber, string customerEmailId, int customerAccountType, string customerAddress, string customerDateOfBirth, int customerGender);
        Message TransferAmount(string toBankId, string toBankbranchId, string toCustomerAccountId, decimal transferAmount, int transferMethod);
        Message UpdateCustomerAccount(string bankId, string branchId, string customerAccountId, string customerName, string customerPassword, string customerPhoneNumber, string customerEmailId, int customerAccountType, string customerAddress, string customerDateOfBirth, int customerGender);
        Message WithdrawAmount(decimal withDrawAmount);
    }
}