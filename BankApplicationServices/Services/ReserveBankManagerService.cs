using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class ReserveBankManagerService : IReserveBankManagerService
    {
        public static string reserveBankManagerName = "TECHNOVERT";
        public static string reserveBankManagerpassword = "Techno123@";

        public Message AuthenticateReserveBankManager(string userName, string userPassword)
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
