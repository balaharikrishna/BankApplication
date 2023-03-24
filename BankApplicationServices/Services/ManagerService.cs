
using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class ManagerService : IManagerService
    {
        private static int bankObjectIndex = 0;
        private static int branchObjectIndex = 0;

        IFileService _fileService;
        IBranchStaffService _branchStaffService;
        List<Bank> banks;
        public ManagerService(IFileService fileService, IBranchStaffService branchStaffService)
        {
            _fileService = fileService;
            _branchStaffService = branchStaffService;
            this.banks = _fileService.GetData();
        }
        Message message = new Message();
        public Message AuthenticateManagerAccount(string bankId, string branchId,
            string branchManagerAccountId, string branchManagerPassword)
        {
            if (banks.Count > 0)
            {
                var bank = banks.FirstOrDefault(b => b.BankId == bankId);
                if (bank != null)
                {
                    var branch = bank.Branches.FirstOrDefault(br => br.branchId == branchId);
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
                    branchObjectIndex = banks[bankObjectIndex].Branches.FindIndex(branch => branch.branchId == branchId);
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Entered bankId '{bankId}' is not found. Please Check Again.";
                }
            }
            return message;
        }

        public Message OpenManagerAccount(string bankId, string branchId, string branchManagerName, string branchManagerPassword)
        {
            bool isBranchManagerAlreadyRegistered = false;
            int branchObjectIndex;

            branchObjectIndex = banks[bankObjectIndex].Branches.FindIndex(branch => branch.branchId == branchId);
            if (banks[bankObjectIndex].Branches[branchObjectIndex].Managers == null)
            {
                banks[bankObjectIndex].Branches[branchObjectIndex].Managers = new List<Manager>();
            }

            isBranchManagerAlreadyRegistered = banks[bankObjectIndex].Branches[branchObjectIndex].Managers.Any(ma => ma.BranchManagerName == branchManagerName);
            if (isBranchManagerAlreadyRegistered)
            {
                message.Result = false;
                message.ResultMessage = $"Branch Manager {branchManagerName} is already Registered";
            }
            else
            {
                DateTime currentDate = DateTime.Now;
                string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                string UserFirstThreeCharecters = branchManagerName.Substring(0, 3);
                string branchManagerAccountId = UserFirstThreeCharecters + date;

                Manager branchManager = new Manager()
                {
                    BranchManagerAccountId = branchManagerAccountId,
                    BranchManagerName = branchManagerName,
                    BranchMangerPassword = branchManagerPassword
                };


                banks[bankObjectIndex].Branches[branchObjectIndex].Managers.Add(branchManager);
                _fileService.WriteFile(banks);
                message.Result = true;
                message.ResultMessage = $"Branch Manager Account Opened with AccountId:{branchManagerAccountId}";
            }
            return message;
        }

        public Message UpdateManagerAccount(string bankId, string branchId, string branchManagerName, string branchManagerPassword)
        {

        }

        public Message DeleteManagerAccount(string bankId, string branchId)
        {

        }
    }
}
