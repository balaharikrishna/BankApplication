using BankApplicationModels;

namespace BankApplicationServices.IServices
{
    public interface IBankService
    {
        Message CreateBank(string bankName);
        Message DeleteBank(string bankId);
        Message GetExchangeRates(string bankId);
        Message UpdateBank(string bankId, string bankName);
        Message AuthenticateBankId(string bankId);
    }
}