using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IFileService _fileService;
        private readonly IBranchService _branchService;
        private readonly IEncryptionService _encryptionService;
        List<Bank> banks;

        public ManagerService(IFileService fileService, IEncryptionService encryptionService,
            IBranchService branchService)
        {
            _fileService = fileService;
            _encryptionService = encryptionService;
            _branchService = branchService;
            banks = new List<Bank>();
        }

        public Message IsManagersExist(string bankId, string branchId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                    if (branch is not null)
                    {
                        List<Manager> managers = branch.Managers;
                        if (managers is null)
                        {
                            managers = new List<Manager>();
                            managers.FindAll(m => m.IsActive.Equals(1));
                        }

                        if (managers is not null && managers.Count > 0)
                        {
                            message.Result = true;
                            message.ResultMessage = "Managers Exist in Branch";
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"No Managers Available In The Branch:{branchId}";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Branch Not Found";
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
                message.ResultMessage = "BranchId Authentication Failed";
            }

            return message;
        }

        public Message AuthenticateManagerAccount(string bankId, string branchId,
            string managerAccountId, string managerPassword)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                    if (branch is not null)
                    {
                        List<Manager> managers = branch.Managers;
                        if (managers is not null)
                        {
                            byte[] salt = new byte[32];
                            var manager = managers.Find(m => m.AccountId.Equals(managerAccountId));
                            if (manager is not null)
                            {
                                salt = manager.Salt;
                            }

                            byte[] hashedPasswordToCheck = _encryptionService.HashPassword(managerPassword, salt);
                            bool isManagerAvilable = managers.Any(m => m.AccountId.Equals(managerAccountId) && Convert.ToBase64String(m.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck)) && m.IsActive == 1);
                            if (isManagerAvilable)
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
                            message.ResultMessage = $"No Managers Available In The Branch:{branchId}";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Branch Not Found";
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
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public Message OpenManagerAccount(string bankId, string branchId, string managerName, string managerPassword)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Manager>? managers = null;
                List<Branch> branches = banks[banks.FindIndex(obj => obj.BankId.Equals(bankId))].Branches;
                if (branches is not null)
                {
                    managers = branches[branches.FindIndex(br => br.BranchId.Equals(branchId))].Managers;
                }

                managers ??= new List<Manager>();
                bool isManagerAlreadyAvailabe = managers.Any(m => m.Name.Equals(managerName) && m.IsActive == 1);

                if (!isManagerAlreadyAvailabe)
                {
                    string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string UserFirstThreeCharecters = managerName.Substring(0, 3);
                    string managerAccountId = UserFirstThreeCharecters + date;

                    byte[] salt = _encryptionService.GenerateSalt();
                    byte[] hashedPassword = _encryptionService.HashPassword(managerPassword, salt);

                    Manager manager = new()
                    {
                        Name = managerName,
                        Salt = salt,
                        HashedPassword = hashedPassword,
                        AccountId = managerAccountId,
                        IsActive = 1
                    };

                    managers.Add(manager);
                    if (branches is not null)
                    {
                        banks[banks.FindIndex(obj => obj.BankId.Equals(bankId))].Branches[branches.FindIndex(br => br.BranchId.Equals(branchId))].Managers = managers;
                    }
                    _fileService.WriteFile(banks);
                    message.Result = true;
                    message.ResultMessage = $"Account Created for {managerName} with Account Id:{managerAccountId}";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Manager: {managerName} Already Existed";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public Message IsAccountExist(string bankId, string branchId, string managerAccountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId.Equals(bankId));
                if (bank is not null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId.Equals(branchId));
                    if (branch is not null)
                    {
                        List<Manager> managers = branch.Managers;
                        if (managers is not null)
                        {
                            bool isManagerAvilable = managers.Any(m => m.AccountId.Equals(managerAccountId) && m.IsActive == 1);
                            if (isManagerAvilable)
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
                            message.ResultMessage = $"No Manager Available In The Branch:{branchId}";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "Branch Not Found";
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
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public Message UpdateManagerAccount(string bankId, string branchId, string accountId, string managerName, string managerPassword)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Manager>? managers = null;
                Manager? manager = null;
                List<Branch> branches = banks[banks.FindIndex(obj => obj.BankId.Equals(bankId))].Branches;
                if (branches is not null)
                {
                    managers = branches[branches.FindIndex(br => br.BranchId.Equals(branchId))].Managers;
                    if (managers is not null)
                    {
                        manager = managers.Find(m => m.AccountId.Equals(accountId) && m.IsActive == 1);
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "No Branches Available.";
                }

                if (manager is not null)
                {
                    if (!string.IsNullOrEmpty(managerName))
                    {
                        manager.Name = managerName;
                    }

                    if (!string.IsNullOrEmpty(managerPassword))
                    {
                        byte[] salt = new byte[32];
                        salt = manager.Salt;
                        byte[] hashedPasswordToCheck = _encryptionService.HashPassword(managerPassword, salt);
                        if (Convert.ToBase64String(manager.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck)))
                        {
                            message.Result = false;
                            message.ResultMessage = "New password Matches with the Old Password.,Provide a New Password";
                        }
                        else
                        {
                            salt = _encryptionService.GenerateSalt();
                            byte[] hashedPassword = _encryptionService.HashPassword(managerPassword, salt);
                            manager.Salt = salt;
                            manager.HashedPassword = hashedPassword;
                            message.Result = true;
                            message.ResultMessage = "Updated Password Sucessfully";
                        }
                    }

                    if (string.IsNullOrEmpty(managerName) && string.IsNullOrEmpty(managerPassword))
                    {
                        message.Result = true;
                        message.ResultMessage = $"No Changes Added.";
                    }
                    else
                    {
                        message.Result = true;
                        message.ResultMessage = $"Updated Details Successfully";
                    }
                    _fileService.WriteFile(banks);
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Manager: {managerName} with AccountId:{accountId} Doesn't Exist";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public Message DeleteManagerAccount(string bankId, string branchId, string accountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Manager>? managers = null;
                Manager? manager = null;
                List<Branch> branches = banks[banks.FindIndex(obj => obj.BankId.Equals(bankId))].Branches;
                if (branches is not null)
                {
                    managers = branches[branches.FindIndex(br => br.BranchId.Equals(branchId))].Managers;
                    if (managers is not null)
                    {
                        manager = managers.Find(m => m.AccountId.Equals(accountId) && m.IsActive == 1);
                    }
                }

                if (manager is not null)
                {
                    manager.IsActive = 0;
                    message.Result = true;
                    message.ResultMessage = $"Deleted AccountId:{accountId} Successfully.";
                    _fileService.WriteFile(banks);
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"{accountId} Doesn't Exist.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public string GetManagerDetails(string bankId, string branchId, string managerAccountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = IsAccountExist(bankId, branchId, managerAccountId);
            string managerDetails = string.Empty;
            if (message.Result)
            {
                int bankIndex = banks.FindIndex(b => b.BankId.Equals(bankId));
                int branchIndex = banks[bankIndex].Branches.FindIndex(br => br.BranchId.Equals(branchId));
                int managerIndex = banks[bankIndex].Branches[branchIndex].Managers.FindIndex(c => c.AccountId.Equals(managerAccountId));
                Manager details = banks[bankIndex].Branches[branchIndex].Managers[managerIndex];
                managerDetails = details.ToString() ?? string.Empty;
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Account Not Exist";
            }
            return managerDetails;
        }
    }
}
