using BankApplication.Models.Enums;

namespace BankApplication
{
    internal static class CommonHelperMethods
    {
        private static string bankId = string.Empty;
        private static string branchId = string.Empty;

        public static ushort GetOption(string position)
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
                                if (Option > 5)
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
                                if (Option > 12)
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
                                if (Option > 3)
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
                                if (Option > 2)
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

        public static string GetBankId(string position)
        {
            bool isInvalidBank = true;
            while (isInvalidBank)
            {
                Console.WriteLine($"Please Enter {position} BankId:");
                bankId = Console.ReadLine().ToUpper();
                Message isValidbankId = ValidateInputs.ValidateBankId(bankId);
                if (isValidbankId.Result)
                {
                    isInvalidBank = false;
                    break;
                }
                else
                {
                    Console.WriteLine(isValidbankId.ResultMessage);
                    continue;
                }
            }
            return bankId;
        }

        public static string GetBranchId(string position)
        {
            bool isInvalidBranchId = true;
            while (isInvalidBranchId)
            {

                Console.WriteLine($"Please Enter {position} BranchId:");
                branchId = Console.ReadLine().ToUpper();
                Message isValidBranchId = ValidateInputs.ValidateBranchId(bankId, branchId);
                if (isValidBranchId.Result)
                {
                    isInvalidBranchId = false;
                    break;

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
        public static string GetAccountId(string position)
        {
            string accountId = string.Empty;
            bool isAccountIdPending = true;
            while (isAccountIdPending)
            {
                Console.WriteLine($"Enter {position} AccountId:");
                accountId = Console.ReadLine().ToUpper();
                Message isValidAccount = ValidateInputs.ValidateAccountIdFormat(accountId);
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

        public static string GetName(string position) //Validate  Name
        {
            string name = string.Empty;
            bool isInvalidName = true;
            while (isInvalidName)
            {
                Console.WriteLine($"Enter {position} Name:");
                name = Console.ReadLine().ToUpper();
                Message isValidName = ValidateInputs.ValidateName(name);
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

        public static string GetPassword(string position) //Validate  Password
        {
            string password = string.Empty;
            bool isInvalidPassword = true;
            while (isInvalidPassword)
            {
                Console.WriteLine($"Enter {position} Password:");
                password = Console.ReadLine();
                Message isValidPassword = ValidateInputs.ValidatePassword(password);
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

        public static string GetPhoneNumber(string position) //Validate  Phone Number
        {
            string phoneNumber = string.Empty;
            bool isInvalidPhoneNumber = true;
            while (isInvalidPhoneNumber)
            {
                Console.WriteLine($"Enter {position} Phone Number:");
                phoneNumber = Console.ReadLine();
                Message isValidPhoneNumber = ValidateInputs.ValidatePhoneNumber(phoneNumber);
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

        public static string GetEmailId(string position) // Validate  email
        {
            string emailId = string.Empty;
            bool isInvalidEmail = true;
            while (isInvalidEmail)
            {
                Console.WriteLine($"Enter {position} Email Id:");
                emailId = Console.ReadLine().ToUpper();
                Message isValidEmail = ValidateInputs.ValidateEmailId(emailId);
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

        public static int GetAccountType(string position) // Validate  account type
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

                accountType = int.Parse(Console.ReadLine());
                Message isValidAccountType = ValidateInputs.ValidateAccountType(accountType);
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

        public static string GetAddress(string position) // Validate address
        {
            string address = string.Empty;
            bool isInvalidAddress = true;
            while (isInvalidAddress)
            {
                Console.WriteLine($"Enter {position} Address:");
                address = Console.ReadLine().ToUpper();
                Message isValidAddress = ValidateInputs.ValidateAddress(address);
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

        public static string GetDateOfBirth(string position) // Validate  date of birth
        {
            string dateOfBirth = string.Empty;
            bool isInvalidDOB = true;
            while (isInvalidDOB)
            {
                Console.WriteLine($"Enter {position} Date Of Birth Ex:27/06/97(DD/MM/YY):");
                dateOfBirth = Console.ReadLine();
                Message isValidDOB = ValidateInputs.ValidateDateOfBirth(dateOfBirth);
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

        public static int GetGender(string position) // Validate  gender
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

                genderType = int.Parse(Console.ReadLine());
                Message isValidGender = ValidateInputs.ValidateGender(genderType);
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

        public static int ValidateTransferMethod()
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

                int choice = int.Parse(Console.ReadLine());

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

        public static decimal ValidateAmount()
        {
            decimal result = 0;
            bool isInvalidAmount = true;
            while (isInvalidAmount)
            {
                Console.WriteLine("Enter the Amount");

                decimal amount = decimal.Parse(Console.ReadLine());

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

        public static string ValidateCurrency(string bankId)  //checks the format and currency already exist or not 
        {
            string result = string.Empty;
            bool isInvalidCurrency = true;
            while (isInvalidCurrency)
            {
                Console.WriteLine("Enter the Currency Name Ex:USD,KWD.");
                string currencyCode = Console.ReadLine().ToUpper();
                if (currencyCode == null || currencyCode.Length != 3 && currencyCode.Length != 4)
                {
                    Console.WriteLine($"Entered CurrencyCode '{currencyCode}' Should Not be NUll & Should be 3 or 4 Charecters only");
                    continue;
                }
                else if (Miscellaneous.regex.IsMatch(currencyCode))
                {
                    Console.WriteLine($"Entered '{currencyCode}' Cannot Contain Special Charecters & Numbers");
                    continue;
                }
                else
                {
                    Message isValidCurrency = ValidateInputs.ValidateCurrency(bankId, currencyCode);
                    if (isValidCurrency.Result)
                    {
                        Console.WriteLine(isValidCurrency.ResultMessage);
                        continue;

                    }
                    else
                    {
                        isInvalidCurrency = false;
                        result = currencyCode;
                        break;

                    }
                }

            }
            return result;
        }

        public static string ValidateTransactionIdFormat()
        {
            string result = string.Empty;
            bool isInvalidTransactionId = true;
            while (isInvalidTransactionId)
            {
                Console.WriteLine("Enter the Transaction Id");
                string transactionId = Console.ReadLine().ToUpper();

                if (transactionId.Length == 46 && transactionId.Contains("TXN"))
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
