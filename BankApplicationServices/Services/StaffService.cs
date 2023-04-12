using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class StaffService : IStaffService
    {
        private readonly IFileService _fileService;
        private readonly IBranchService _branchService;
        private readonly IEncryptionService _encryptionService;

        List<Bank> banks;
        public StaffService(IFileService fileService, IBranchService branchService, IEncryptionService encryptionService)
        {
            _fileService = fileService;
            _branchService = branchService;
            _encryptionService = encryptionService;
            banks = new List<Bank>();
        }

        public Task<Message> IsStaffExistAsync(string bankId, string branchId)
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
                        List<Staff> staffs = branch.Staffs;
                        if (staffs is null)
                        {
                            staffs = new List<Staff>();
                            staffs.FindAll(s => s.IsActive.Equals(1));
                        }

                        if (staffs is not null && staffs.Count > 0)
                        {
                            message.Result = true;
                            message.ResultMessage = "Staff Exist in Branch";
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"No Staff Available In The Branch:{branchId}";
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
        public Task<Message> AuthenticateStaffAccountAsync(string bankId, string branchId,
           string staffAccountId, string staffAccountPassword)
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
                        List<Staff> staffs = branch.Staffs;
                        if (staffs is not null)
                        {
                            byte[] salt = new byte[32];
                            var staff = staffs.Find(m => m.AccountId.Equals(staffAccountId));
                            if (staff is not null)
                            {
                                salt = staff.Salt;
                            }

                            byte[] hashedPasswordToCheck = _encryptionService.HashPassword(staffAccountPassword, salt);
                            bool isManagerAvilable = staffs.Any(m => m.AccountId.Equals(staffAccountId) && Convert.ToBase64String(m.HashedPassword).Equals(Convert.ToBase64String(hashedPasswordToCheck)) && m.IsActive == 1);
                            if (isManagerAvilable)
                            {
                                message.Result = true;
                                message.ResultMessage = "Staff Validation Successful.";
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = "Staff Validation Failed.";
                            }
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"No Staff Available In The Branch:{branchId}";
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

        public Task<Message> OpenStaffAccountAsync(string bankId, string branchId, string staffName, string staffPassword, StaffRole staffRole)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Staff>? staffs = null;
                List<Branch> branches = banks[banks.FindIndex(obj => obj.BankId.Equals(bankId))].Branches;
                if (branches is not null)
                {
                    staffs = branches[branches.FindIndex(br => br.BranchId.Equals(branchId))].Staffs;
                }

                if (staffs is null)
                {
                    staffs = new List<Staff>();
                }
                bool isManagerAlreadyAvailabe = staffs.Any(m => m.Name.Equals(staffName) && m.IsActive == 1);

                if (!isManagerAlreadyAvailabe)
                {
                    string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string UserFirstThreeCharecters = staffName.Substring(0, 3);
                    string staffAccountId = string.Concat(UserFirstThreeCharecters, date);

                    byte[] salt = _encryptionService.GenerateSalt();
                    byte[] hashedPassword = _encryptionService.HashPassword(staffPassword, salt);

                    Staff staff = new()
                    {
                        Name = staffName,
                        Salt = salt,
                        HashedPassword = hashedPassword,
                        AccountId = staffAccountId,
                        Role = staffRole,
                        IsActive = 1
                    };

                    staffs.Add(staff);
                    if (branches is not null)
                    {
                        banks[banks.FindIndex(obj => obj.BankId == bankId)].Branches[branches.FindIndex(br => br.BranchId == branchId)].Staffs = staffs;
                    }

                    _fileService.WriteFile(banks);
                    message.Result = true;
                    message.ResultMessage = $"Account Created for {staffName} with Account Id:{staffAccountId}";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Staff: {staffName} Already Existed";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public Task<Message> IsAccountExistAsync(string bankId, string branchId, string staffAccountId)
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
                        List<Staff> staffs = branch.Staffs;
                        if (staffs is not null)
                        {
                            bool isStaffAvilable = staffs.Any(m => m.AccountId.Equals(staffAccountId) && m.IsActive == 1);
                            if (isStaffAvilable)
                            {
                                message.Result = true;
                                message.ResultMessage = "Staff Validation Successful.";
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = "Staff Validation Failed.";
                            }
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"No Staff Available In The Branch:{branchId}";
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

        public Task<Message> UpdateStaffAccountAsync(string bankId, string branchId, string staffAccountId, string staffName, string staffPassword, ushort staffRole)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Staff>? staffs = null;
                Staff? staff = null;
                List<Branch> branches = banks[banks.FindIndex(obj => obj.BankId.Equals(bankId))].Branches;
                if (branches is not null)
                {
                    staffs = branches[branches.FindIndex(br => br.BranchId.Equals(branchId))].Staffs;
                    if (staffs is not null)
                    {
                        staff = staffs.Find(m => m.AccountId.Equals(staffAccountId) && m.IsActive == 1);
                    }
                }

                if (staff is not null)
                {
                    if (!string.IsNullOrEmpty(staffName))
                    {
                        staff.Name = staffName;
                    }

                    if (!string.IsNullOrEmpty(staffPassword))
                    {
                        byte[] salt = new byte[32];
                        salt = staff.Salt;
                        byte[] hashedPasswordToCheck = _encryptionService.HashPassword(staffPassword, salt);
                        if (Convert.ToBase64String(staff.HashedPassword) == Convert.ToBase64String(hashedPasswordToCheck))
                        {
                            message.Result = false;
                            message.ResultMessage = "New password Matches with the Old Password.,Provide a New Password";
                        }
                        else
                        {
                            salt = _encryptionService.GenerateSalt();
                            byte[] hashedPassword = _encryptionService.HashPassword(staffPassword, salt);
                            staff.Salt = salt;
                            staff.HashedPassword = hashedPassword;
                            message.Result = true;
                            message.ResultMessage = "Updated Password Sucessfully";
                        }
                    }
                    if (string.IsNullOrEmpty(staffName) && string.IsNullOrEmpty(staffPassword))
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
                    message.ResultMessage = $"Head Manager: {staffName} with AccountId:{staffAccountId} Doesn't Exist";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public Task<Message> DeleteStaffAccountAsync(string bankId, string branchId, string staffAccountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Staff>? staffs = null;
                Staff? staff = null;
                List<Branch> branches = banks[banks.FindIndex(obj => obj.BankId.Equals(bankId))].Branches;
                if (branches is not null)
                {
                    staffs = branches[branches.FindIndex(br => br.BranchId.Equals(branchId))].Staffs;
                    if (staffs is not null)
                    {
                        staff = staffs.Find(m => m.AccountId.Equals(staffAccountId) && m.IsActive == 1);
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"No Branches Found";
                }

                if (staff is not null)
                {
                    staff.IsActive = 0;
                    message.Result = true;
                    message.ResultMessage = $"Deleted AccountId:{staffAccountId} Successfully.";
                    _fileService.WriteFile(banks);
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"{staffAccountId} Doesn't Exist.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "BranchId Authentication Failed";
            }
            return message;
        }

        public Task<Message> GetStaffDetailsAsync(string bankId, string branchId, string staffAccountId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = IsAccountExist(bankId, branchId, staffAccountId);
            string staffDetails = string.Empty;
            if (message.Result)
            {
                int bankIndex = banks.FindIndex(b => b.BankId.Equals(bankId));
                int branchIndex = banks[bankIndex].Branches.FindIndex(br => br.BranchId.Equals(branchId));
                int staffIndex = banks[bankIndex].Branches[branchIndex].Staffs.FindIndex(c => c.AccountId.Equals(staffAccountId));
                Staff details = banks[bankIndex].Branches[branchIndex].Staffs[staffIndex];
                staffDetails = details.ToString();
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Account Doesn't Exist";
            }
            return staffDetails;
        }
    }
}
