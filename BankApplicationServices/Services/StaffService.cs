using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class StaffService : IStaffService
    {
        IFileService _fileService;
        IBranchCustomerService _branchCustomerService;
        List<Bank> banks;
        public StaffService(IFileService fileService, IBranchCustomerService branchCustomerService)
        {
            _fileService = fileService;
            this.banks = _fileService.GetData();
            this._branchCustomerService = branchCustomerService;
        }
        private static int bankObjectIndex = 0;
        private static int branchObjectIndex = 0;
        private static string staffBankId = string.Empty;
        private static string staffBranchId = string.Empty;

        Message message = new Message();
        public Message AuthenticateBranchStaffAccount(string bankId, string branchid,
           string staffAccountId, string staffAccountPassword)
        {

            if (banks.Count > 0)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchid);
                    if (branch != null)
                    {
                        bankObjectIndex = banks.IndexOf(bank);
                        var staff = branch.Staffs.FirstOrDefault(s => s.StaffAccountId == staffAccountId && s.StaffPassword == staffAccountPassword);
                        if (staff != null)
                        {
                            staffBankId = bankId;
                            staffBranchId = branchid;

                            message.Result = true;
                            message.ResultMessage = $"Validation Succesful";

                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"Validation Failed! Please check Accound Id and Password";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = $"Entered branchId '{branchid}' is not found. Please Check Again.";
                    }
                    branchObjectIndex = banks[bankObjectIndex].Branches.FindIndex(branch => branch.BranchId == branchid);
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Entered bankId '{bankId}' is not found. Please Check Again.";
                }
            }
            return message;
        }

        public Message OpenStaffAccount(string bankId, string branchId, string staffName, string staffPassword, ushort staffRole)
        {

            bool isStaffAlreadyRegistered = false;
            if (banks[bankObjectIndex].Branches[branchObjectIndex].Staffs == null)
            {
                banks[bankObjectIndex].Branches[branchObjectIndex].Staffs = new List<Staff>();
            }
            isStaffAlreadyRegistered = banks[bankObjectIndex].Branches[branchObjectIndex].Staffs.Any(sn => sn.StaffName == staffName);
            if (isStaffAlreadyRegistered)
            {
                Console.WriteLine($"Staff Member {staffName} is already Registered");
                message.Result = false;
            }
            else
            {
                DateTime currentDate = DateTime.Now;
                string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                string UserFirstThreeCharecters = staffName.Substring(0, 3);
                string bankStaffAccountId = UserFirstThreeCharecters + date;

                Staff bankManager = new Staff()
                {
                    StaffAccountId = bankStaffAccountId,
                    StaffName = staffName,
                    StaffPassword = staffPassword,
                    Role = (StaffRole)staffRole
                };

                banks[bankObjectIndex].Branches[branchObjectIndex].Staffs.Add(bankManager);
                _fileService.WriteFile(banks);

                message.Result = true;

            }
            return message;
        }

        public Message UpdateStaffAccount(string bankId, string branchId, string staffAccountId, string staffName, string staffPassword, ushort staffRole)
        {

        }

        public Message DeleteStaffAccount(string bankId, string branchId,, string staffAccountId)
        {

        }
