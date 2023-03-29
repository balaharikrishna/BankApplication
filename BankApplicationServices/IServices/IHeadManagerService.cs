using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IHeadManagerService
    {
        Message AuthenticateHeadManager(string bankId, string headManagerAccountId, string headManagerPassword);
        Message OpenHeadManagerAccount(string bankId, string headManagerName, string headManagerPassword);
        Message DeleteHeadManagerAccount(string bankId, string headManagerAccountId);
        Message UpdateHeadManagerAccount(string bankId, string headManagerAccountId, string headManagerName, string headManagerPassword);
        Message IsHeadManagerExist(string bankId, string headManagerAccountId);
        public string GetHeadManagerDetails(string bankId, string headManagerAccountId);

    }
}