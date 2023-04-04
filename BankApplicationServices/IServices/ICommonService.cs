using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface ICommonService
    {
        Message AuthenticateAccount(string bankId, string branchId, string accountId, string password, string level);
        Message DeleteAccount(string bankId, string branchId, string accountId, string level);
        Message GetAccountDetails(string bankId, string branchId, string managerAccountId, string level);
        Message IsAccountsExist(string bankId, string branchId, string level);
    }
}