using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IManagerService
    {
        Message AuthenticateManagerAccount(string bankId, string branchId, string branchManagerAccountId, string branchManagerPassword);
        Message DeleteManagerAccount(string bankId, string branchId, string accountId);
        Message OpenManagerAccount(string bankId, string branchId, string branchManagerName, string branchManagerPassword);
        Message UpdateManagerAccount(string bankId, string branchId, string accountId, string branchManagerName, string branchManagerPassword);
        Message IsAccountExist(string bankId, string branchId, string managerAccountId);
        public string GetManagerDetails(string bankId, string branchId, string managerAccountId);

    }
}