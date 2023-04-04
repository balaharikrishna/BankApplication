using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IBranchService
    {
        /// <summary>
        /// Checks whether any branches exist for a given bank.
        /// </summary>
        /// <param name="bankId">The ID of the bank to check.</param>
        /// <returns>A message about Status of the operation whether branches exist or not.</returns>
        Message IsBranchesExist(string bankId);

        /// <summary>
        /// Creates a new branch for a given bank.
        /// </summary>
        /// <param name="bankId">The ID of the bank in which to create the branch.</param>
        /// <param name="branchName">The name of the new branch.</param>
        /// <param name="branchPhoneNumber">The phone number of the new branch.</param>
        /// <param name="branchAddress">The address of the new branch.</param>
        /// <returns>A message about the status of  branch Creation.</returns>
        Message CreateBranch(string bankId, string branchName, string branchPhoneNumber, string branchAddress);

        /// <summary>
        /// Deletes an existing branch for a given bank.
        /// </summary>
        /// <param name="bankId">The ID of the bank in which the branch exists.</param>
        /// <param name="branchId">The ID of the branch to delete.</param>
        /// <returns>A message about the Status of Bank Deletion.</returns>
        Message DeleteBranch(string bankId, string branchId);

        /// <summary>
        /// Gets the transaction charges for a given bank and branch.
        /// </summary>
        /// <param name="bankId">The ID of the bank for which to get transaction charges.</param>
        /// <param name="branchId">The ID of the branch for which to get transaction charges.</param>
        /// <returns>A message about Status of Availbility of transaction charges.</returns>
        Message GetTransactionCharges(string bankId, string branchId);

        /// <summary>
        /// Authenticates the given branch ID for a given bank.
        /// </summary>
        /// <param name="bankId">The ID of the bank in which the branch exists.</param>
        /// <param name="branchId">The ID of the branch to authenticate.</param>
        /// <returns>A message about Status of Branch Authentication.</returns>
        Message AuthenticateBranchId(string bankId, string branchId);

        /// <summary>
        /// Updates an existing branch for a given bank.
        /// </summary>
        /// <param name="bankId">The ID of the bank in which the branch exists.</param>
        /// <param name="branchId">The ID of the branch to update.</param>
        /// <param name="branchName">The new name of the branch.</param>
        /// <param name="branchPhoneNumber">The new phone number of the branch.</param>
        /// <param name="branchAddress">The new address of the branch.</param>
        /// <returns>A message about Status of Branch Updation.</returns>
        Message UpdateBranch(string bankId, string branchId, string branchName, string branchPhoneNumber, string branchAddress);
    }
}