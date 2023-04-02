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
        Message message = new Message();
        List<Bank> banks;
        public StaffService(IFileService fileService, IBranchService branchService, IEncryptionService encryptionService)
        {
            _fileService = fileService;
            _branchService = branchService;
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
        

        public Message IsStaffExist(string bankId, string branchId)
        {
            GetBankData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        List<Staff> staffs = branch.Staffs; 
                        if(staffs == null)
                        {
                            staffs = new List<Staff>();
                            staffs.FindAll(s => s.IsActive == 1);
                        }
                        if (staffs != null && staffs.Count > 0)
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
                }
            }
            return message;
        }
        public Message AuthenticateStaffAccount(string bankId, string branchId,
           string staffAccountId, string staffAccountPassword)
        {
            GetBankData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        List<Staff> staffs = branch.Staffs;
                        if (staffs != null)
                        {
                            byte[] salt = new byte[32];
                            var staff = staffs.Find(m => m.AccountId == staffAccountId);
                            if (staff != null)
                            {
                                salt = staff.Salt;
                            }

                            byte[] hashedPasswordToCheck = _encryptionService.HashPassword(staffAccountPassword, salt);
                            bool isManagerAvilable = staffs.Any(m => m.AccountId == staffAccountId && Convert.ToBase64String(m.HashedPassword) == Convert.ToBase64String(hashedPasswordToCheck) && m.IsActive == 1);
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
                }
            }
            return message;
        }

        public Message OpenStaffAccount(string bankId, string branchId, string staffName, string staffPassword, StaffRole staffRole)
        {
            GetBankData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Staff>? staffs = null;
                List<Branch> branches = banks[banks.FindIndex(obj => obj.BankId == bankId)].Branches;
                if (branches != null)
                {
                    staffs = branches[branches.FindIndex(br => br.BranchId == branchId)].Staffs;
                }

                if (staffs == null)
                {
                    staffs = new List<Staff>();
                }
                bool isManagerAlreadyAvailabe = staffs.Any(m => m.Name == staffName && m.IsActive == 1);

                if (!isManagerAlreadyAvailabe)
                {
                    DateTime currentDate = DateTime.Now;
                    string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                    string UserFirstThreeCharecters = staffName.Substring(0, 3);
                    string staffAccountId = UserFirstThreeCharecters + date;

                    byte[] salt = _encryptionService.GenerateSalt();
                    byte[] hashedPassword = _encryptionService.HashPassword(staffPassword, salt);

                    Staff staff = new Staff()
                    {
                        Name = staffName,
                        Salt = salt,
                        HashedPassword = hashedPassword,
                        AccountId = staffAccountId,
                        Role = staffRole,
                        IsActive = 1
                    };

                    staffs.Add(staff);
                    if (branches != null)
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
            return message;
        }

        public Message IsAccountExist(string bankId, string branchId, string staffAccountId)
        {
            GetBankData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        List<Staff> staffs = branch.Staffs;
                        if (staffs != null)
                        {
                            bool isStaffAvilable = staffs.Any(m => m.AccountId == staffAccountId && m.IsActive == 1);
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
                }
            }
            return message;
        }



        public Message UpdateStaffAccount(string bankId, string branchId, string staffAccountId, string staffName, string staffPassword, ushort staffRole)
        {
            GetBankData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Staff>? staffs = null;
                Staff? staff = null;
                List<Branch> branches = banks[banks.FindIndex(obj => obj.BankId == bankId)].Branches;
                if (branches != null)
                {
                    staffs = branches[branches.FindIndex(br => br.BranchId == branchId)].Staffs;
                    if (staffs != null)
                    {
                        staff = staffs.Find(m => m.AccountId == staffAccountId && m.IsActive == 1);
                    }
                }

                if (staff != null)
                {
                    if (staffName != string.Empty)
                    {
                        staff.Name = staffName;
                    }

                    if (staffPassword != string.Empty)
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
                    if (staffName == string.Empty && staffPassword == string.Empty)
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
            return message;
        }

        public Message DeleteStaffAccount(string bankId, string branchId, string staffAccountId)
        {
            GetBankData();
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Staff>? staffs = null;
                Staff? staff = null;
                List<Branch> branches = banks[banks.FindIndex(obj => obj.BankId == bankId)].Branches;
                if (branches != null)
                {
                    staffs = branches[branches.FindIndex(br => br.BranchId == branchId)].Staffs;
                    if (staffs != null)
                    {
                        staff = staffs.Find(m => m.AccountId == staffAccountId && m.IsActive == 1);
                    }
                }

                if (staff != null)
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
            return message;
        }

        public string GetStaffDetails(string bankId, string branchId, string staffAccountId)
        {
            GetBankData();
            message = IsAccountExist(bankId, branchId, staffAccountId);
            string staffDetails = string.Empty;
            if (message.Result)
            {
                int bankIndex = banks.FindIndex(b => b.BankId == bankId);
                int branchIndex = banks[bankIndex].Branches.FindIndex(br => br.BranchId == branchId);
                int staffIndex = banks[bankIndex].Branches[branchIndex].Staffs.FindIndex(c => c.AccountId == staffAccountId);
                Staff details = banks[bankIndex].Branches[branchIndex].Staffs[staffIndex];
                staffDetails =  details.ToString()??string.Empty;
            }
            return staffDetails;
        }
    }
}
