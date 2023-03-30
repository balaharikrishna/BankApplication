using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;


namespace BankApplication
{
    public class CommonHelperService : ICommonHelperService
    {
   //     private IBankService? _bankService;
        private IBranchService? _branchService;
        private ICurrencyService? _currencyService;
   //     private IValidateInputs? _validateInputs;

        Message message = new Message();

        private string bankId = string.Empty;
        private string branchId = string.Empty;

        //It takes the menu options and Process It To and Fro.
        public ushort GetOption(string position)
        {
            ushort result = 0;
            bool isInvalidOption = true;
            while (isInvalidOption)
            {
                Console.WriteLine("Please Enter the Option");
                ushort Option;
                bool isValidOption = ushort.TryParse(Console.ReadLine(), out Option);
                if (isValidOption == false)
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
                            bool invalidMainPageOption = true;
                            while (invalidMainPageOption)
                            {
                                if (Option == 0 || Option > 5)
                                {
                                    Console.WriteLine(errorMessage);
                                    invalidMainPageOption = false;
                                    break;
                                }
                                else
                                {
                                    result = Option;
                                    invalidMainPageOption = false;
                                    isInvalidOption = false;
                                    break;
                                }
                            }
                            break;

                        case "Customer":
                            bool invalidCustomerOption = true;
                            while (invalidCustomerOption)
                            {
                                if (Option > 7)
                                {
                                    Console.WriteLine(errorMessage);
                                    invalidCustomerOption = false;
                                }
                                else
                                {
                                    invalidCustomerOption = false;
                                    isInvalidOption = false;
                                    result = Option;
                                    break;
                                }
                            }
                            break;

                        case "Staff":
                            bool invalidStaffOption = true;
                            while (invalidStaffOption)
                            {
                                if (Option > 10)
                                {
                                    Console.WriteLine(errorMessage);
                                    invalidStaffOption = false;
                                    break;
                                }
                                else
                                {
                                    invalidStaffOption = false;
                                    isInvalidOption = false;
                                    result = Option;
                                    break;
                                }
                            }
                            break;

                        case "Branch Manager":
                            bool invalidBranchManagerOption = true;
                            while (invalidBranchManagerOption)
                            {
                                if (Option > 16)
                                {
                                    Console.WriteLine(errorMessage);
                                    invalidBranchManagerOption = false;
                                    break;
                                }
                                else
                                {
                                    invalidBranchManagerOption = false;
                                    isInvalidOption = false;
                                    result = Option;
                                    break;
                                }
                            }
                            break;
                        case "Head Manager":
                            bool invalidBankHeadManagerOption = true;
                            while (invalidBankHeadManagerOption)
                            {
                                if (Option > 7)
                                {
                                    Console.WriteLine(errorMessage);
                                    invalidBankHeadManagerOption = false;
                                    break;
                                }
                                else
                                {
                                    invalidBankHeadManagerOption = false;
                                    isInvalidOption = false;
                                    result = Option;
                                    break;
                                }
                            }
                            break;

                        case "Reserve Bank Manager":
                            bool invalidReserveBankManagerOption = true;
                            while (invalidReserveBankManagerOption)
                            {
                                if (Option > 4)
                                {
                                    Console.WriteLine(errorMessage);
                                    invalidReserveBankManagerOption = false;
                                    break;
                                }
                                else
                                {
                                    invalidReserveBankManagerOption = false;
                                    isInvalidOption = false;
                                    result = Option;
                                    break;
                                }
                            }
                            break;

                    }
                }
            }
            return result;
        }

        //Takes BankId Input and Validates It.
        public string GetBankId(string position, IBankService _bankService,IValidateInputs _validateInputs)
        {
            bool isInvalidBank = true;
            while (isInvalidBank)
            {
                Console.WriteLine($"Please Enter {position} BankId:");
                bankId = Console.ReadLine().ToUpper() ?? string.Empty;
                Message isValidbankId = _validateInputs.ValidateBankIdFormat(bankId);
                if (isValidbankId.Result)
                {
                    message = _bankService.AuthenticateBankId(bankId);
                    if (message.Result)
                    {
                        isInvalidBank = false;
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
        public string GetBranchId(string position, IBranchService branchService, IValidateInputs _validateInputs)
        {
            _branchService = branchService;
            bool isInvalidBranchId = true;
            while (isInvalidBranchId)
            {

                Console.WriteLine($"Please Enter {position} BranchId:");
                branchId = Console.ReadLine() ?? string.Empty.ToUpper();
                Message isValidBranchId = _validateInputs.ValidateBranchIdFormat(branchId);
                if (isValidBranchId.Result)
                {
                    message = _branchService.AuthenticateBranchId(bankId, branchId);
                    if (message.Result)
                    {
                        isInvalidBranchId = false;
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
            string accountId = string.Empty;
            bool isAccountIdPending = true;
            while (isAccountIdPending)
            {
                Console.WriteLine($"Enter {position} AccountId:");
                accountId = Console.ReadLine().ToUpper() ?? string.Empty;
                Message isValidAccount = _validateInputs.ValidateAccountIdFormat(accountId);
                if (isValidAccount.Result)
                {
                    isAccountIdPending = false;
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
            string name = string.Empty;
            bool isInvalidName = true;
            while (isInvalidName)
            {
                Console.WriteLine($"Enter {position} Name:");
                name = Console.ReadLine().Replace(" ","").ToUpper() ?? string.Empty;
                Message isValidName = _validateInputs.ValidateNameFormat(name);
                if (isValidName.Result)
                {
                    isInvalidName = false;
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
            string password = string.Empty;
            bool isInvalidPassword = true;
            while (isInvalidPassword)
            {
                Console.WriteLine($"Enter {position} Password:");
                password = Console.ReadLine() ?? string.Empty;
                Message isValidPassword = _validateInputs.ValidatePasswordFormat(password);
                if (isValidPassword.Result)
                {
                    isInvalidPassword = false;
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
            string phoneNumber = string.Empty;
            bool isInvalidPhoneNumber = true;
            while (isInvalidPhoneNumber)
            {
                Console.WriteLine($"Enter {position} Phone Number:");
                phoneNumber = Console.ReadLine() ?? string.Empty;
                Message isValidPhoneNumber = _validateInputs.ValidatePhoneNumberFormat(phoneNumber);
                if (isValidPhoneNumber.Result)
                {
                    isInvalidPhoneNumber = false;
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
            string emailId = string.Empty;
            bool isInvalidEmail = true;
            while (isInvalidEmail)
            {
                Console.WriteLine($"Enter {position} Email Id:");
                emailId = Console.ReadLine().ToUpper() ?? string.Empty;
                Message isValidEmail = _validateInputs.ValidateEmailIdFormat(emailId);
                if (isValidEmail.Result)
                {
                    isInvalidEmail = false;
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
            int accountType = 0;
            bool isInvalidAccountType = true;
            while (isInvalidAccountType)
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
                    isInvalidAccountType = false;
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
            string address = string.Empty;
            bool isInvalidAddress = true;
            while (isInvalidAddress)
            {
                Console.WriteLine($"Enter {position} Address:");
                address = Console.ReadLine().ToUpper() ?? string.Empty;
                Message isValidAddress = _validateInputs.ValidateAddressFormat(address);
                if (isValidAddress.Result)
                {
                    isInvalidAddress = false;
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
            string dateOfBirth = string.Empty;
            bool isInvalidDOB = true;
            while (isInvalidDOB)
            {
                Console.WriteLine($"Enter {position} Date Of Birth Ex:27/06/97(DD/MM/YY):");
                dateOfBirth = Console.ReadLine() ?? string.Empty;
                Message isValidDOB = _validateInputs.ValidateDateOfBirthFormat(dateOfBirth);
                if (isValidDOB.Result)
                {
                    isInvalidDOB = false;
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
            int genderType = 0;
            bool isInvalidGender = true;
            while (isInvalidGender)
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
                    isInvalidGender = false;
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
            int result = 0;
            bool isInvalidChoice = true;
            while (isInvalidChoice)
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
                    isInvalidChoice = false;
                    result = choice;
                    break;
                }

            }
            return result;
        }

        //Checks whether the given amount is valid or not.
        public decimal ValidateAmount()
        {
            decimal result = 0;
            bool isInvalidAmount = true;
            while (isInvalidAmount)
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
                    isInvalidAmount = false;
                    result = amount;
                    break;
                }

            }
            return result;
        }

        //checks the format and currency already exist or not 
        public string ValidateCurrency(string bankId, ICurrencyService currencyService, IValidateInputs _validateInputs)
        {
            _currencyService = currencyService;
            string result = string.Empty;
            bool isInvalidCurrency = true;
            while (isInvalidCurrency)
            {
                Console.WriteLine("Enter the Currency Name Ex:USD,KWD.");
                string currencyCode = Console.ReadLine().ToUpper() ?? string.Empty;
                message = _validateInputs.ValidateCurrencyCodeFormat(currencyCode);

                if (message.Result)
                {
                    message = _currencyService.ValidateCurrency(bankId, currencyCode);
                    if (message.Result)
                    {
                        isInvalidCurrency = false;
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
            string result = string.Empty;
            bool isInvalidTransactionId = true;
            while (isInvalidTransactionId)
            {
                Console.WriteLine("Enter the Transaction Id");
                string transactionId = Console.ReadLine().ToUpper() ?? string.Empty;

                if (transactionId.Length == 19 && transactionId.Contains("TXN"))
                {
                    isInvalidTransactionId = false;
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
