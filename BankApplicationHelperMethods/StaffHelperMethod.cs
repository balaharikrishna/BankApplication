namespace BankApplicationHelperMethods
{
    internal static class StaffHelperMethod
    {
        public static void SelectedOption(ushort Option, string staffBankId, string staffBranchId)
        {
            Message message = new Message();
            BranchStaffService branchStaffService = new BranchStaffService();
            BranchCustomerService branchCustomerService = new BranchCustomerService();


            switch (Option)
            {

                case 1: //OpenCustomerAccount

                    bool case1Pending = true;
                    while (case1Pending)
                    {
                        string customerName = CommonHelperMethods.GetName(Miscellaneous.customer);
                        string customerPassword = CommonHelperMethods.GetPassword(Miscellaneous.customer);
                        string customerPhoneNumber = CommonHelperMethods.GetPhoneNumber(Miscellaneous.customer);
                        string customerEmailId = CommonHelperMethods.GetEmailId(Miscellaneous.customer);
                        int customerAccountType = CommonHelperMethods.GetAccountType(Miscellaneous.customer);
                        string customerAddress = CommonHelperMethods.GetAddress(Miscellaneous.customer);
                        string customerDOB = CommonHelperMethods.GetDateOfBirth(Miscellaneous.customer);
                        int customerGender = CommonHelperMethods.GetGender(Miscellaneous.customer);

                        Message isCustomerAccountOpened = branchStaffService.OpenCustomerAccount(customerName, customerPassword, customerPhoneNumber,
                            customerEmailId, customerAccountType, customerAddress, customerDOB, customerGender);
                        if (isCustomerAccountOpened.Result)
                        {
                            Console.WriteLine(isCustomerAccountOpened.ResultMessage);
                            case1Pending = false;
                            break;

                        }
                        else
                        {
                            Console.WriteLine(isCustomerAccountOpened.ResultMessage);
                            continue;
                        }
                    };
                    break;

                case 2: //UpdateCustomerAccount
                    bool case2Pending = true;
                    while (case2Pending)
                    {
                        string customerAccountIdForUpdate = CommonHelperMethods.GetAccountId(Miscellaneous.customer);
                        bool isValidCustomer = branchCustomerService.ValidateCustomerAccount(staffBankId, staffBranchId, customerAccountIdForUpdate);
                        if (isValidCustomer)
                        {
                            string passbookDetatils = branchCustomerService.GetPassbook();
                            Console.WriteLine("Passbook Details:");
                            Console.WriteLine(passbookDetatils);

                            Console.WriteLine("Update Customer Name");
                            string customerName = Console.ReadLine();
                            bool invalidCustomerName = true;
                            while (invalidCustomerName)
                            {
                                if (customerName != string.Empty)
                                {
                                    Message isValidName = ValidateInputs.ValidateName(customerName);
                                    if (isValidName.Result == false)
                                    {
                                        Console.WriteLine(isValidName.ResultMessage);
                                        continue;
                                    }
                                    else
                                    {
                                        invalidCustomerName = false;
                                        break;
                                    }

                                }
                                else
                                {
                                    invalidCustomerName = false;
                                    break;
                                }
                            }

                            Console.WriteLine("Update Customer Password");
                            string customerPassword = Console.ReadLine();
                            bool invalidCustomerPassword = true;
                            while (invalidCustomerPassword)
                            {
                                if (customerPassword != string.Empty)
                                {
                                    Message isValidPassword = ValidateInputs.ValidatePassword(customerPassword);
                                    if (isValidPassword.Result == false)
                                    {
                                        Console.WriteLine(isValidPassword.ResultMessage);
                                        continue;
                                    }
                                    else
                                    {
                                        invalidCustomerPassword = false;
                                        break;
                                    }

                                }
                                else
                                {
                                    invalidCustomerPassword = false;
                                    break;
                                }
                            }

                            Console.WriteLine("Update Customer Phone Number");
                            string customerPhoneNumber = Console.ReadLine();
                            bool invalidCustomerPhoneNumber = true;
                            while (invalidCustomerPhoneNumber)
                            {
                                if (customerPhoneNumber != string.Empty)
                                {
                                    Message isValidPassword = ValidateInputs.ValidatePhoneNumber(customerPhoneNumber);
                                    if (isValidPassword.Result == false)
                                    {
                                        Console.WriteLine(isValidPassword.ResultMessage);
                                        continue;
                                    }
                                    else
                                    {
                                        invalidCustomerPhoneNumber = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    invalidCustomerPhoneNumber = false;
                                    break;
                                }
                            }

                            Console.WriteLine("Update Customer Email Id");
                            string customerEmailId = Console.ReadLine();
                            bool invalidCustomerEmailId = true;
                            while (invalidCustomerEmailId)
                            {
                                if (customerEmailId != string.Empty)
                                {
                                    Message isValidEmailId = ValidateInputs.ValidateEmailId(customerEmailId);
                                    if (isValidEmailId.Result == false)
                                    {
                                        Console.WriteLine(isValidEmailId.ResultMessage);
                                        continue;
                                    }
                                    else
                                    {
                                        invalidCustomerEmailId = false;
                                        break;
                                    }

                                }
                                else
                                {
                                    invalidCustomerEmailId = false;
                                    break;
                                }
                            }


                            Console.WriteLine("Choose From Below Menu Options To Update");
                            foreach (AccountType option in Enum.GetValues(typeof(AccountType)))
                            {
                                Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                            }

                            int customerAccountType;
                            int.TryParse(Console.ReadLine(), out customerAccountType);
                            bool invalidCustomerAccountType = true;
                            while (invalidCustomerAccountType)
                            {
                                if (customerAccountType != 0)
                                {
                                    Message isValidCustomerAccountType = ValidateInputs.ValidateAccountType(customerAccountType);
                                    if (isValidCustomerAccountType.Result == false)
                                    {
                                        Console.WriteLine(isValidCustomerAccountType.ResultMessage);
                                        continue;
                                    }
                                    else
                                    {
                                        invalidCustomerAccountType = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    invalidCustomerAccountType = false;
                                    break;
                                }
                            }

                            Console.WriteLine("Update Customer Address");
                            string customerAddress = Console.ReadLine();
                            bool invalidCustomerAddress = true;
                            while (invalidCustomerAddress)
                            {
                                if (customerAddress != string.Empty)
                                {
                                    Message isValidCustomerAddress = ValidateInputs.ValidateAddress(customerAddress);
                                    if (isValidCustomerAddress.Result == false)
                                    {
                                        Console.WriteLine(isValidCustomerAddress.ResultMessage);
                                        continue;
                                    }
                                    else
                                    {
                                        invalidCustomerAddress = false;
                                        break;
                                    }

                                }
                                else
                                {
                                    invalidCustomerAddress = false;
                                    break;
                                }
                            }

                            Console.WriteLine("Update Customer Date Of Birth");
                            string customerDOB = Console.ReadLine();
                            bool invalidCustomerDob = true;
                            while (invalidCustomerDob)
                            {
                                if (customerDOB != string.Empty)
                                {
                                    Message isValidCustomerDob = ValidateInputs.ValidateDateOfBirth(customerDOB);
                                    if (isValidCustomerDob.Result == false)
                                    {
                                        Console.WriteLine(isValidCustomerDob.ResultMessage);
                                        continue;
                                    }
                                    else
                                    {
                                        invalidCustomerDob = false;
                                        break;
                                    }

                                }
                                else
                                {
                                    invalidCustomerDob = false;
                                    break;
                                }
                            }

                            Console.WriteLine("Choose From Below Menu Options To Update");
                            foreach (Gender option in Enum.GetValues(typeof(Gender)))
                            {
                                Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                            }
                            int customerGender;
                            int.TryParse(Console.ReadLine(), out customerGender);
                            bool invalidCustomerGender = true;
                            while (invalidCustomerGender)
                            {
                                if (customerGender != 0)
                                {
                                    Message isValidCustomerGender = ValidateInputs.ValidateAccountType(customerAccountType);
                                    if (isValidCustomerGender.Result == false)
                                    {
                                        Console.WriteLine(isValidCustomerGender.ResultMessage);
                                        continue;
                                    }
                                    else
                                    {
                                        invalidCustomerGender = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    invalidCustomerGender = false;
                                    break;
                                }
                            }


                            Message isCustomerAccountUpdated = branchStaffService.UpdateCustomerAccount(customerAccountIdForUpdate, customerName, customerPassword, customerPhoneNumber,
                                customerEmailId, customerAccountType, customerAddress, customerDOB, customerGender);
                            if (isCustomerAccountUpdated.Result)
                            {
                                Console.WriteLine(isCustomerAccountUpdated.ResultMessage);
                                case2Pending = false;
                                break;
                            }
                            else
                            {
                                Console.WriteLine(isCustomerAccountUpdated.ResultMessage);
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Account Id:{customerAccountId} is not a Valid Account,Please Enter Correct Account Id");
                            continue;
                        }

                    }
                    break;

                case 3://DeleteCustomerAccount
                    bool case3Pending = true;
                    while (case3Pending)
                    {
                        string customerAccountIdForDelete = CommonHelperMethods.GetAccountId(Miscellaneous.customer);
                        Message isCustomerAccountDeleted = branchStaffService.DeleteCustomerAccount(customerAccountIdForDelete);
                        if (isCustomerAccountDeleted.Result)
                        {
                            Console.WriteLine(isCustomerAccountDeleted.ResultMessage);
                            case3Pending = false;
                            break;

                        }
                        else
                        {
                            Console.WriteLine(isCustomerAccountDeleted.ResultMessage);
                            continue;
                        }
                    }
                    break;

                case 4://Displaying Customer Transaction History
                    bool case4Pending = true;
                    while (case4Pending)
                    {
                        string customerAccountIdForTransHistory = CommonHelperMethods.GetAccountId(Miscellaneous.customer);
                        bool isValidAccountId = branchCustomerService.ValidateCustomerAccount(staffBankId, staffBranchId, customerAccountIdForTransHistory);
                        if (isValidAccountId)
                        {
                            List<string> transactions = branchCustomerService.GetTransactionHistory();
                            foreach (string transaction in transactions)
                            {

                                Console.WriteLine(transaction);
                                Console.WriteLine();
                            }
                            case4Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Account Not Found For '{customerAccountIdForTransHistory}'");
                            case4Pending = false;
                            break;
                        }

                    }
                    break;

                case 5://Revert Customer Transaction
                    bool case5Pending = true;
                    while (case5Pending)
                    {
                        string fromCustomerAccountId = CommonHelperMethods.GetAccountId(Miscellaneous.customer);

                        bool isFromCustomerValid = branchCustomerService.ValidateCustomerAccount(staffBankId, staffBranchId, fromCustomerAccountId);
                        if (isFromCustomerValid)
                        {
                            string toCustomerBankId = CommonHelperMethods.GetBankId(Miscellaneous.toCustomer);
                            string toCustomerBranchId = CommonHelperMethods.GetBranchId(Miscellaneous.toCustomer);
                            string toCustomerAccountId = CommonHelperMethods.GetAccountId(Miscellaneous.toCustomer);

                            bool isToCustomerValid = branchCustomerService.ValidateToCustomerAccount(toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                            if (isToCustomerValid)
                            {
                                string transactionId = CommonHelperMethods.ValidateTransactionIdFormat();
                                message = branchStaffService.RevertTransaction(transactionId, staffBankId, staffBranchId, fromCustomerAccountId, toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                                Console.WriteLine(message.ResultMessage);
                                case5Pending = false;
                                break;
                            }

                        }
                        else
                        {
                            Console.WriteLine($"Customer AccountId:{fromCustomerAccountId} does not Exist");
                            continue;
                        }
                    }
                    break;

                case 6://Check Customer Account Balance
                    bool case6Pending = true;
                    while (case6Pending)
                    {
                        string customerAccountIdForBalCheck = CommonHelperMethods.GetAccountId(Miscellaneous.customer);
                        bool isValidAccountId = branchCustomerService.ValidateCustomerAccount(staffBankId, staffBranchId, customerAccountIdForBalCheck);
                        if (isValidAccountId)
                        {
                            Message isBalanceFetchSuccesful = branchCustomerService.CheckAccountBalance();
                            if (isBalanceFetchSuccesful.Result)
                            {
                                Console.WriteLine(isBalanceFetchSuccesful.ResultMessage);
                                case6Pending = false;
                                break;
                            }
                            else
                            {
                                Console.WriteLine(isBalanceFetchSuccesful.ResultMessage);
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Account Not Found For '{customerAccountIdForBalCheck}'");
                            case4Pending = false;
                            break;
                        }

                    }
                    break;

                case 7:// Get ExchangeRates
                    bool case7Pending = true;
                    while (case7Pending)
                    {

                        Dictionary<string, decimal> exchangeRates = BankService.GetExchangeRates(staffBankId);
                        if (exchangeRates != null)
                        {
                            Console.WriteLine("Available Exchange Rates:");
                            foreach (KeyValuePair<string, decimal> rates in exchangeRates)
                            {
                                Console.WriteLine($"{rates.Key}:{rates.Value}₹");
                            }
                            case7Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"ExchangeRates Not Available for {staffBankId}");
                            continue;
                        }
                    }
                    break;

                case 8:// Get TransactionCharges
                    bool case8Pending = true;
                    while (case8Pending)
                    {
                        string transactionCharges = BranchService.GetTransactionCharges(staffBankId, staffBranchId);
                        if (transactionCharges != null)
                        {
                            Console.WriteLine(transactionCharges);
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Transaction Charges not Available for {staffBranchId}");
                            continue;
                        }
                    }
                    break;

                case 9://Deposit Amount in Customer Account
                    bool case9Pending = true;
                    while (case9Pending)
                    {
                        string customerAccountId = CommonHelperMethods.GetAccountId(Miscellaneous.customer);
                        decimal depositAmount = CommonHelperMethods.ValidateAmount();
                        string currencyCode = CommonHelperMethods.ValidateCurrency(staffBankId);
                        Message isDepositSuccesful = branchStaffService.DepositAmount(customerAccountId, depositAmount, currencyCode);
                        if (isDepositSuccesful.Result)
                        {
                            Console.WriteLine(isDepositSuccesful.ResultMessage);
                            case9Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine(isDepositSuccesful.ResultMessage);
                            continue;
                        }
                    }
                    break;

                case 10:// Transfer Amount 
                    bool case10Pending = true;
                    while (case10Pending)
                    {
                        string fromCustomerAccountId = CommonHelperMethods.GetAccountId(Miscellaneous.customer);

                        int transferMethod = CommonHelperMethods.ValidateTransferMethod();
                        decimal amount = CommonHelperMethods.ValidateAmount();
                        bool isFromCustomerAccountExist = branchCustomerService.ValidateCustomerAccount(staffBankId, staffBranchId, fromCustomerAccountId);

                        if (isFromCustomerAccountExist)
                        {
                            bool isInvalidToCustomer = true;
                            while (isInvalidToCustomer)
                            {
                                string toCustomerBankId = CommonHelperMethods.GetBankId(Miscellaneous.toCustomer);
                                string toCustomerBranchId = CommonHelperMethods.GetBranchId(Miscellaneous.toCustomer);
                                string toCustomerAccountId = CommonHelperMethods.GetAccountId(Miscellaneous.toCustomer);
                                bool isToCustomerAccountExist = branchCustomerService.ValidateToCustomerAccount(toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                                if (isToCustomerAccountExist)
                                {
                                    Message isTransferSuccessful = branchCustomerService.TransferAmount(toCustomerBankId, toCustomerBranchId, toCustomerAccountId, amount, transferMethod);
                                    if (isTransferSuccessful.Result)
                                    {
                                        Console.WriteLine(isTransferSuccessful.ResultMessage);
                                        isInvalidToCustomer = false;
                                        case10Pending = false;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine(isTransferSuccessful.ResultMessage);
                                        continue;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Customer Account ID:{toCustomerAccountId} with BankId:{toCustomerBankId} && BranchId:{toCustomerBranchId} is Not Mathcing with Records.Please try again.");
                                    continue;
                                }
                            }

                        }
                        else
                        {
                            Console.WriteLine($"Customer Account ID:{fromCustomerAccountId} with BankId:{staffBankId} && BranchId:{staffBranchId} is Not Mathcing with Records.Please try again.");
                            continue;
                        }
                    }
                    break;
            }

        }
    }
}
