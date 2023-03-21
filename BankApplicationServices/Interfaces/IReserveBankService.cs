using BankApplicationModels;
namespace BankApplicationServices.Interfaces
{
    public interface IReserveBankService
    {
        Message CreateBank(string bankName);
        Message CreateBankHeadManagerAccount(string bankId, string headManagerName, string headManagerPassword);
        Message ValidateReserveBankManager(string userName, string userPassword);
    }
}