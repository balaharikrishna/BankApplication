using BankApplicationModels;
using BankApplicationModels.Enums;
using System.Text.RegularExpressions;

namespace BankApplicationHelperMethods
{
    public static class ValidateInputs
    {
        static Message message = new Message();
        
        private static Regex passwordRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
        private static Regex regex = new Regex("^[a-zA-Z]+$");
        private static Regex phoneNumberRegex = new Regex("^\\d{10}$");
        private static Regex exchangeRateRegex = new Regex(@"\b[A-Z]{3}\s\d{1,3}(,\d{3})*(\.\d+)?\b");
        private static Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        private static Regex dateRegex = new Regex(@"^(0[1-9]|[1-2][0-9]|3[0-1])/(0[1-9]|1[0-2])/(\d{4})$");// Enter a date(DD/MM/YYYY)
      //  public static string passwordComment = "Password Should Contain minimum 8 characters with atleast one uppercase ,one lowercase,one digit,one special character";
        public static Message ValidateBankIdFormat(string bankId)
        {
            if (bankId.Length != 12 )
            {
                message.Result = false;
                message.ResultMessage = $"Bank Id:'{bankId}' Format Is Invalid.";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Bank Id Format:'{bankId}' Is Valid";
            }
            return message;
        }

        public static Message ValidateBranchIdFormat(string branchId)
        {
            if (branchId.Length != 17)
            {
                message.Result = false;
                message.ResultMessage = $"Branch Id:'{branchId}' Format Is Invalid.";
            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Branch Id '{branchId}' Format Is Valid.";
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
        public static Message ValidateNameFormat(string name)
        {
            bool isValidName = regex.IsMatch(name);
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

        public static Message ValidatePasswordFormat(string password)
        {
            bool isValidPassword = passwordRegex.IsMatch(password);
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

        public static Message ValidatePhoneNumberFormat(string phoneNumber)
        {
            bool isValidPhoneNumber = phoneNumberRegex.IsMatch(phoneNumber);
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

        public static Message ValidateEmailIdFormat(string emailId)
        {
            bool isValidemail = emailRegex.IsMatch(emailId);
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

        public static Message ValidateAccountTypeFormat(int accountType)
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

        public static Message ValidateAddressFormat(string address)
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

        public static Message ValidateDateOfBirthFormat(string dateOfBirth)
        {
            bool isValidDob = dateRegex.IsMatch(dateOfBirth);
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

        public static Message ValidateGenderFormat(int gender)
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

        public static Message ValidateCurrencyCodeFormat(string currencyCode)
        {
            if (currencyCode == null || currencyCode.Length != 3 && currencyCode.Length != 4)
            {
                message.Result = false;
                message.ResultMessage = $"Entered CurrencyCode '{currencyCode}' Should Not be NUll & Should be 3 or 4 Charecters only";
       
            }
            else if (regex.IsMatch(currencyCode))
            {
                message.Result = false;
                message.ResultMessage = $"Entered '{currencyCode}' Cannot Contain Special Charecters & Numbers";

            }
            else
            {
                message.Result = true;
                message.ResultMessage = $"Currency Code:'{currencyCode}' Format is Valid";
            }

            return message;
        }

       

    }
}
