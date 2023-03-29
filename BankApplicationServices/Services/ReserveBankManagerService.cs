using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class ReserveBankManagerService : IReserveBankManagerService
    {
        private readonly IFileService _fileService;
        List<Bank> banks;
        public ReserveBankManagerService(IFileService fileService)
        {
            _fileService = fileService;
            banks = _fileService.GetData();
        }
        public static string reserveBankManagerName = "Technovert";
        public static string reserveBankManagerpassword = "Techno123@";
        Message message = new Message();
        public Message AuthenticateReserveBankManager(string userName, string userPassword)
        {

            if (userName.Equals(reserveBankManagerName) && userPassword.Equals(reserveBankManagerpassword))
            {
                message.Result = true;
            }
            else
            {
                message.Result = false;
            }
            return message;
        }

    }
}
