using BankApplication.Models;

namespace BankApplication.Services.IServices
{
    public interface ITokenIssueService
    {
        /// <summary>
        /// authenticates the given userName and password.
        /// </summary>
        /// <param name="accountId">Account Id of User.</param>
        /// <param name="password">password of User.</param>
        /// <returns>A Message object containing information about the success or failure of the operation.</returns>
        Task<Message> IssueToken(string accountId, string password);
    }
}
