using BankApplicationServices.IServices;

namespace BankApplication.IHelperServices
{
    public interface ICommonHelperService
    {
        string GetAccountId(string position);
        int GetAccountType(string position);
        string GetAddress(string position);
        string GetBankId(string position, IBankService bankService);
        string GetBranchId(string position, IBranchService branchService);
        string GetDateOfBirth(string position);
        string GetEmailId(string position);
        int GetGender(string position);
        string GetName(string position);
        ushort GetOption(string position);
        string GetPassword(string position);
        string GetPhoneNumber(string position);
        decimal ValidateAmount();
        string ValidateCurrency(string bankId, ICurrencyService currencyService);
        string ValidateTransactionIdFormat();
        int ValidateTransferMethod();
    }
}