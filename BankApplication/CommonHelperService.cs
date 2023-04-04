using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;


namespace BankApplication
{
    public class CommonHelperService : ICommonHelperService
    {
        private string bankId = string.Empty;
        private string branchId = string.Empty;

        //It takes the menu options and Process It To and Fro.
        public ushort GetOption(string position)
        {
            while (true)
            {
                Console.WriteLine("Please Enter the Option");
                bool isValidOption = ushort.TryParse(Console.ReadLine(), out ushort Option);
                if (!isValidOption)
                {
                    Console.WriteLine("Option should contain only Positive Numbers.");
                    continue;
                }
                else
                {
                    string errorMessage = $"Entered Option:{Option} is Invalid.Please Select as per Above Options";
                    switch (position)
                    {
                        case "Main Page":
                            while (true)
                            {
                                if (Option == 0 || Option > 5)
                                {
                                    Console.WriteLine(errorMessage);
                                    break;
                                }
                                break;
                            }
                            break;

                        case "Customer":
                            while (true)
                            {
                                if (Option > 7)
                                {
                                    Console.WriteLine(errorMessage);
                                    break;
                                }
                                break;
                            }
                            break;

                        case "Staff":
                            while (true)
                            {
                                if (Option > 10)
                                {
                                    Console.WriteLine(errorMessage);
                                    break;
                                }
                                break;
                            }
                            break;

                        case "Branch Manager":
                            while (true)
                            {
                                if (Option > 16)
                                {
                                    Console.WriteLine(errorMessage);
                                    break;
                                }
                                break;
                            }
                            break;

                        case "Head Manager":
                            while (true)
                            {
                                if (Option > 7)
                                {
                                    Console.WriteLine(errorMessage);
                                    break;
                                }
                                break;
                            }
                            break;

                        case "Reserve Bank Manager":
                            while (true)
                            {
                                if (Option > 4)
                                {
                                    Console.WriteLine(errorMessage);
                                    break;
                                }
                                break;
                            }
                            break;
                    }
                }
                return Option;
            }

        }

        //Takes BankId Input and Validates It.
        public string GetBankId(string position, IBankService _bankService, IValidateInputs _validateInputs)
        {
            Message message;
            while (true)
            {
                Console.WriteLine($"Please Enter {position} BankId:");
                bankId = Console.ReadLine()?.ToUpper() ?? string.Empty;
                Message isValidbankId = _validateInputs.ValidateBankIdFormat(bankId);
                if (isValidbankId.Result)
                {
                    message = _bankService.AuthenticateBankId(bankId);
                    if (message.Result)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine(isValidbankId.ResultMessage);
                    continue;
                }
            }
            return bankId;
        }

        //Takes BranchId Input and Validates It.
        public string GetBranchId(string position, IBranchService _branchService, IValidateInputs _validateInputs)
        {
            Message message ;
            while (true)
            {
                Console.WriteLine($"Please Enter {position} BranchId:");
                branchId = Console.ReadLine()?.ToUpper() ?? string.Empty;
                Message isValidBranchId = _validateInputs.ValidateBranchIdFormat(branchId);
                if (isValidBranchId.Result)
                {
                    message = _branchService.AuthenticateBranchId(bankId, branchId);
                    if (message.Result)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine(isValidBranchId.ResultMessage);
                    continue;
                }

            }
            return branchId;
        }

        //Validate  AccountId
        public string GetAccountId(string position, IValidateInputs _validateInputs)
        {
            string accountId;
            while (true)
            {
                Console.WriteLine($"Enter {position} AccountId:");
                accountId = Console.ReadLine()?.ToUpper() ?? string.Empty;
                Message isValidAccount = _validateInputs.ValidateAccountIdFormat(accountId);
                if (isValidAccount.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidAccount.ResultMessage);
                    continue;
                }
            }
            return accountId;
        }

        //Validate  Name
        public string GetName(string position, IValidateInputs _validateInputs)
        {
            string name;
            while (true)
            {
                Console.WriteLine($"Enter {position} Name:");
                name = Console.ReadLine()?.ToUpper().Replace(" ", "").ToUpper() ?? string.Empty;
                Message isValidName = _validateInputs.ValidateNameFormat(name);
                if (isValidName.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidName.ResultMessage);
                    continue;
                }

            }
            return name;
        }

        //Validate  Password
        public string GetPassword(string position, IValidateInputs _validateInputs)
        {
            string password;
            while (true)
            {
                Console.WriteLine($"Enter {position} Password:");
                password = Console.ReadLine() ?? string.Empty;
                Message isValidPassword = _validateInputs.ValidatePasswordFormat(password);
                if (isValidPassword.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidPassword.ResultMessage);
                    continue;
                }

            }
            return password;
        }

        //Validate  Phone Number
        public string GetPhoneNumber(string position, IValidateInputs _validateInputs)
        {
            string phoneNumber;
            while (true)
            {
                Console.WriteLine($"Enter {position} Phone Number:");
                phoneNumber = Console.ReadLine() ?? string.Empty;
                Message isValidPhoneNumber = _validateInputs.ValidatePhoneNumberFormat(phoneNumber);
                if (isValidPhoneNumber.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidPhoneNumber.ResultMessage);
                    continue;
                }

            }
            return phoneNumber;
        }

        // Validate  email
        public string GetEmailId(string position, IValidateInputs _validateInputs)
        {
            string emailId;
            while (true)
            {
                Console.WriteLine($"Enter {position} Email Id:");
                emailId = Console.ReadLine()?.ToUpper() ?? string.Empty;
                Message isValidEmail = _validateInputs.ValidateEmailIdFormat(emailId);
                if (isValidEmail.Result)
                {
                    break;
                }
                else
                {

                    Console.WriteLine(isValidEmail.ResultMessage);
                    continue;
                }
            }
            return emailId;
        }

        // Validate  account type
        public int GetAccountType(string position, IValidateInputs _validateInputs)
        {
            int accountType;
            while (true)
            {
                Console.WriteLine($"Enter {position} Account Type:");
                foreach (AccountType type in Enum.GetValues(typeof(AccountType)))
                {
                    Console.WriteLine("Enter {0} For {1}", (int)type, type.ToString());
                }

                accountType = int.Parse(Console.ReadLine() ?? string.Empty);
                Message isValidAccountType = _validateInputs.ValidateAccountTypeFormat(accountType);
                if (isValidAccountType.Result)
                {
                    break;

                }
                else
                {
                    Console.WriteLine(isValidAccountType.ResultMessage);
                    continue;
                }
            }
            return accountType;
        }

        // Validate address
        public string GetAddress(string position, IValidateInputs _validateInputs)
        {
            string address;
            while (true)
            {
                Console.WriteLine($"Enter {position} Address:");
                address = Console.ReadLine()?.ToUpper() ?? string.Empty;
                Message isValidAddress = _validateInputs.ValidateAddressFormat(address);
                if (isValidAddress.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidAddress.ResultMessage);
                    continue;
                }
            }
            return address;
        }

        // Validates  date of birth 
        public string GetDateOfBirth(string position, IValidateInputs _validateInputs)
        {
            string dateOfBirth;
            while (true)
            {
                Console.WriteLine($"Enter {position} Date Of Birth Ex:27/06/97(DD/MM/YY):");
                dateOfBirth = Console.ReadLine() ?? string.Empty;
                Message isValidDOB = _validateInputs.ValidateDateOfBirthFormat(dateOfBirth);
                if (isValidDOB.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidDOB.ResultMessage);
                    continue;
                }
            }
            return dateOfBirth;
        }

        //Checks whether the given GenderOption is valid or not.
        public int GetGender(string position, IValidateInputs _validateInputs)
        {
            int genderType;
            while (true)
            {
                Console.WriteLine($"Enter {position} Gender:");
                foreach (Gender gender in Enum.GetValues(typeof(Gender)))
                {
                    Console.WriteLine("Enter {0} For {1}", (int)gender, gender.ToString());
                }

                genderType = int.Parse(Console.ReadLine() ?? string.Empty);
                Message isValidGender = _validateInputs.ValidateGenderFormat(genderType);
                if (isValidGender.Result)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(isValidGender.ResultMessage);
                    continue;
                }
            }
            return genderType;
        }
        //Checks whether the given transferOptions are valid or not.
        public int ValidateTransferMethod()
        {
            int result;
            while (true)
            {
                Console.WriteLine("Enter the Transfer Method");
                foreach (TransferMethod method in Enum.GetValues(typeof(TransferMethod)))
                {
                    Console.WriteLine("Enter {0} For {1}", (int)method, method.ToString());
                }

                int choice = int.Parse(Console.ReadLine() ?? string.Empty);

                if (choice != 1 && choice != 2)
                {
                    Console.WriteLine("Enter as per Above Choices.");
                    continue;
                }
                else
                {
                    result = choice;
                    break;
                }
            }
            return result;
        }

        //Checks whether the given amount is valid or not.
        public decimal ValidateAmount()
        {
            decimal result;
            while (true)
            {
                Console.WriteLine("Enter the Amount");

                decimal amount = decimal.Parse(Console.ReadLine() ?? string.Empty);

                if (amount < 1)
                {
                    Console.WriteLine($"Entered Amount:{amount} is Invalid.,Please Enter a Valid Amout Ex:1");
                    continue;
                }
                else
                {
                    result = amount;
                    break;
                }
            }
            return result;
        }

        //checks the format and currency already exist or not 
        public string ValidateCurrency(string bankId, ICurrencyService _currencyService, IValidateInputs _validateInputs)
        {
            Message message;
            string result;
            while (true)
            {
                Console.WriteLine("Enter the Currency Name Ex:USD,KWD.");
                string currencyCode = Console.ReadLine()?.ToUpper() ?? string.Empty;
                message = _validateInputs.ValidateCurrencyCodeFormat(currencyCode);

                if (message.Result)
                {
                    message = _currencyService.ValidateCurrency(bankId, currencyCode);
                    if (message.Result)
                    {
                        result = currencyCode;
                        break;
                    }
                    else
                    {
                        Console.WriteLine(message.ResultMessage);
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine(message.ResultMessage);
                    continue;
                }
            }
            return result;
        }

        public string ValidateTransactionIdFormat()
        {
            string result;
            while (true)
            {
                Console.WriteLine("Enter the Transaction Id");
                string transactionId = Console.ReadLine()?.ToUpper() ?? string.Empty;

                if (transactionId.Length == 23 && transactionId.Contains("TXN"))
                {
                    result = transactionId;
                    break;
                }
                else
                {
                    Console.WriteLine($"TransactionId:{transactionId} Is Invalid");
                    continue;
                }
            }
            return result;
        }
    }
}
