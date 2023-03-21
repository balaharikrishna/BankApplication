
using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.Interfaces;

namespace BankApplicationServices.Services
{
    public class BranchManagerService : IBranchManagerService
    {
        private static int bankObjectIndex = 0;
        private static int branchObjectIndex = 0;

        IFileService _fileService;
        List<Bank> banks;
        public BranchManagerService(IFileService fileService)
        {
            _fileService = fileService;
            this.banks = _fileService.GetData();
        }
        Message message = new Message();
        public Message ValidateBranchManagerAccount(string bankId, string branchId,
            string branchManagerAccountId, string branchManagerPassword)
        {
            if (banks.Count > 0)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.BranchId == branchId);
                    if (branch != null)
                    {
                        bankObjectIndex = banks.IndexOf(bank);
                        var staff = branch.Staffs.FirstOrDefault(s => s.StaffAccountId == branchManagerAccountId && s.StaffPassword == branchManagerPassword);
                        if (staff != null)
                        {
                            message.Result = true;
                            message.ResultMessage = "Validation Succesful";
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "Validation Failed! Please check Accound Id and Password";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = $"Entered branchId '{branchId}' is not found. Please Check Again.";
                    }
                    branchObjectIndex = banks[bankObjectIndex].Branches.FindIndex(branch => branch.BranchId == branchId);
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Entered bankId '{bankId}' is not found. Please Check Again.";
                }
            }
            return message;
        }

        public Message AddTransactionCharges(ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank)
        {
            if (branchObjectIndex != -1)
            {
                TransactionCharges transactionCharges = new TransactionCharges()
                {
                    RtgsSameBank = rtgsSameBank,
                    RtgsOtherBank = rtgsOtherBank,
                    ImpsSameBank = impsSameBank,
                    ImpsOtherBank = impsOtherBank,

                };

                if (banks[bankObjectIndex].Branches[branchObjectIndex].Charges == null)
                {
                    banks[bankObjectIndex].Branches[branchObjectIndex].Charges = new List<TransactionCharges>();
                }

                banks[bankObjectIndex].Branches[branchObjectIndex].Charges.Add(transactionCharges);
                _fileService.WriteFile(banks);

                message.Result = true;
            }
            return message;
        }

        public Message OpenStaffAccount(string staffName, string staffPassword, ushort staffRole)
        {

            bool isStaffAlreadyRegistered = false;
            if (banks[bankObjectIndex].Branches[branchObjectIndex].Staffs == null)
            {
                banks[bankObjectIndex].Branches[branchObjectIndex].Staffs = new List<BranchStaff>();
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

                BranchStaff bankManager = new BranchStaff()
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
    }
}
