using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IHeadManagerService
    {
        /// <summary>
        /// Checks if any Head Managers exist for the given BankId.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <returns>Message indicating the status of Head Managers Existance.</returns>
        Message IsHeadManagersExist(string bankId);

        /// <summary>
        /// Authenticates a Head Manager.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <param name="headManagerPassword">Password of the Head Manager</param>
        /// <returns>Message indicating the Status of Authentication.</returns>
        Message AuthenticateHeadManager(string bankId, string headManagerAccountId, string headManagerPassword);

        /// <summary>
        /// Opens a Head Manager Account.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerName">Name of the Head Manager</param>
        /// <param name="headManagerPassword">Password of the Head Manager</param>
        /// <returns>Message indicating the status of Account Creation.</returns>
        Message OpenHeadManagerAccount(string bankId, string headManagerName, string headManagerPassword);

        /// <summary>
        /// Deletes the Head Manager Account.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <returns>Message indicating the Status of Account Deletion.</returns>
        Message DeleteHeadManagerAccount(string bankId, string headManagerAccountId);

        /// <summary>
        /// Updates the Head Manager Account.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <param name="headManagerName">New name for the Head Manager</param>
        /// <param name="headManagerPassword">New password for the Head Manager</param>
        /// <returns>Message indicating status of Account Updation.</returns>
        Message UpdateHeadManagerAccount(string bankId, string headManagerAccountId, string headManagerName, string headManagerPassword);

        /// <summary>
        /// Checks for Head Manager account Existence.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <returns>Message indicating status of account Existence.</returns>
        Message IsHeadManagerExist(string bankId, string headManagerAccountId);

        /// <summary>
        /// Provides details of the Head Manager Account.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <returns>Details of the Head Manager account.</returns>
        public string GetHeadManagerDetails(string bankId, string headManagerAccountId);

    }
}