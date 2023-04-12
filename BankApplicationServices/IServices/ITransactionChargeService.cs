using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface ITransactionChargeService
    {
        /// <summary>
        /// Adds transaction charges for the specified bank and branch.
        /// </summary>
        /// <param name="bankId">The BankId of the bank.</param>
        /// <param name="branchId">The BranchId of the branch.</param>
        /// <param name="rtgsSameBank">The RTGS transaction charge for transactions within the same bank.</param>
        /// <param name="rtgsOtherBank">The RTGS transaction charge for transactions to other banks.</param>
        /// <param name="impsSameBank">The IMPS transaction charge for transactions within the same bank.</param>
        /// <param name="impsOtherBank">The IMPS transaction charge for transactions to other banks.</param>
        /// <returns>Message about the status of the transaction charges addition.</returns>
        Task<Message> AddTransactionChargesAsync(string bankId, string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank);

        /// <summary>
        /// Deletes transaction charges for the specified bank and branch.
        /// </summary>
        /// <param name="bankId">The BankId of the bank.</param>
        /// <param name="branchId">The BranchId of the branch.</param>
        /// <returns>Message about the status of the transaction charges deletion.</returns>
        Task<Message> DeleteTransactionChargesAsync(string bankId, string branchId);

        /// <summary>
        /// Updates transaction charges for the specified bank and branch.
        /// </summary>
        /// <param name="bankId">The BankId of the bank.</param>
        /// <param name="branchId">The BranchId of the branch.</param>
        /// <param name="rtgsSameBank">The updated RTGS transaction charge for transactions within the same bank.</param>
        /// <param name="rtgsOtherBank">The updated RTGS transaction charge for transactions to other banks.</param>
        /// <param name="impsSameBank">The updated IMPS transaction charge for transactions within the same bank.</param>
        /// <param name="impsOtherBank">The updated IMPS transaction charge for transactions to other banks.</param>
        /// <returns>Message about the status of the transaction charges updation.</returns>
        Task<Message> UpdateTransactionChargesAsync(string bankId, string branchId, ushort rtgsSameBank, ushort rtgsOtherBank, ushort impsSameBank, ushort impsOtherBank);
    }
}