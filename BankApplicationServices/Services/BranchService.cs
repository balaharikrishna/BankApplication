using BankApplication.Models;
using BankApplication.Repository.IRepository;
using BankApplication.Services.IServices;

namespace BankApplication.Services.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        public BranchService(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<IEnumerable<Branch>> GetAllBranchesAsync(string bankId)
        {
            IEnumerable<Branch>? branches = await _branchRepository.GetAllBranches(bankId);
            if (branches.Any())
            {
                return branches;
            }
            else
            {
                return Enumerable.Empty<Branch>();
            }
        }

        public async Task<Branch?> GetBranchByIdAsync(string id)
        {
            Branch? branch = await _branchRepository.GetBranchById(id);
            if (branch is not null)
            {
                return branch;
            }
            else
            {
                return null;
            }
        }

        public async Task<Branch?> GetBranchByNameAsync(string name)
        {
            Branch? branch = await _branchRepository.GetBranchByName(name);
            if (branch is not null)
            {
                return branch;
            }
            else
            {
                return null;
            }
        }

        public async Task<Message> IsBranchesExistAsync(string bankId)
        {
            IEnumerable<Branch>? branches = await _branchRepository.GetAllBranches(bankId);
            Message message = new();
            if (branches.Any())
            {
                message.Result = true;
                message.ResultMessage = "Branches Exist";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"No Branches Available for BankId:{bankId}";
            }
            return message;
        }

        public async Task<Message> AuthenticateBranchIdAsync(string branchId)
        {
            bool isBranchExist = await _branchRepository.IsBranchExist(branchId);

            Message message = new();
            if (isBranchExist)
            {
                message.Result = true;
                message.ResultMessage = $"Branch Id:'{branchId}' Exist.";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Bank Id:'{branchId}' Doesn't Exist.";
            }
            return message;
        }

        public async Task<Message> CreateBranchAsync(string bankId, string branchName, string branchPhoneNumber, string branchAddress)
        {
            Message message = new();

            Branch? _branchName = await _branchRepository.GetBranchByName(branchName);
            if (_branchName is not null)
            {
                message.Result = false;
                message.ResultMessage = $"BranchName:{branchName} is Already Registered.";
            }
            else
            {
                string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                string bankFirstThreeCharecters = branchName.Substring(0, 3);
                string branchId = bankFirstThreeCharecters + date + "M";

                Branch branch = new()
                {
                    BranchId = branchId,
                    BranchName = branchName,
                    BranchPhoneNumber = branchPhoneNumber,
                    BranchAddress = branchAddress,
                    IsActive = true
                };

                bool isBranchAdded = await _branchRepository.AddBranch(branch, bankId);
                if (isBranchAdded)
                {
                    message.Result = true;
                    message.ResultMessage = $"Branch {branchName} is Created with {branchId}";
                    message.Data = branchId;
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Branch {branchName} is Created with {branchId}";
                }
            }
            return message;
        }

        public async Task<Message> UpdateBranchAsync(string branchId, string branchName, string branchPhoneNumber, string branchAddress)
        {
            Message message = new();

            Message messageResult = await AuthenticateBranchIdAsync(branchId);
            if (messageResult.Result)
            {
                Branch branch = new()
                {
                    BranchName = branchName,
                    BranchId = branchId,
                    BranchPhoneNumber = branchPhoneNumber,
                    BranchAddress = branchAddress,
                    IsActive = true
                };
                bool isBranchUpdated = await _branchRepository.UpdateBranch(branch);
                if (isBranchUpdated)
                {
                    message.Result = true;
                    message.ResultMessage = $"Updated BranchId:{branchId} with Branch Name:{branch.BranchName},Branch Phone Number:{branch.BranchPhoneNumber},Branch Address:{branch.BranchAddress}";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = "Failed to Update the Branch";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Branch Authentication Failed";
            }
            return message;
        }
        public async Task<Message> DeleteBranchAsync(string branchId)
        {
            Message message = new();
            Message messageResult = await AuthenticateBranchIdAsync(branchId);
            if (messageResult.Result)
            {
                bool isDeleted = await _branchRepository.DeleteBranch(branchId);
                if (isDeleted)
                {
                    message.Result = true;
                    message.ResultMessage = $"Deleted BranchId:{branchId} Successfully";
                }
                else
                {
                    message.Result = false;
                    message.ResultMessage = $"Failed to Delete BranchId:{branchId}";
                }
            }
            else
            {
                message.Result = false;
                message.ResultMessage = "Branch Authentication Failed";
            }
            return message;
        }

        public async Task<IEnumerable<TransactionCharge>> GetTransactionChargesAsync(string branchId)
        {
            IEnumerable<TransactionCharge> charges = await _branchRepository.GetAllTransactionCharges(branchId);
            if (charges.Any())
            {
                return charges;
            }
            else
            {
                return Enumerable.Empty<TransactionCharge>();
            }
        }
    }
}
