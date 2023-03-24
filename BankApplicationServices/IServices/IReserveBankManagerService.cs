using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IReserveBankManagerService
    {
        Message AuthenticateReserveBankManager(string userName, string userPassword);
    }
}