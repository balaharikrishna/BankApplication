using BankApplicationModels;

namespace BankApplicationHelperMethods
{
    public interface IValidateInputs
    {
        Message ValidateAccountIdFormat(string accountId);
        Message ValidateAccountTypeFormat(int accountType);
        Message ValidateAddressFormat(string address);
        Message ValidateBankIdFormat(string bankId);
        Message ValidateBranchIdFormat(string branchId);
        Message ValidateCurrencyCodeFormat(string currencyCode);
        Message ValidateDateOfBirthFormat(string dateOfBirth);
        Message ValidateEmailIdFormat(string emailId);
        Message ValidateGenderFormat(int gender);
        Message ValidateNameFormat(string name);
        Message ValidatePasswordFormat(string password);
        Message ValidatePhoneNumberFormat(string phoneNumber);
    }
}