namespace BankApplicationHelperMethods
{
    internal class ManagerHelperMethod
    {
        public static void SelectedOption(short Option, string managerBankId, string managerBranchId)
        {
            Message message = new Message();
            BranchManagerService branchManagerService = new BranchManagerService();
            BranchCustomerService branchCustomerService = new BranchCustomerService();
            BranchStaffService branchStaffService = new BranchStaffService();

            switch (Option)
            {
                case 1: //Open Staff Account
                    bool case1Pending = true;
                    while (case1Pending)
                    {
                        string staffName = CommonHelperMethods.GetName(Miscellaneous.staff);
                        string staffPassword = CommonHelperMethods.GetPassword(Miscellaneous.staff);
                        Console.WriteLine("Choose Staff Roles from Below:");
                        foreach (StaffRole option in Enum.GetValues(typeof(StaffRole)))
                        {
                            Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                        }

                        short staffRole = 0;
                        bool isInvalidStaffRole = true;
                        while (isInvalidStaffRole)
                        {
                            Console.WriteLine("Enter Staff Role:");

                            short.TryParse(Console.ReadLine(), out staffRole);
                            if (staffRole == 0)
                            {
                                Console.WriteLine("Please Enter as per the Above Staff Roles");
                                continue;
                            }
                            else
                            {
                                break;
                            }

                        }

                        message = branchManagerService.OpenStaffAccount(staffName, staffPassword, staffRole);
                        if (message.Result)
                        {
                            Console.WriteLine(message.ResultMessage);
                            case1Pending = false;
                            break;

                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
                        }
                    };
                    break;

                case 2: //Add Transaction Charges
                    bool case2Pending = true;
                    while (case2Pending)
                    {
                        Console.WriteLine("Enter RtgsSameBank Charge in %");
                        bool rtgsSameBankPending = true;
                        short rtgsSameBank = 0;
                        while (rtgsSameBankPending)
                        {
                            short.TryParse(Console.ReadLine(), out rtgsSameBank);
                            if (rtgsSameBank >= 100)
                            {
                                Console.WriteLine($"Entered {rtgsSameBank} Value Should not be Greater Than 100%");
                                continue;
                            }
                            else
                            {
                                rtgsSameBankPending = false;
                                break;
                            }
                        }

                        bool rtgsOtherBankPending = true;
                        short rtgsOtherBank = 0;
                        while (rtgsOtherBankPending)
                        {
                            Console.WriteLine("Enter RtgsOtherBank Charge in %");

                            short.TryParse(Console.ReadLine(), out rtgsOtherBank);
                            if (rtgsOtherBank >= 100)
                            {
                                Console.WriteLine($"Entered {rtgsOtherBank} Value Should not be Greater Than 100%");
                                continue;
                            }
                            else
                            {
                                rtgsOtherBankPending = false;
                                break;
                            }
                        }

                        bool impsSameBankPending = true;
                        short impsSameBank = 0;
                        while (impsSameBankPending)
                        {
                            Console.WriteLine("Enter ImpsSameBank Charge in %");

                            short.TryParse(Console.ReadLine(), out impsSameBank);

                            if (impsSameBank >= 100)
                            {
                                Console.WriteLine($"Entered {impsSameBank} Value Should not be Greater Than 100%");
                                continue;
                            }
                            else
                            {
                                impsSameBankPending = false;
                                break;
                            }
                        }

                        bool impsOtherBankPending = true;
                        short impsOtherBank = 0;
                        while (impsOtherBankPending)
                        {
                            Console.WriteLine("Enter ImpsOtherBank Charge in %");

                            short.TryParse(Console.ReadLine(), out impsOtherBank);

                            if (impsOtherBank >= 100)
                            {
                                Console.WriteLine($"Entered {impsOtherBank} Value Should not be Greater Than 100%");
                                continue;
                            }
                            else
                            {
                                impsOtherBankPending = false;
                                break;
                            }
                        }

                        message = branchManagerService.AddTransactionCharges(rtgsSameBank, rtgsOtherBank, impsSameBank, impsOtherBank);
                        if (message.Result)
                        {
                            Console.WriteLine($"Transaction Charges RtgsSameBank:{rtgsSameBank}, RtgsOtherBank:{rtgsOtherBank}, ImpsSameBank:{impsSameBank}, ImpsOtherBank:{impsOtherBank} Added Successfully");

                        }
                        else
                        {
                            Console.WriteLine("Failed to Create Charges Please Try again ");
                            continue;
                        }

                    }
                    break;
                case 3: //OpenCustomerAccount

                    bool case3Pending = true;
                    while (case3Pending)
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
                            case3Pending = false;
                            break;

                        }
                        else
                        {
                            Console.WriteLine(isCustomerAccountOpened.ResultMessage);
                            continue;
                        }
                    };
                    break;

                case 4: //UpdateCustomerAccount
                    bool case4Pending = true;
                    while (case4Pending)
                    {
                        string customerAccountIdForUpdate = CommonHelperMethods.GetAccountId(Miscellaneous.customer);
                        bool isValidCustomer = branchCustomerService.ValidateCustomerAccount(managerBankId, managerBranchId, customerAccountIdForUpdate);
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
                                case4Pending = false;
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

                case 5://DeleteCustomerAccount
                    bool case5Pending = true;
                    while (case5Pending)
                    {
                        string customerAccountIdForDelete = CommonHelperMethods.GetAccountId(Miscellaneous.customer);
                        Message isCustomerAccountDeleted = branchStaffService.DeleteCustomerAccount(customerAccountIdForDelete);
                        if (isCustomerAccountDeleted.Result)
                        {
                            Console.WriteLine(isCustomerAccountDeleted.ResultMessage);
                            case5Pending = false;
                            break;

                        }
                        else
                        {
                            Console.WriteLine(isCustomerAccountDeleted.ResultMessage);
                            continue;
                        }
                    }
                    break;

                case 6://Displaying Customer Transaction History
                    bool case6Pending = true;
                    while (case6Pending)
                    {
                        string customerAccountIdForTransHistory = CommonHelperMethods.GetAccountId(Miscellaneous.customer);
                        bool isValidAccountId = branchCustomerService.ValidateCustomerAccount(managerBankId, managerBranchId, customerAccountIdForTransHistory);
                        if (isValidAccountId)
                        {
                            List<string> transactions = branchCustomerService.GetTransactionHistory();
                            foreach (string transaction in transactions)
                            {

                                Console.WriteLine(transaction);
                                Console.WriteLine();
                            }
                            case6Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Account Not Found For '{customerAccountIdForTransHistory}'");
                            case6Pending = false;
                            break;
                        }

                    }
                    break;

                case 7://Revert Customer Transaction
                    bool case7Pending = true;
                    while (case7Pending)
                    {
                        string fromCustomerAccountId = CommonHelperMethods.GetAccountId(Miscellaneous.customer);

                        bool isFromCustomerValid = branchCustomerService.ValidateCustomerAccount(managerBankId, managerBranchId, fromCustomerAccountId);
                        if (isFromCustomerValid)
                        {
                            string toCustomerBankId = CommonHelperMethods.GetBankId(Miscellaneous.toCustomer);
                            string toCustomerBranchId = CommonHelperMethods.GetBranchId(Miscellaneous.toCustomer);
                            string toCustomerAccountId = CommonHelperMethods.GetAccountId(Miscellaneous.toCustomer);

                            bool isToCustomerValid = branchCustomerService.ValidateToCustomerAccount(toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                            if (isToCustomerValid)
                            {
                                string transactionId = CommonHelperMethods.ValidateTransactionIdFormat();
                                message = branchStaffService.RevertTransaction(transactionId, managerBankId, managerBranchId, fromCustomerAccountId, toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                                Console.WriteLine(message.ResultMessage);
                                case7Pending = false;
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

                case 8://Check Customer Account Balance
                    bool case8Pending = true;
                    while (case8Pending)
                    {
                        string customerAccountIdForBalCheck = CommonHelperMethods.GetAccountId(Miscellaneous.customer);
                        bool isValidAccountId = branchCustomerService.ValidateCustomerAccount(managerBankId, managerBranchId, customerAccountIdForBalCheck);
                        if (isValidAccountId)
                        {
                            Message isBalanceFetchSuccesful = branchCustomerService.CheckAccountBalance();
                            if (isBalanceFetchSuccesful.Result)
                            {
                                Console.WriteLine(isBalanceFetchSuccesful.ResultMessage);
                                case8Pending = false;
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
                            case8Pending = false;
                            break;
                        }

                    }
                    break;

                case 9:// Get ExchangeRates
                    bool case9Pending = true;
                    while (case9Pending)
                    {

                        Dictionary<string, decimal> exchangeRates = BankService.GetExchangeRates(managerBankId);
                        if (exchangeRates != null)
                        {
                            Console.WriteLine("Available Exchange Rates:");
                            foreach (KeyValuePair<string, decimal> rates in exchangeRates)
                            {
                                Console.WriteLine($"{rates.Key}:{rates.Value}₹");
                            }
                            case9Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"ExchangeRates Not Available for {managerBankId}");
                            continue;
                        }
                    }
                    break;

                case 10:// Get TransactionCharges
                    bool case10Pending = true;
                    while (case10Pending)
                    {
                        string transactionCharges = BranchService.GetTransactionCharges(managerBankId, managerBranchId);
                        if (transactionCharges != null)
                        {
                            Console.WriteLine(transactionCharges);
                            case10Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Transaction Charges not Available for {managerBranchId}");
                            continue;
                        }
                    }
                    break;

                case 11://Deposit Amount in Customer Account
                    bool case11Pending = true;
                    while (case11Pending)
                    {
                        string customerAccountId = CommonHelperMethods.GetAccountId(Miscellaneous.customer);
                        decimal depositAmount = CommonHelperMethods.ValidateAmount();
                        string currencyCode = CommonHelperMethods.ValidateCurrency(managerBankId);
                        Message isDepositSuccesful = branchStaffService.DepositAmount(customerAccountId, depositAmount, currencyCode);
                        if (isDepositSuccesful.Result)
                        {
                            Console.WriteLine(isDepositSuccesful.ResultMessage);
                            case11Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine(isDepositSuccesful.ResultMessage);
                            continue;
                        }
                    }
                    break;

                case 12:// Transfer Amount 
                    bool case12Pending = true;
                    while (case12Pending)
                    {
                        string fromCustomerAccountId = CommonHelperMethods.GetAccountId(Miscellaneous.customer);

                        int transferMethod = CommonHelperMethods.ValidateTransferMethod();
                        decimal amount = CommonHelperMethods.ValidateAmount();
                        bool isFromCustomerAccountExist = branchCustomerService.ValidateCustomerAccount(managerBankId, managerBranchId, fromCustomerAccountId);

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
                                        case12Pending = false;
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
                            Console.WriteLine($"Customer Account ID:{fromCustomerAccountId} with BankId:{managerBankId} && BranchId:{managerBranchId} is Not Mathcing with Records.Please try again.");
                            continue;
                        }
                    }
                    break;
            }
        }

    }
}

