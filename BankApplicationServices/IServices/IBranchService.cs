using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IBranchService
    {
        /// <summary>
        /// Checks whether any branches exist for a given bank.
        /// </summary>
        /// <param name="bankId">The ID of the bank to check.</param>
        /// <returns>A message about status of the operation whether branches exist or not.</returns>
        Message IsBranchesExist(string bankId);
        /// <summary>
        /// Creates a new branch for a given bank.
        /// </summary>
        /// <param name="bankId">The ID of the bank in which to create the branch.</param>
        /// <param name="branchName">The name of the new branch.</param>
        /// <param name="branchPhoneNumber">The phone number of the new branch.</param>
        /// <param name="branchAddress">The address of the new branch.</param>
        /// <returns>A message Object about status of  branch Creation.</returns>
        Message CreateBranch(string bankId, string branchName, string branchPhoneNumber, string branchAddress);
        Message DeleteBranch(string bankId, string branchId);
        Message GetTransactionCharges(string bankId, string branchId);
        Message AuthenticateBranchId(string bankId, string branchId);
        Message UpdateBranch(string bankId, string branchId, string branchName, string branchPhoneNumber, string branchAddress);
    }
}