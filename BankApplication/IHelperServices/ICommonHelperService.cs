using BankApplicationHelperMethods;
using BankApplicationServices.IServices;

namespace BankApplication.IHelperServices
{
    public interface ICommonHelperService
    {
        string GetAccountId(string position, IValidateInputs _validateInputs);
        int GetAccountType(string position, IValidateInputs _validateInputs);
        string GetAddress(string position, IValidateInputs _validateInputs);
        string GetBankId(string position, IBankService bankService, IValidateInputs _validateInputs);
        string GetBranchId(string position, IBranchService branchService, IValidateInputs _validateInputs);
        string GetDateOfBirth(string position, IValidateInputs _validateInputs);
        string GetEmailId(string position, IValidateInputs _validateInputs);
        int GetGender(string position, IValidateInputs _validateInputs);
        string GetName(string position, IValidateInputs _validateInputs);
        ushort GetOption(string position);
        string GetPassword(string position, IValidateInputs _validateInputs);
        string GetPhoneNumber(string position, IValidateInputs _validateInputs);
        decimal ValidateAmount();
        string ValidateCurrency(string bankId, ICurrencyService currencyService, IValidateInputs _validateInputs);
        string ValidateTransactionIdFormat();
        int ValidateTransferMethod();
    }
}