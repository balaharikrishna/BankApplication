using BankApplicationModels;
using BankApplicationRepository.IRepository;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class HeadManagerService : IHeadManagerService
    {
        private readonly IBankService _bankService;
        private readonly IEncryptionService _encryptionService;
        IHeadManagerRepository _headManagerRepository;

        public HeadManagerService(IHeadManagerRepository headManagerRepository, IBankService bankService, IEncryptionService encryptionService)
        {
            _bankService = bankService;
            _encryptionService = encryptionService;
            _headManagerRepository = headManagerRepository;
        }

        public async Task<Message> IsHeadManagersExistAsync(string bankId)
        {
            Message message;
            message = await _bankService.AuthenticateBankIdAsync(bankId);
            if (message.Result)
            {
               IEnumerable<HeadManager> headManagers = await _headManagerRepository.GetAllHeadManagers(bankId);

                if (headManagers.Any())
                {
                    message.Result = true;
                    message.ResultMessage = $"Head Managers Exist in The Bank:{bankId}";
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
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }
        public async Task<Message> AuthenticateHeadManagerAsync(string bankId, string headManagerAccountId, string headManagerPassword)
        {
            Message message;
            message = await _bankService.AuthenticateBankIdAsync(bankId);
            if (message.Result)
            {
                IEnumerable<HeadManager> headManagers = await _headManagerRepository.GetAllHeadManagers(bankId);
                if (headManagers.Any())
                {
                    byte[] salt = new byte[32];
                    HeadManager headManager = await _headManagerRepository.GetHeadManagerById(headManagerAccountId, bankId);
                    if (headManager is not null)
                    {
                        salt = headManager.Salt;
                    }

                    byte[] hashedPasswordToCheck = _encryptionService.HashPassword(headManagerPassword, salt);
                    bool isValidPassword = Convert.ToBase64String(headManager.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck));
                    if (isValidPassword)
                    {
                        message.Result = true;
                        message.ResultMessage = "Manager Validation Successful.";
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Manager Validation Failed.";
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
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }

        public async Task<Message> OpenHeadManagerAccountAsync(string bankId, string headManagerName, string headManagerPassword)
        {
            Message message = new();
            HeadManager headManager = await _headManagerRepository.GetHeadManagerByName(headManagerName, bankId);
            
            bool isHeadManagerAlreadyAvailabe = headManager.Name.Equals(headManagerName);
            if (!isHeadManagerAlreadyAvailabe)
            {
                string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                string UserFirstThreeCharecters = headManagerName.Substring(0,3);
                string bankHeadManagerAccountId = string.Concat(UserFirstThreeCharecters, date);

                byte[] salt = _encryptionService.GenerateSalt();
                byte[] hashedPassword = _encryptionService.HashPassword(headManagerPassword, salt);

                HeadManager headManagerObject = new()
                {
                    Name = headManagerName,
                    Salt = salt,
                    HashedPassword = hashedPassword,
                    AccountId = bankHeadManagerAccountId,
                    IsActive = true
                };

                bool isHeadManagerAdded = await _headManagerRepository.AddHeadManagerAccount(headManagerObject, bankId);
                if(isHeadManagerAdded)
                {
                    message.Result = true;
                    message.ResultMessage = $"Account Created for {headManagerName} with Account Id:{bankHeadManagerAccountId}";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Failed to Create HeadManager Account for {headManagerName}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Head Manager: {headManagerName} Already Existed";
            }
            return message;
        }

        public async Task<Message> IsHeadManagerExistAsync(string bankId, string headManagerAccountId)
        {
            Message message;
            message = await _bankService.AuthenticateBankIdAsync(bankId);
            if (message.Result)
            {
                message = await IsHeadManagersExistAsync(bankId);
                if (message.Result)
                {
                    bool isHeadManagerExist = await _headManagerRepository.IsHeadManagerExist(headManagerAccountId, bankId);
                    if (isHeadManagerExist)
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
                message.ResultMessage = "BankId Authentication Failed";
            }
            return message;
        }
        public async Task<Message> UpdateHeadManagerAccountAsync(string bankId, string headManagerAccountId, string headManagerName, string headManagerPassword)
        {
            Message message = new();
            bool isHeadManagerExist = await _headManagerRepository.IsHeadManagerExist(headManagerAccountId, bankId);
            if (isHeadManagerExist)
            {
              HeadManager headManager = await _headManagerRepository.GetHeadManagerById(headManagerAccountId, bankId);

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

           
                HeadManager headManager = new()
              {
                  AccountId = headManagerAccountId,
                  Salt = 
              }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Head Manager Validation Failed.";
            }

            List<Branch> branches = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Branches;
            if (branches is not null)
            {
                List<HeadManager> headManagers = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].HeadManagers;
                var headManager = headManagers.Find(hm => hm.AccountId.Equals(headManagerAccountId));
                if (headManager is not null)
                {
                    

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
            return message;
        }
        public Task<Message> DeleteHeadManagerAccountAsync(string bankId, string headManagerAccountId)
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

        public Task<Message> GetHeadManagerDetailsAsync(string bankId, string headManagerAccountId)
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
