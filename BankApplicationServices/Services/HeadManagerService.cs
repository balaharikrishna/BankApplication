using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class HeadManagerService : IHeadManagerService
    {
        IFileService _fileService;
        IBankService _bankService;
        IEncryptionService _encryptionService;

        List<Bank> banks;
        public HeadManagerService(IFileService fileService, IBankService bankService,IEncryptionService encryptionService)
        {
            _fileService = fileService;
            _bankService = bankService;
            _encryptionService = encryptionService;
            banks = _fileService.GetData();
        }

        private static int bankObjectIndex = 0;
        Message message = new Message();
        public Message AuthenticateHeadManager(string bankId, string headManagerAccountId, string headManagerPassword)
        {
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<HeadManager> headManagers = banks[banks.FindIndex(bk => bk.BankId == bankId)].HeadManagers;
                if (headManagers.Count > 0)
                {
                    byte[] salt = new byte[32];
                    var headManager = headManagers.Find(hm => hm.AccountId == headManagerAccountId);
                    if (headManager != null)
                    {
                        salt = headManager.Salt;    
                    }

                    byte[] hashedPasswordToCheck =_encryptionService.HashPassword(headManagerPassword,salt);
                    bool isHeadManagerAvilable = headManagers.Any(hm => hm.AccountId == headManagerAccountId && hm.HashedPassword == hashedPasswordToCheck && hm.IsActive == 1);
                    if (isHeadManagerAvilable)
                    {
                        message.Result = true;
                        message.ResultMessage = "Head Manager Validation Successful.";
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Head Manager Validation Failed.";
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "No Head Managers Available In The Bank.";
                }
            }
            return message;
        }

        public Message CreateHeadManagerAccount(string bankId, string headManagerName, string headManagerPassword)
        {
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                bool isHeadManagerAlreadyAvailabe = banks[banks.FindIndex(obj => obj.BankId == bankId)].HeadManagers.Any(hm=>hm.Name == headManagerName && hm.IsActive == 1);
                if (!isHeadManagerAlreadyAvailabe)
                {
                    DateTime currentDate = DateTime.Now;
                    string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                    string UserFirstThreeCharecters = headManagerName.Substring(0, 3);
                    string bankHeadManagerAccountId = UserFirstThreeCharecters + date;

                    byte[] salt =  _encryptionService.GenerateSalt();
                    byte[] hashedPassword = _encryptionService.HashPassword(headManagerPassword, salt);

                    HeadManager bankHeadManagerService = new HeadManager()
                    {
                        Name = headManagerName,
                        Salt = salt,
                        HashedPassword = hashedPassword,
                        AccountId = bankHeadManagerAccountId,
                        IsActive = 1
                    };

                    List<HeadManager> managers = banks[banks.FindIndex(obj => obj.BankId == bankId)].HeadManagers;

                    if (managers == null)
                    {
                        managers = new List<HeadManager>();
                    }

                    banks[banks.FindIndex(obj => obj.BankId == bankId)].HeadManagers.Add(bankHeadManagerService);
                    _fileService.WriteFile(banks);
                    message.Result = true;
                    message.ResultMessage = $"Account Created for {headManagerName} with Account Id:{bankHeadManagerAccountId}";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Head Manager: {headManagerName} Already Existed";
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
