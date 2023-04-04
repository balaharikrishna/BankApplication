using BankApplicationHelperMethods;
using BankApplicationServices.IServices;

namespace BankApplication.IHelperServices
{
    public interface ICommonHelperService
    {
        /// <summary>
        /// Retrieves the account ID.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User</param>
        /// <param name="_validateInputs">The input validator.</param>
        /// <returns>The account ID.</returns>
        string GetAccountId(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the account type.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User</param>
        /// <param name="_validateInputs">The input validator.</param>
        /// <returns>The account type.</returns>
        int GetAccountType(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the address.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User</param>
        /// <param name="_validateInputs">The input validator.</param>
        /// <returns>The address.</returns>
        string GetAddress(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the bank ID, using a bank service and after validating the inputs.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User</param>
        /// <param name="bankService">The bank service to use to retrieve the bank ID</param>
        /// <param name="_validateInputs">The input validator.</param>
        /// <returns>The bank ID.</returns>
        string GetBankId(string position, IBankService bankService, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the branch ID, using a branch service and after validating the inputs.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User</param>
        /// <param name="branchService">The branch service to use to retrieve the branch ID.</param>
        /// <param name="_validateInputs">The input validator.</param>
        /// <returns>The branch ID.</returns>
        string GetBranchId(string position, IBranchService branchService, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the date of birth of a Account Holder.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <param name="_validateInputs">The validation service to be used to validate inputs.</param>
        /// <returns>The date of birth of the Account Holder as a string.</returns>
        string GetDateOfBirth(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the email address of a Account Holder.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <param name="_validateInputs">The validation service to be used to validate inputs.</param>
        /// <returns>The email address of the Account Holder as a string.</returns>
        string GetEmailId(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the gender of a Account Holder. 
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <param name="_validateInputs">The validation service to be used to validate inputs.</param>
        /// <returns>The gender of the Account Holder as an integer.</returns>
        int GetGender(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves the name of a Account Holder.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <param name="_validateInputs">The validation service to be used to validate inputs.</param>
        /// <returns>The name of the Account Holder as a string.</returns>
        string GetName(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Retrieves an option based on selection of Choices.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <returns>The option as an unsigned short integer.</returns>
        ushort GetOption(string position);

        /// <summary>
        /// Retreives an Option based on User Choice Selection.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <param name="_validateInputs">An object used to validate the inputs.</param>
        /// <returns>The generated password as a string.</returns>
        string GetPassword(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Gets a phone number.
        /// </summary>
        /// <param name="position">The position is to specify the Level of User.</param>
        /// <param name="_validateInputs">An object used to validate the inputs.</param>
        /// <returns>The generated phone number as a string.</returns>
        string GetPhoneNumber(string position, IValidateInputs _validateInputs);

        /// <summary>
        /// Validates the amount entered by the user.
        /// </summary>
        /// <returns>The validated amount as a decimal.</returns>
        decimal ValidateAmount();

        /// <summary>
        /// Validates the currency entered by the user based on the bank ID and a currency service.
        /// </summary>
        /// <param name="bankId">The ID of the bank for which the currency is being validated.</param>
        /// <param name="currencyService">The currency service used to validate the currency.</param>
        /// <param name="_validateInputs">An object used to validate the inputs.</param>
        /// <returns>The validated currency as a string.</returns>
        string ValidateCurrency(string bankId, ICurrencyService currencyService, IValidateInputs _validateInputs);

        /// <summary>
        /// Validates the format of the transaction ID entered by the user.
        /// </summary>
        /// <returns>The validated transaction ID format as a string.</returns>
        string ValidateTransactionIdFormat();

        /// <summary>
        /// Validates the transfer method entered by the user.
        /// </summary>
        /// <returns>The validated transfer method as an integer.</returns>
        int ValidateTransferMethod();
    }
}