
using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class ManagerService : IManagerService
    {
        IFileService _fileService;
        IBranchService _branchService;
        List<Bank> banks;
        IEncryptionService _encryptionService;
        public ManagerService(IFileService fileService, IEncryptionService encryptionService,
            IBranchService branchService)
        {
            _fileService = fileService;
            _encryptionService = encryptionService;
            _branchService = branchService;
            banks = _fileService.GetData();
        }
        Message message = new Message();
        public Message AuthenticateManagerAccount(string bankId, string branchId,
            string managerAccountId, string managerPassword)
        {
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        List<Manager> managers = branch.Managers;
                        if (managers != null)
                        {
                            byte[] salt = new byte[32];
                            var manager = managers.Find(m => m.AccountId == managerAccountId);
                            if (manager != null)
                            {
                                salt = manager.Salt;
                            }

                            byte[] hashedPasswordToCheck = _encryptionService.HashPassword(managerPassword, salt);
                            bool isManagerAvilable = managers.Any(m => m.AccountId == managerAccountId && m.HashedPassword == hashedPasswordToCheck && m.IsActive == 1);
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
                }
            }
            return message;
        }


        public Message OpenManagerAccount(string bankId, string branchId, string managerName, string managerPassword)
        {
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                bool isManagerAlreadyAvailabe = false;
                List<Manager>? managers = null;
                List<Branch> branches = banks[banks.FindIndex(obj => obj.BankId == bankId)].Branches;
                if(branches != null)
                {
                    managers = branches[branches.FindIndex(br => br.BranchId == branchId)].Managers;
                    isManagerAlreadyAvailabe = managers.Any(m => m.Name == managerName && m.IsActive == 1);
                }

                if (!isManagerAlreadyAvailabe)
                {
                    DateTime currentDate = DateTime.Now;
                    string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                    string UserFirstThreeCharecters = managerName.Substring(0, 3);
                    string managerAccountId = UserFirstThreeCharecters + date;

                    byte[] salt = _encryptionService.GenerateSalt();
                    byte[] hashedPassword = _encryptionService.HashPassword(managerPassword, salt);

                    Manager manager = new Manager()
                    {
                        Name = managerName,
                        Salt = salt,
                        HashedPassword = hashedPassword,
                        AccountId = managerAccountId,
                        IsActive = 1
                    };

                    if (managers == null)
                    {
                        managers = new List<Manager>();
                    }

                    managers.Add(manager);
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
            return message;
        }

        public Message UpdateManagerAccount(string bankId, string branchId,string accountId, string managerName, string managerPassword)
        {
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Manager>? managers = null;
                Manager? manager = null;
                List<Branch> branches = banks[banks.FindIndex(obj => obj.BankId == bankId)].Branches;
                if (branches != null)
                {
                    managers = branches[branches.FindIndex(br => br.BranchId == branchId)].Managers;
                    if(managers != null)
                    {
                      manager = managers.Find(m => m.AccountId == accountId && m.IsActive == 1);
                    }
                }

                if (manager != null)
                {
                    if (managerName != string.Empty)
                    {
                        manager.Name = managerName;
                    }

                    if (managerPassword != string.Empty)
                    {
                        byte[] salt = new byte[32];
                        salt = manager.Salt;
                        byte[] hashedPasswordToCheck = _encryptionService.HashPassword(managerPassword, salt);
                        if (manager.HashedPassword == hashedPasswordToCheck)
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
                    _fileService.WriteFile(banks);
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Manager: {managerName} with AccountId:{accountId} Doesn't Exist";
                }
            }
            return message;
        }

        public Message DeleteManagerAccount(string bankId, string branchId,string accountId)
        {
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Manager>? managers = null;
                Manager? manager = null;
                List<Branch> branches = banks[banks.FindIndex(obj => obj.BankId == bankId)].Branches;
                if (branches != null)
                {
                    managers = branches[branches.FindIndex(br => br.BranchId == branchId)].Managers;
                    if (managers != null)
                    {
                        manager = managers.Find(m => m.AccountId == accountId && m.IsActive == 1);
                    }
                }
                
                if (manager != null)
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
            return message;
        }
    }
}
