using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IHeadManagerService
    {
        Message AuthenticateHeadManager(string bankId, string headManagerAccountId, string headManagerPassword);
        Message CreateHeadManagerAccount(string bankId, string headManagerName, string headManagerPassword);
        Message DeleteHeadManagerAccount(string bankId, string headManagerAccountId);
        Message UpdateHeadManagerAccount(string bankId, string headManagerName, string headManagerPassword);
    }
}