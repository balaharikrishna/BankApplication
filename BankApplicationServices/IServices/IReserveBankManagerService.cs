using BankApplication.Models;

namespace BankApplication.Services.IServices
{
    public interface IReserveBankManagerService
    {
        /// <summary>
        /// Retrieves the details of All Reserve Bank Manager's Accounts.
        /// </summary>
        /// <returns>All branch Reserve Bank Manager's Account Details.</returns>
        Task<IEnumerable<ReserveBankManager>> GetAllReserveBankManagersAsync();

        /// <summary>
        /// Retrieves Reserve Bank manager's Account Details.
        /// </summary>
        /// <param name="reserveBankManagerAccountId">The account ID of Reserve Bank Manager.</param>
        /// <returns>Details of branch manager's account.</returns>
        Task<ReserveBankManager?> GetReserveBankManagerByIdAsync(string reserveBankManagerAccountId);

        /// <summary>
        /// Retrieves the details of a Reserve Bank manager Account.
        /// </summary>
        /// <param name="reserveBankManagerName">The Name of the Reserve Bank manager.</param>
        /// <returns>A string containing the details of the branch manager's account.</returns>
        Task<ReserveBankManager?> GetReserveBankManagerByNameAsync(string reserveBankManagerName);

        /// <summary>
        /// Authenticates the Reserve Bank manager Account.
        /// </summary>
        /// <param name="ReserveBankManagerAccountId">The account ID of the Reserve Bank manager.</param>
        /// <param name="ReserveBankManagerPassword">The password of the Reserve Bank manager.</param>
        /// <returns>A message indicating status of Authentication.</returns>
        Task<Message> AuthenticateManagerAccountAsync(string ReserveBankManagerAccountId, string ReserveBankManagerPassword);

        /// <summary>
        /// Checks for managers Existence.
        /// </summary>
        /// <returns>A message indicating Status of Reserve Bank manager Existence.</returns>
        Task<Message> IsReserveBankManagersExistAsync();

        /// <summary>
        /// Opens a new Account for Reserve Bank manager.
        /// </summary>
        /// <param name="ReserveBankManagerName">The name of the Reserve Bank manager.</param>
        /// <param name="ReserveBankManagerPassword">The password for the Reserve Bank manager account.</param>
        /// <returns>A message indicating status of Account Opening.</returns>
        Task<Message> OpenReserveBankManagerAccountAsync(string ReserveBankManagerName, string ReserveBankManagerPassword);

        /// <summary>
        /// Updates Reserve Bank manager Account.
        /// </summary>
        /// <param name="ReserveBankManagerAccountId">The account ID of the Reserve Bank manager to update.</param>
        /// <param name="ReserveBankManagerName">The new name for the Reserve Bank manager.</param>
        /// <param name="ReserveBankManagerPassword">The new password for the Reserve Bank manager account.</param>
        /// <returns>A message indicating status of Account Updation.</returns>
        Task<Message> UpdateReserveBankManagerAccountAsync(string ReserveBankManagerAccountId, string ReserveBankManagerName, string ReserveBankManagerPassword);

        /// <summary>
        /// Deletes Reserve Bank manager Account.
        /// </summary>
        /// <param name="ReserveBankManagerAccountId">The account ID of the Reserve Bank manager to delete.</param>
        /// <returns>A message indicating status of Account Deletion.</returns>
        Task<Message> DeleteReserveBankManagerAccountAsync(string ReserveBankManagerAccountId);

        /// <summary>
        /// Checks for Account Existence.
        /// </summary>
        /// <param name="ReserveBankManagerAccountId">The account ID of the Reserve Bank manager.</param>
        /// <returns>A message indicating status of Account Existence.</returns>
        Task<Message> IsAccountExistAsync(string ReserveBankManagerAccountId);
    }
}