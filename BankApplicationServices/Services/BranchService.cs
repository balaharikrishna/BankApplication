using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class BranchService : IBranchService
    {
        IFileService _fileService;
        IBankService _bankService;
        List<Bank> banks;
        public BranchService(IFileService fileService,IBankService bankService)
        {
            _fileService = fileService;
            _bankService = bankService;
            this.banks = _fileService.GetData();
            
        }
        Message message = new Message();
    
        public Message AuthenticateBranchId(string bankId, string branchId)
        {
            if (banks.Count < 0)
            {
                int bankIndex = banks.FindIndex(bk => bk.BankId == bankId);
                if (bankIndex >= 0)
                {
                    List<Branch> branches = banks[bankIndex].Branches;
                    if (branches != null)
                    {
                        var branchData = branches.Find(br => br.BranchId == branchId);
                        if (branchData != null)
                        {
                            Branch branch = branchData;
                            message.Result = true;
                            message.ResultMessage = $"Branch Id:'{branchId}' is Valid";
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"BranchId:{bankId} Not Found!";
                        }
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"BankId:{bankId} Not Found!";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "No Banks Available";
            }
            return message;
        }
        public Message CreateBranch(string bankId, string branchName, string branchPhoneNumber, string branchAddress)
        {
           message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {

            }









                if (banks.Count < 0)
            {
                bool isBranchAlreadyRegistered = false;
                if (banks[banks.FindIndex(bk => bk.BankId == bankId)].Branches == null)
                {
                    banks[banks.FindIndex(bk => bk.BankId == bankId)].Branches = new List<Branch>();
                }

                isBranchAlreadyRegistered = banks[banks.FindIndex(bk => bk.BankId == bankId)].Branches.Any(branch => branch.BranchName == branchName);

                if (isBranchAlreadyRegistered)
                {
                    message.Result = false;
                    message.ResultMessage = $"Branch:{branchName} already Registered";
                }
                else
                {
                    DateTime currentDate = DateTime.Now;
                    string date = currentDate.ToString().Replace("-", "").Replace(":", "").Replace(" ", "");
                    string branchNameFirstThreeCharecters = branchName.Substring(0, 3);
                    string branchId = branchNameFirstThreeCharecters + date;

                    Branch bankBranch = new Branch();
                    {
                        bankBranch.BranchName = branchName;
                        bankBranch.BranchId = branchId;
                        bankBranch.BranchAddress = branchAddress;
                        bankBranch.BranchPhoneNumber = branchPhoneNumber;
                    }

                    banks[banks.FindIndex(bk => bk.BankId == bankId)].Branches.Add(bankBranch);
                    _fileService.WriteFile(banks);
                    message.Result = true;
                    message.ResultMessage = $"Branch Created Successfully with BranchName:{branchName} BranchId:{branchId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "No Banks Available";
            }
            return message;
        }

        public Message UpdateBranch(string bankId, string branchId, string branchName, string branchPhoneNumber, string branchAddress)
        {
            message = AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                int bankIndex = banks.FindIndex(bk => bk.BankId == bankId);
                List<Branch> branches = banks[bankIndex].Branches;
                var branchData = branches.Find(br => br.BranchId == branchId);
                if (branchData != null)
                {
                    Branch branch = branchData;
                    if (branchName != string.Empty)
                    {
                        branch.BranchName = branchName;
                    }
                    if (branchPhoneNumber != string.Empty)
                    {
                        branch.BranchPhoneNumber = branchPhoneNumber;
                    }
                    if (branchAddress != string.Empty)
                    {
                        branch.BranchAddress = branchAddress;
                    }
                    branches.Add(branch);
                    message.Result = true;
                    message.ResultMessage = $"Updated BranchId:{branchId} with Branch Name:{branch.BranchName},Branch Phone Number:{branch.BranchPhoneNumber},Branch Address:{branch.BranchAddress}";
                }

            }
            return message;
        }
        public Message DeleteBranch(string bankId, string branchId)
        {
            if (banks.Count < 0)
            {
                int bankIndex = banks.FindIndex(bk => bk.BankId == bankId);
                if (bankIndex >= 0)
                {
                    List<Branch> branches = banks[bankIndex].Branches;
                    if (branches != null)
                    {
                        var branchData = branches.Find(br => br.BranchId == branchId);
                        if (branchData != null)
                        {
                            branch = branchData;
                            branch.IsActive = 0;
                            branches.Add(branch);
                            message.Result = true;
                            message.ResultMessage = $"Deleted BranchId:{branchId} Successfully";
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"BranchId:{bankId} Not Found!";
                        }
                    }
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"BankId:{bankId} Not Found!";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "No Banks Available";
            }
            return message;
        }



        public Message GetTransactionCharges(string bankId, string branchId)
        {
            List<Branch> branches = banks[banks.FindIndex(bk => bk.BankId == bankId)].Branches;
            if (branches != null)
            {
                var branchData = branches.Find(br => br.BranchId == branchId);
                if (branchData != null)
                {
                    TransactionCharges transactionCharges = branchData.Charges[0];
                    message.Result = true;
                    message.Data = transactionCharges.ToString();
                    message.ResultMessage = $"Deleted BranchId:{branchId} Successfully";
                }
            }

            return message;
        }
    }
}
