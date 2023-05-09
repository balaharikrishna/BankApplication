using BankApplication.Models;

namespace BankApplication.Services.IServices
{
    public interface IHeadManagerService
    {
        /// <summary>
        /// Gets All Available Head Managers in a Branch by fetching with branch Id.
        /// </summary>
        /// <param name="branchId">The ID of the Branch.</param>
        /// <returns>All Head Managers Available in a Branch.</returns>
        Task<IEnumerable<HeadManager>> GetAllHeadManagersAsync(string branchId);

        /// <summary>
        /// Retrieves the Customer Details using Bank Id and Head Manager Id.
        /// </summary>
        /// <param name="bankId">The unique identifier of the Bank where the Head manager account is located.</param>
        /// <param name="headManagerAccountId">The unique identifier of the Head manager Account.</param>
        /// <returns>Head Manager Details.</returns>
        Task<HeadManager?> GetHeadManagerByIdAsync(string bankId, string headManagerAccountId);

        /// <summary>
        /// Retrieves the Customer Details using Bank Id and Head Manager Name.
        /// </summary>
        /// <param name="bankId">The unique identifier of the Bank where the Head manager account is located.</param>
        /// <param name="headManagerName">The Name of the Head manager.</param>
        /// <returns>Head Manager Details.</returns>
        Task<HeadManager?> GetHeadManagerByNameAsync(string bankId, string headManagerName);

        /// <summary>
        /// Checks if any Head Managers exist for the given BankId.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <returns>Message indicating the status of Head Managers Existance.</returns>
        Task<Message> IsHeadManagersExistAsync(string bankId);

        /// <summary>
        /// Authenticates a Head Manager.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <param name="headManagerPassword">Password of the Head Manager</param>
        /// <returns>Message indicating the Status of Authentication.</returns>
        Task<Message> AuthenticateHeadManagerAsync(string bankId, string headManagerAccountId, string headManagerPassword);

        /// <summary>
        /// Opens a Head Manager Account.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerName">Name of the Head Manager</param>
        /// <param name="headManagerPassword">Password of the Head Manager</param>
        /// <returns>Message indicating the status of Account Creation.</returns>
        Task<Message> OpenHeadManagerAccountAsync(string bankId, string headManagerName, string headManagerPassword);

        /// <summary>
        /// Deletes the Head Manager Account.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <returns>Message indicating the Status of Account Deletion.</returns>
        Task<Message> DeleteHeadManagerAccountAsync(string bankId, string headManagerAccountId);

        /// <summary>
        /// Updates the Head Manager Account.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <param name="headManagerName">New name for the Head Manager</param>
        /// <param name="headManagerPassword">New password for the Head Manager</param>
        /// <returns>Message indicating status of Account Updation.</returns>
        Task<Message> UpdateHeadManagerAccountAsync(string bankId, string headManagerAccountId, string headManagerName, string headManagerPassword);

        /// <summary>
        /// Checks for Head Manager account Existence.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <param name="headManagerAccountId">Account Id of the Head Manager</param>
        /// <returns>Message indicating status of account Existence.</returns>
        Task<Message> IsHeadManagerExistAsync(string bankId, string headManagerAccountId);
    }
}