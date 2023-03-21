using BankApplicationModels;
using BankApplicationServices.Interfaces;

namespace BankApplicationServices.Services
{
    public class BranchService : IBranchService
    {
        IFileService _fileService;
        List<Bank> banks;
        public BranchService(IFileService fileService)
        {
            _fileService = fileService;
            this.banks = _fileService.GetData();
        }

        public string GetTransactionCharges(string bankId, string branchId)
        {

            TransactionCharges transactionCharges = banks.Find(bank => bank.BankId == bankId).Branches.Find(branch => branch.BranchId == branchId).Charges[0];

            return transactionCharges.ToString();
        }
    }
}
