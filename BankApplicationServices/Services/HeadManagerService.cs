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
        public HeadManagerService(IFileService fileService, IBankService bankService, IEncryptionService encryptionService)
        {
            _fileService = fileService;
            _bankService = bankService;
            _encryptionService = encryptionService;
            banks = _fileService.GetData();
        }

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

                    byte[] hashedPasswordToCheck = _encryptionService.HashPassword(headManagerPassword, salt);
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

        public Message OpenHeadManagerAccount(string bankId, string headManagerName, string headManagerPassword)
        {
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                bool isHeadManagerAlreadyAvailabe = banks[banks.FindIndex(obj => obj.BankId == bankId)].HeadManagers.Any(hm => hm.Name == headManagerName && hm.IsActive == 1);
                if (!isHeadManagerAlreadyAvailabe)
                {
                    DateTime currentDate = DateTime.Now;
                    string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                    string UserFirstThreeCharecters = headManagerName.Substring(0, 3);
                    string bankHeadManagerAccountId = UserFirstThreeCharecters + date;

                    byte[] salt = _encryptionService.GenerateSalt();
                    byte[] hashedPassword = _encryptionService.HashPassword(headManagerPassword, salt);

                    HeadManager headManager = new HeadManager()
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

                    banks[banks.FindIndex(obj => obj.BankId == bankId)].HeadManagers.Add(headManager);
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

        public Message UpdateHeadManagerAccount(string bankId, string headManagerAccountId, string headManagerName, string headManagerPassword)
        {
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<HeadManager> headManagers = banks[banks.FindIndex(bk => bk.BankId == bankId)].HeadManagers;
                var headManager = headManagers.Find(hm => hm.AccountId == headManagerAccountId);
                if (headManager != null)
                {
                    if (headManagerName != string.Empty)
                    {
                        headManager.Name = headManagerName;
                    }

                    if (headManagerPassword != string.Empty)
                    {
                        byte[] salt = new byte[32];
                        salt = headManager.Salt;
                        byte[] hashedPasswordToCheck = _encryptionService.HashPassword(headManagerPassword, salt);
                        if (headManager.HashedPassword == hashedPasswordToCheck)
                        {
                            message.Result = false;
                            message.ResultMessage = "New password Matches with the Old Password.,Provide a New Password";
                        }
                        else
                        {
                            salt = _encryptionService.GenerateSalt();
                            byte[] hashedPassword = _encryptionService.HashPassword(headManagerPassword, salt);
                            headManager.Salt = salt;
                            headManager.HashedPassword = hashedPassword;
                            message.Result = true;
                            message.ResultMessage = "Updated Password Sucessfully";

                        }
                    }
                    _fileService.WriteFile(banks);
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Head Manager: {headManagerName} with AccountId:{headManagerAccountId} Doesn't Exist";
                }
            }
            return message;
        }
        public Message DeleteHeadManagerAccount(string bankId, string headManagerAccountId)
        {
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<HeadManager> headManagers = banks[banks.FindIndex(bk => bk.BankId == bankId)].HeadManagers;
                var headManager = headManagers.Find(hm => hm.AccountId == headManagerAccountId);
                if (headManager != null)
                {
                    headManager.IsActive = 0;
                    message.Result = true;
                    message.ResultMessage = $"Deleted AccountId:{headManagerAccountId} Successfully.";
                    _fileService.WriteFile(banks);
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"{headManagerAccountId} Doesn't Exist.";
                }
            }
            return message;
        }

    }
}
