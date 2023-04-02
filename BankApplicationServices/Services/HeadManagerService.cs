using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class HeadManagerService : IHeadManagerService
    {
        private readonly IFileService _fileService;
        private readonly IBankService _bankService;
        private readonly IEncryptionService _encryptionService;

        List<Bank> banks;
        public HeadManagerService(IFileService fileService, IBankService bankService, IEncryptionService encryptionService)
        {
            _fileService = fileService;
            _bankService = bankService;
            _encryptionService = encryptionService;
        }
        public List<Bank> GetBankData()
        {
            if (_fileService.GetData() != null)
            {
                banks = _fileService.GetData();
            }
            return banks;
        }
        Message message = new Message();

        public Message IsHeadManagersExist(string bankId)
        {
            GetBankData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    List<HeadManager> headManagers = bank.HeadManagers; 
                    if(headManagers == null)
                    {
                        headManagers = new List<HeadManager>();
                        headManagers.FindAll(hm => hm.IsActive == 1);
                    }
                   
                    if (headManagers != null && headManagers.Count > 0)
                    {
                        message.Result = true;
                        message.ResultMessage = "Head Managers Exist in Branch";
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = $"No Head Managers Available In The Bank:{bankId}";
                    }
                }
            }
            return message;
        }
        public Message AuthenticateHeadManager(string bankId, string headManagerAccountId, string headManagerPassword)
        {
            GetBankData();
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
                    string hashedPassword = Convert.ToBase64String(hashedPasswordToCheck);
                    bool isHeadManagerAvilable = headManagers.Any(hm => hm.AccountId == headManagerAccountId && Convert.ToBase64String(hm.HashedPassword) == hashedPassword && hm.IsActive == 1);
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
            GetBankData();
            List<HeadManager> headManagers = banks[banks.FindIndex(obj => obj.BankId == bankId)].HeadManagers;
            if (headManagers == null)
            {
                headManagers = new List<HeadManager>();
            }
            bool isHeadManagerAlreadyAvailabe = headManagers.Any(hm => hm.Name == headManagerName && hm.IsActive == 1);
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
            return message;
        }

        public Message IsHeadManagerExist(string bankId, string headManagerAccountId)
        {
            GetBankData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    List<HeadManager> headManagers = bank.HeadManagers;
                    if (headManagers != null)
                    {
                        bool isManagerAvilable = headManagers.Any(m => m.AccountId == headManagerAccountId && m.IsActive == 1);
                        if (isManagerAvilable)
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
                        message.ResultMessage = $"No Head Managers Available In The Bank:{bankId}";
                    }
                }
            }
            return message;
        }
        public Message UpdateHeadManagerAccount(string bankId, string headManagerAccountId, string headManagerName, string headManagerPassword)
        {
            GetBankData();
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
                        if (Convert.ToBase64String(headManager.HashedPassword) == Convert.ToBase64String(hashedPasswordToCheck))
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
                    if (headManagerName == string.Empty && headManagerPassword == string.Empty)
                    {
                        message.Result = true;
                        message.ResultMessage = $"No Changes Added.";
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
            GetBankData();
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

        public string GetHeadManagerDetails(string bankId, string headManagerAccountId)
        {
            GetBankData();
            message = IsHeadManagerExist(bankId, headManagerAccountId);
            string headManagerDetails = string.Empty;
            if (message.Result)
            {
                int bankIndex = banks.FindIndex(b => b.BankId == bankId);
                int headManagerIndex = banks[bankIndex].HeadManagers.FindIndex(hm => hm.AccountId == headManagerAccountId);

                HeadManager details = banks[bankIndex].HeadManagers[headManagerIndex];
                headManagerDetails = details.ToString() ?? string.Empty;
            }
            return headManagerDetails;
        }
    }
}
