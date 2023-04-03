using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IBankService
    {
        /// <summary>
        /// Creates a new bank
        /// </summary>
        /// <param name="bankName">Bank name for the new bank</param>
        /// <returns>Message about the status of the operation</returns>
        Message CreateBank(string bankName);
        Message DeleteBank(string bankId);
        Message GetExchangeRates(string bankId);
        Message UpdateBank(string bankId, string bankName);
        Message AuthenticateBankId(string bankId);
    }
}