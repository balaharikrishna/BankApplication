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
        }
        public static string reserveBankManagerName = "TECHNOVERT";
        public static string reserveBankManagerpassword = "Techno123@";
        public List<Bank> GetBankData()
        {
            if (_fileService.GetData() != null)
            {
                banks = _fileService.GetData();
            }
            return banks;
        }
        Message message = new Message();
        public Message AuthenticateReserveBankManager(string userName, string userPassword)
        {
            GetBankData();
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
