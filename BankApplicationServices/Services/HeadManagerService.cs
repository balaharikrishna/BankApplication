using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class HeadManagerService : IHeadManagerService
    {
        IFileService _fileService;
        List<Bank> banks;
        public HeadManagerService(IFileService fileService)
        {
            _fileService = fileService;
            this.banks = _fileService.GetData();
        }

        private static int bankObjectIndex = 0;
        Message message = new Message();
        public Message AuthenticateHeadManager(string bankName, string bankId, string headManagerAccountId, string headManagerPassword)
        {
            if (banks.Count > 0)
            {
                var bank = banks.FirstOrDefault(b => b.BankName == bankName && b.BankId == bankId);
                if (bank != null)
                {
                    var headManager = bank.HeadManagers?.FirstOrDefault(hm => hm.HeadManagerAccountId == headManagerAccountId && hm.HeadManagerPassword == headManagerPassword);
                    if (headManager != null)
                    {
                        bankObjectIndex = banks.IndexOf(bank);
                        message.Result = true;
                        message.ResultMessage = "Head Manager Validation Successful.";
                    }
                }
            }
            return message;
        }

        public Message CreateHeadManagerAccount(string bankId, string headManagerName, string headManagerPassword)
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

        public Message UpdateHeadManagerAccount(string bankId, string headManagerName, string headManagerPassword)
        {

        }
        public Message DeleteHeadManagerAccount(string bankId, string headManagerAccountId)
        {

        }

    }
}
