using BankApplicationModels;
namespace BankApplicationServices.Interfaces
{
    public interface IBankHeadManagerService
    {
        Message AddCurrencyWithExchangeRate(string currencyCode, decimal exchangeRate);
        Message CreateBankBranch(string branchName, string branchPhoneNumber, string branchAddress);
        Message OpenBranchManagerAccount(string branchid, string branchManagerName, string branchManagerPassword);
        Message ValidateBankHeadManager(string bankName, string bankId, string headManagerAccountId, string headManagerPassword);
    }
}