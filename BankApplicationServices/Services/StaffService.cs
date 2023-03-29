using BankApplicationModels;
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
            banks = _fileService.GetData();
        }

        Message message = new Message();
        public Message AuthenticateStaffAccount(string bankId, string branchId,
           string staffAccountId, string staffAccountPassword)
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
                            bool isManagerAvilable = staffs.Any(m => m.AccountId == staffAccountId && m.HashedPassword == hashedPasswordToCheck && m.IsActive == 1);
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

        public Message OpenStaffAccount(string bankId, string branchId, string staffName, string staffPassword, ushort staffRole)
        {
            message = _branchService.AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                bool isManagerAlreadyAvailabe = false;
                List<Staff>? staffs = null;
                List<Branch> branches = banks[banks.FindIndex(obj => obj.BankId == bankId)].Branches;
                if (branches != null)
                {
                    staffs = branches[branches.FindIndex(br => br.BranchId == branchId)].Staffs;
                    isManagerAlreadyAvailabe = staffs.Any(m => m.Name == staffName && m.IsActive == 1);
                }

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
                        IsActive = 1
                    };

                    if (staffs == null)
                    {
                        staffs = new List<Staff>();
                    }

                    staffs.Add(staff);
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
                        if (staff.HashedPassword == hashedPasswordToCheck)
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
