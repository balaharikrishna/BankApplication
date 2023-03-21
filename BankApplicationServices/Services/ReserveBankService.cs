using BankApplicationModels;
using BankApplicationServices.Interfaces;

namespace BankApplicationServices.Services
{
    public class ReserveBankService : IReserveBankService
    {
        IFileService _fileService;
        List<Bank> banks;
        public ReserveBankService(IFileService fileService)
        {
            _fileService = fileService;
            this.banks = _fileService.GetData();
        }
        public static string reserveBankManagerName = "Technovert";
        public static string reserveBankManagerpassword = "Techno123@";
        Message message = new Message();
        public Message ValidateReserveBankManager(string userName, string userPassword)
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
        public Message CreateBank(string bankName)
        {
            bool isBankAlreadyRegistered = false;

            if (_fileService.ReadFile().Length > 0)
            {
                isBankAlreadyRegistered = banks.Any(bank => bank.BankName == bankName);
            }

            if (isBankAlreadyRegistered)
            {
                Console.WriteLine();
                message.Result = false;
                message.ResultMessage = $"BankName:{bankName} is Already Registered.";
            }
            else
            {
                DateTime currentDate = DateTime.Today;
                string date = currentDate.ToString().Substring(0, 10).Replace("-", "");
                string bankFirstThreeCharecters = bankName.Substring(0, 3);
                string bankId = bankFirstThreeCharecters + date + "M";

                Bank bank = new Bank() { BankName = bankName, BankId = bankId };
                banks.Add(bank);

                _fileService.WriteFile(banks);
                message.Result = true;
                message.ResultMessage = $"Bank {bankName} is Created with {bankId}";
            }
            return message;
        }
        public Message CreateBankHeadManagerAccount(string bankId, string headManagerName, string headManagerPassword)
        {
            bool isBankExist = false;
            int bankObjectIndex = 0;

            if (banks.Count > 0)
            {
                isBankExist = banks.Any(bank => bank.BankId == bankId);
                bankObjectIndex = banks.FindIndex(obj => obj.BankId == bankId);

                if (isBankExist)
                {
                    DateTime currentDate = DateTime.Now;
                    string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                    string UserFirstThreeCharecters = headManagerName.Substring(0, 3);
                    string bankHeadManagerAccountId = UserFirstThreeCharecters + date;

                    BankHeadManager bankHeadManagerService = new BankHeadManager()
                    {
                        HeadManagerName = headManagerName,
                        HeadManagerPassword = headManagerPassword,
                        HeadManagerAccountId = bankHeadManagerAccountId
                    };

                    if (banks[bankObjectIndex].HeadManagers == null)
                    {
                        banks[bankObjectIndex].HeadManagers = new List<BankHeadManager>();
                    }

                    banks[bankObjectIndex].HeadManagers.Add(bankHeadManagerService);

                    _fileService.WriteFile(banks);
                    message.Result = true;
                    message.ResultMessage = $"Account Created for {headManagerName} with Account Id:{bankHeadManagerAccountId}";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Provided BankId:{bankId} is Not Available., Please Provide Valid Details.";
                }
            }

            return message;
        }
    }
}
