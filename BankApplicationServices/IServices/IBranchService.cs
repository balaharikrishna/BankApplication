using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IBranchService
    {
        Message CreateBranch(string bankId, string branchName, string branchPhoneNumber, string branchAddress);
        Message DeleteBranch(string bankId, string branchId);
        Message GetTransactionCharges(string bankId, string branchId);

        Message AuthenticateBranchId(string bankId, string branchId);
        Message UpdateBranch(string bankId, string branchId, string branchName, string branchPhoneNumber, string branchAddress);
    }
}