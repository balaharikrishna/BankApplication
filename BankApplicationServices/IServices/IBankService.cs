using BankApplication.Models;

namespace BankApplication.Services.IServices
{
    public interface IBankService
    {
        /// <summary>
        /// Fetches all branches.
        /// </summary>
        /// <returns>Returns all Banks.</returns>
        Task<IEnumerable<Bank>> GetAllBanksAsync();

        /// <summary>
        /// Fetches Bank Details.
        /// </summary>
        /// <param name="id">Bank Id</param>
        /// <returns>Bank Details by given Id</returns>
        Task<Bank?> GetBankByIdAsync(string id);

        /// <summary>
        /// Fetches Bank Details.
        /// </summary>
        /// <param name="name">Name of the Bank</param>
        /// <returns>Bank Detils by given Name</returns>
        Task<Bank?> GetBankByNameAsync(string name);

        /// <summary>
        /// Creates a new bank.
        /// </summary>
        /// <param name="bankName">Bank name for the new Bank</param>
        /// <returns>Message about the Status of Bank Creation.</returns>
        Task<Message> CreateBankAsync(string bankName);

        /// <summary>
        /// Deletes a Existing bank.
        /// </summary>
        /// <param name="bankId">BankId of the Bank</param>
        /// <returns>Message about the Status of Bank Deletion.</returns>
        Task<Message> DeleteBankAsync(string bankId);

        /// <summary>
        /// Retrieves the exchange rates for a specified bank.
        /// </summary>
        /// <param name="bankId">The unique identifier of the bank for which exchange rates are being retrieved.</param>
        /// <returns>A Message containing information about the status of the operation and the exchange rates retrieved.</returns>
        Task<IEnumerable<Currency>> GetExchangeRatesAsync(string bankId);

        /// <summary>
        /// Updates the name of a bank.
        /// </summary>
        /// <param name="bankId">The unique identifier of the bank being updated.</param>
        /// <param name="bankName">The new name of the bank.</param>
        /// <returns>A Message object containing information about the success or failure of the operation.</returns>
        Task<Message> UpdateBankAsync(string bankId, string bankName);

        /// <summary>
        /// Authenticates a bank's bankId to ensure that it is valid and authorized to perform certain actions within the system.
        /// </summary>
        /// <param name="bankId">The unique identifier of the bank being authenticated.</param>
        /// <returns>A Message containing information about the success or failure of the operation.</returns>
        Task<Message> AuthenticateBankIdAsync(string bankId);
    }
}