using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplicationServices.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBankService _bankService;
        public BranchService(IBankService bankService)
        {
            _bankService = bankService;
        }

        public Task<Message> IsBranchesExistAsync(string bankId)
        {
            
            Message message = new();
            banks = _fileService.GetData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                int bankIndex = banks.FindIndex(bk => bk.BankId.Equals(bankId));
                if (bankIndex >= 0)
                {
                    List<Branch> branches = banks[bankIndex].Branches;
                    branches ??= new List<Branch>();
                    branches.FindAll(br => br.IsActive == 1);

                    if (branches is not null && branches.Count >= 1)
                    {
                        message.Result = true;
                        message.ResultMessage = "Branches Exist";
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = $"No Branches Available for {bankId}";
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
                message.ResultMessage = "Bank Authentication Failed";
            }

            return message;
        }

        public Task<Message> AuthenticateBranchIdAsync(string bankId, string branchId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                int bankIndex = banks.FindIndex(bk => bk.BankId.Equals(bankId));
                if (bankIndex >= 0)
                {
                    List<Branch> branches = banks[bankIndex].Branches;
                    if (branches is not null)
                    {

                        var branchData = branches.Find(br => br.BranchId.Equals(branchId) && br.IsActive == 1);
                        if (branchData is not null)
                        {
                            Branch branch = branchData;
                            message.Result = true;
                            message.ResultMessage = $"Branch Id:'{branchId}' is Exist";
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = $"BranchId:{branchId} Not Found!";
                        }
                    }
                    else
                    {
                        message.Result = false;
                        message.ResultMessage = "No Branches Available";
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
                message.ResultMessage = "Bank Authentication Failed";
            }
            return message;
        }

        public Task<Message> CreateBranchAsync(string bankId, string branchName, string branchPhoneNumber, string branchAddress)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = _bankService.AuthenticateBankId(bankId);
            if (message.Result)
            {
                bool isBranchAlreadyRegistered = false;
                List<Branch> branches = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Branches;
                branches ??= new List<Branch>();

                isBranchAlreadyRegistered = branches.Any(branch => branch.BranchName.Equals(branchName));

                if (isBranchAlreadyRegistered)
                {
                    message.Result = false;
                    message.ResultMessage = $"Branch:{branchName} already Registered";
                }
                else
                {
                    string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string branchNameFirstThreeCharecters = branchName.Substring(0, 3);
                    string branchId = branchNameFirstThreeCharecters + date;

                    Branch branch = new();
                    {
                        branch.BranchName = branchName;
                        branch.BranchId = branchId;
                        branch.BranchAddress = branchAddress;
                        branch.BranchPhoneNumber = branchPhoneNumber;
                        branch.IsActive = 1;
                    }
                    branches.Add(branch);
                    banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Branches = branches;
                    _fileService.WriteFile(banks);
                    message.Result = true;
                    message.ResultMessage = $"Branch Created Successfully with BranchName:{branchName} & BranchId:{branchId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Bank Authentication Failed";
            }
            return message;
        }

        public Task<Message> UpdateBranchAsync(string bankId, string branchId, string branchName, string branchPhoneNumber, string branchAddress)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                int bankIndex = banks.FindIndex(bk => bk.BankId.Equals(bankId));
                List<Branch> branches = banks[bankIndex].Branches;
                var branchData = branches.Find(br => br.BranchId.Equals(branchId));
                if (branchData is not null)
                {
                    Branch branch = branchData;

                    if (!string.IsNullOrEmpty(branchName))
                    {
                        branch.BranchName = branchName;
                    }

                    if (!string.IsNullOrEmpty(branchPhoneNumber))
                    {
                        branch.BranchPhoneNumber = branchPhoneNumber;
                    }

                    if (!string.IsNullOrEmpty(branchAddress))
                    {
                        branch.BranchAddress = branchAddress;
                    }
                    _fileService.WriteFile(banks);
                    message.Result = true;
                    message.ResultMessage = $"Updated BranchId:{branchId} with Branch Name:{branch.BranchName},Branch Phone Number:{branch.BranchPhoneNumber},Branch Address:{branch.BranchAddress}";
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
                message.ResultMessage = "Branch Authentication Failed";
            }
            return message;
        }
        public Task<Message> DeleteBranchAsync(string bankId, string branchId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                int bankIndex = banks.FindIndex(bk => bk.BankId.Equals(bankId));
                List<Branch> branches = banks[bankIndex].Branches;
                var branchData = branches.Find(br => br.BranchId.Equals(branchId));
                if (branchData is not null)
                {
                    branchData.IsActive = 0;
                    _fileService.WriteFile(banks);
                    message.Result = true;
                    message.ResultMessage = $"Deleted BranchId:{branchId} Successfully";
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
                message.ResultMessage = "Branch Authentication Failed";
            }
            return message;
        }

        public Task<Message> GetTransactionChargesAsync(string bankId, string branchId)
        {
            Message message = new();
            banks = _fileService.GetData();
            message = AuthenticateBranchId(bankId, branchId);
            if (message.Result)
            {
                List<Branch> branches = banks[banks.FindIndex(bk => bk.BankId.Equals(bankId))].Branches;
                if (branches is not null)
                {
                    var branchData = branches.Find(br => br.BranchId.Equals(branchId));
                    if (branchData is not null)
                    {
                        List<TransactionCharges> transactionCharges = branchData.Charges;
                        if (transactionCharges is not null)
                        {
                            var charge = transactionCharges.Find(c => c.IsActive == 1);
                            if (charge is not null)
                            {
                                message.Result = true;
                                message.Data = charge.ToString();
                                message.ResultMessage = $"Transaction Charges Available";
                            }
                            else
                            {
                                message.Result = false;
                                message.ResultMessage = $"Transaction Charges Not Available";
                            }
                        }
                        else
                        {
                            message.Result = false;
                            message.ResultMessage = "TransactionCharges Not Found";
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
                    message.ResultMessage = "No Branches Available.";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Branch Authentication Failed";
            }
            return message;
        }
    }
}
