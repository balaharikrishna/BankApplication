using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IManagerService
    {
        /// <summary>
        /// Checks for managers Existence.
        /// </summary>
        /// <param name="bankId">The ID of the bank.</param>
        /// <param name="branchId">The ID of the branch.</param>
        /// <returns>A message indicating Status of managers Existence.</returns>
        Message IsManagersExist(string bankId, string branchId);

        /// <summary>
        /// Authenticates the Manager Account.
        /// </summary>
        /// <param name="bankId">The ID of the bank.</param>
        /// <param name="branchId">The ID of the branch.</param>
        /// <param name="branchManagerAccountId">The account ID of the branch manager.</param>
        /// <param name="branchManagerPassword">The password of the branch manager.</param>
        /// <returns>A message indicating status of Authentication.</returns>
        Message AuthenticateManagerAccount(string bankId, string branchId, string branchManagerAccountId, string branchManagerPassword);

        /// <summary>
        /// Deletes Manager Account.
        /// </summary>
        /// <param name="bankId">The ID of the bank.</param>
        /// <param name="branchId">The ID of the branch.</param>
        /// <param name="accountId">The account ID of the branch manager to delete.</param>
        /// <returns>A message indicating status of Account Deletion.</returns>
        Message DeleteManagerAccount(string bankId, string branchId, string accountId);

        /// <summary>
        /// Opens a new Account for Manager.
        /// </summary>
        /// <param name="bankId">The ID of the bank.</param>
        /// <param name="branchId">The ID of the branch.</param>
        /// <param name="branchManagerName">The name of the branch manager.</param>
        /// <param name="branchManagerPassword">The password for the branch manager's account.</param>
        /// <returns>A message indicating status of Account Opening.</returns>
        Message OpenManagerAccount(string bankId, string branchId, string branchManagerName, string branchManagerPassword);

        /// <summary>
        /// Updates manager's Account.
        /// </summary>
        /// <param name="bankId">The ID of the bank.</param>
        /// <param name="branchId">The ID of the branch.</param>
        /// <param name="accountId">The account ID of the branch manager to update.</param>
        /// <param name="branchManagerName">The new name for the branch manager.</param>
        /// <param name="branchManagerPassword">The new password for the branch manager's account.</param>
        /// <returns>A message indicating status of Account Updation.</returns>
        Message UpdateManagerAccount(string bankId, string branchId, string accountId, string branchManagerName, string branchManagerPassword);

        /// <summary>
        /// Checks for Account Existence.
        /// </summary>
        /// <param name="bankId">The ID of the bank.</param>
        /// <param name="branchId">The ID of the branch.</param>
        /// <param name="managerAccountId">The account ID of the branch manager to check for.</param>
        /// <returns>A message indicating status of Account Existence.</returns>
        Message IsAccountExist(string bankId, string branchId, string managerAccountId);

        /// <summary>
        /// Retrieves the details of a branch manager's Account.
        /// </summary>
        /// <param name="bankId">The ID of the bank.</param>
        /// <param name="branchId">The ID of the branch.</param>
        /// <param name="managerAccountId">The account ID of the branch manager.</param>
        /// <returns>A string containing the details of the branch manager's account.</returns>
        public string GetManagerDetails(string bankId, string branchId, string managerAccountId);
    }
}