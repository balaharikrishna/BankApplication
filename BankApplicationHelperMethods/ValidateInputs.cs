using System.Text.Json;
using BankApplicationModels;
using BankApplicationServices;
using BankApplicationServices.Interfaces;

namespace BankApplicationHelperMethods
{
    public class ValidateInputs : IFileService
    {
        IFileService _fileService;
        List<Bank> banks;
        public ValidateInputs(IFileService fileService)
        {
            _fileService = fileService;
            this.banks = _fileService.GetData();
        }
        static Message message = new Message();

        public static Message ValidateBankId(string bankId)
        {
            bool bank = banks.Any(b => b.BankId == bankId);
            if (bank)
            {
                message.Result = true;
                message.ResultMessage = $"Valid Bank Id:'{bankId}'";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Invalid Bank Id:'{bankId}'";
            }
            return message;
        }

        public static Message ValidateBranchId(string bankId, string branchId)
        {
            var bank = banks.FirstOrDefault(b => b.BankId == bankId);
            bool isValidBranch = false;
            if (bank != null)
            {
                isValidBranch = bank.Branches.Any(br => br.BranchId == branchId);
                message.Result = true;
                message.ResultMessage = $"Branch Id:'{branchId}' is Valid";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Invalid Branch Id:'{branchId}'";
            }
            return message;
        }

        public static Message ValidateAccountIdFormat(string accountId)
        {
            if (accountId.Length != 17)
            {
                message.Result = false;
                message.ResultMessage = $"Account Id '{accountId}' cannot not be Empty and must be 17 Numbers";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Account Id '{accountId}' Validation Successful";
            }
            return message;
        }
        public static Message ValidateName(string name)
        {
            bool isValidName = Miscellaneous.regex.IsMatch(name);
            if (!isValidName)
            {
                message.Result = false;
                message.ResultMessage = $"Name '{name}' Should contain Only Charecters";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Name '{name}' Validation Successful";
            }
            return message;
        }

        public static Message ValidatePassword(string password)
        {
            bool isValidPassword = Miscellaneous.passwordRegex.IsMatch(password);
            if (!isValidPassword)
            {
                message.Result = false;
                message.ResultMessage = $"Password '{password}' Should contain One Capital,small,numbers,& Special Charecters";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Password '{password}' Validation Successful";
            }

            return message;
        }

        public static Message ValidatePhoneNumber(string phoneNumber)
        {
            bool isValidPhoneNumber = Miscellaneous.phoneNumberRegex.IsMatch(phoneNumber);
            if (!isValidPhoneNumber)
            {
                message.Result = false;
                message.ResultMessage = $"Customer Phone Number '{phoneNumber}' Should contain only numbers";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Phone Number '{phoneNumber}' Validation Successful";
            }

            return message;
        }

        public static Message ValidateEmailId(string emailId)
        {
            bool isValidemail = Miscellaneous.emailRegex.IsMatch(emailId);
            if (!isValidemail)
            {
                message.Result = false;
                message.ResultMessage = $"Email Id '{emailId}' Should contain Valid Email Format Ex.example@example.com";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Email Id '{emailId}' Validation Successful";
            }

            return message;
        }

        public static Message ValidateAccountType(int accountType)
        {
            if (accountType != 1 && accountType != 2)
            {
                message.Result = false;
                message.ResultMessage = $"Entered Account Type '{accountType}'is Invalid., Please Enter Either 1 or 2";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Account Type '{accountType}' Validation Successful";
            }

            return message;
        }

        public static Message ValidateAddress(string address)
        {

            if (address == string.Empty || address.Length < 10)
            {
                message.Result = false;
                message.ResultMessage = $"Entered Address '{address}'., Address Should not be Empty or Less Than 10 Charecters.";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Address:'{address}' Validation Successful.";
            }

            return message;
        }

        public static Message ValidateDateOfBirth(string dateOfBirth)
        {
            bool isValidDob = Miscellaneous.dateRegex.IsMatch(dateOfBirth);
            if (!isValidDob)
            {
                message.Result = false;
                message.ResultMessage = $"Entered Date Of Birth '{dateOfBirth}' is Invalid.,Please Enter in DD/MM/YY Format.";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Date Of Birth '{dateOfBirth}' Validation Successful.";
            }

            return message;
        }

        public static Message ValidateGender(int gender)
        {
            if (gender < 1 && gender > 3)
            {
                message.Result = false;
                message.ResultMessage = $"Entered '{gender}' is Invalid, Please Select Either 1 , 2 or 3";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Gender '{gender}' Validation Successful.";
            }

            return message;
        }

        public static Message ValidateCurrency(string bankId, string currencyCode)
        {
            var bank = banks.FirstOrDefault(b => b.BankId == bankId);
            if (bank != null)
            {
                bool isValidCurrency = bank.Currency.Any(cr => cr.CurrencyCode == currencyCode);
                message.Result = true;
                message.ResultMessage = $"Currency Code:'{currencyCode}' is Existed";
            }
            else
            {
                message.Result = false;
                message.ResultMessage = $"Invalid Currency Code:'{currencyCode}'";
            }
            return message;
        }

    }
}
