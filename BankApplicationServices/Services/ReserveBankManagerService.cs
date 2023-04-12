using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class ReserveBankManagerService : IReserveBankManagerService
    {
        private static string reserveBankManagerName = "TECHNOVERT";
        private static string reserveBankManagerpassword = "Techno123@";

        public Task<Message> AuthenticateReserveBankManagerAsync(string userName, string userPassword)
        {
            Message message = new();
            if (userName.Equals(reserveBankManagerName) && userPassword.Equals(reserveBankManagerpassword))
            {
                message.Result = true;
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Entered Password is Wrong.";
            }
            return message;
        }
    }
}
