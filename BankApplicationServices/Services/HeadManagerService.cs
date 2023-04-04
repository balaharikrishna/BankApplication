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
            banks = new List<Bank>();
        }
       
        public Message IsHeadManagersExist(string bankId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    List<HeadManager> headManagers = bank.HeadManagers;
                    if (headManagers is null)
                    {
                        headManagers = new List<HeadManager>();
                        headManagers.FindAll(hm => hm.IsActive.Equals(1));
                    }

                    if (headManagers is not null && headManagers.Count > 0)
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
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Bank Not Found";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }
        public Message AuthenticateHeadManager(string bankId, string headManagerAccountId, string headManagerPassword)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<HeadManager> headManagers = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].HeadManagers;
                if (headManagers.Count > 0)
                {
                    byte[] salt = new byte[32];
                    var headManager = headManagers.Find(hm => hm.AccountId.Equals(headManagerAccountId));
                    if (headManager is not null)
                    {
                        salt = headManager.Salt;
                    }

                    byte[] hashedPasswordToCheck = _encryptionService.HashPassword(headManagerPassword, salt);
                    string hashedPassword = Convert.ToBase64String(hashedPasswordToCheck);
                    bool isHeadManagerAvilable = headManagers.Any(hm => hm.AccountId.Equals(headManagerAccountId) && Convert.ToBase64String(hm.HashedPassword).Equals(hashedPassword) && hm.IsActive == 1);
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
            else
            {
                message.Result = false;
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }

        public Message OpenHeadManagerAccount(string bankId, string headManagerName, string headManagerPassword)
        {
            Message message = new();
            banks = _fileService.GetData();
            List<HeadManager> headManagers = banks[banks.FindIndex(obj => obj.BankId.Equals(bankId))].HeadManagers;
            headManagers ??= new List<HeadManager>();
            bool isHeadManagerAlreadyAvailabe = headManagers.Any(hm => hm.Name.Equals(headManagerName) && hm.IsActive == 1);
            if (!isHeadManagerAlreadyAvailabe)
            {
                string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                string UserFirstThreeCharecters = headManagerName.Substring(0, 3);
                string bankHeadManagerAccountId = string.Concat(UserFirstThreeCharecters,date);

                byte[] salt = _encryptionService.GenerateSalt();
                byte[] hashedPassword = _encryptionService.HashPassword(headManagerPassword, salt);

                HeadManager headManager = new()
                {
                    Name = headManagerName,
                    Salt = salt,
                    HashedPassword = hashedPassword,
                    AccountId = bankHeadManagerAccountId,
                    IsActive = 1
                };

                headManagers.Add(headManager);
                banks[banks.FindIndex(obj => obj.BankId.Equals(bankId))].HeadManagers = headManagers;

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
            Message message = new();
            banks = _fileService.GetData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    List<HeadManager> headManagers = bank.HeadManagers;
                    if (headManagers is not null)
                    {
                        bool isManagerAvilable = headManagers.Any(m => m.AccountId.Equals(headManagerAccountId) && m.IsActive == 1);
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
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Bank Not Found";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }
        public Message UpdateHeadManagerAccount(string bankId, string headManagerAccountId, string headManagerName, string headManagerPassword)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<Branch> branches = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Branches;
                if (branches is not null)
                {
                    List<HeadManager> headManagers = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].HeadManagers;
                    var headManager = headManagers.Find(hm => hm.AccountId.Equals(headManagerAccountId));
                    if (headManager is not null)
                    {
                        if (!string.IsNullOrEmpty(headManagerName))
                        {
                            headManager.Name = headManagerName;
                        }

                        if (!string.IsNullOrEmpty(headManagerPassword))
                        {
                            byte[] salt = new byte[32];
                            salt = headManager.Salt;
                            byte[] hashedPasswordToCheck = _encryptionService.HashPassword(headManagerPassword, salt);
                            if (Convert.ToBase64String(headManager.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck)))
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
                        if (string.IsNullOrEmpty(headManagerName) && string.IsNullOrEmpty(headManagerPassword))
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
                else
                {
                    message.Result = false;
                    message.ResultMessage = "No Branches Available in Bank";

                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }
        public Message DeleteHeadManagerAccount(string bankId, string headManagerAccountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<Branch> branches = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Branches;
                if (branches is not null)
                {
                    List<HeadManager> headManagers = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].HeadManagers;
                    var headManager = headManagers.Find(hm => hm.AccountId.Equals(headManagerAccountId));
                    if (headManager is not null)
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
                else
                {
                    message.Result = false;
                    message.ResultMessage = "No Branches Available in Bank";
                } 
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }

        public string GetHeadManagerDetails(string bankId, string headManagerAccountId)
        {
            string data = string.Empty;
            Message message = new();
            banks = _fileService.GetData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                List<Branch> branches = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Branches;
                if (branches is not null)
                {
                    message = IsHeadManagerExist(bankId, headManagerAccountId);
                    string headManagerDetails = string.Empty;
                    if (message.Result)
                    {
                        int bankIndex = banks.FindIndex(b => b.BankId.Equals(bankId));
                        int headManagerIndex = banks[bankIndex].HeadManagers.FindIndex(hm => hm.AccountId.Equals(headManagerAccountId));

                        HeadManager details = banks[bankIndex].HeadManagers[headManagerIndex];
                        headManagerDetails = details.ToString() ?? string.Empty;
                        data = headManagerDetails;
                    }
                    else
                    {
                        data = "Head Manager Doesnt Exist";
                    }
                }
                else
                {
                    data = "No Branches Available in Bank";
                }
            }
            else
            {
                data = "Invalid BanKId";
            }
            return data;
        }
    }
}
