using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;
using Newtonsoft.Json;

namespace BankApplication
{
    public class ManagerHelperService : IManagerHelperService
    {
        IBankService _bankService;
        IBranchService _branchService;
        IStaffService _staffService;
        ICustomerService _customerService;
        ICommonHelperService _commonHelperService;
        ITransactionChargeService _transactionChargeService;
        IValidateInputs _validateInputs;
        ITransactionService _transactionService;
        ICurrencyService _currencyService;
        public ManagerHelperService(IBankService bankService, IBranchService branchService,
        IStaffService staffService, ICustomerService customerService, ICommonHelperService commonHelperService,
        ITransactionChargeService transactionChargeService, IValidateInputs validateInputs,
        ITransactionService transactionService, ICurrencyService currencyService)
        {
            _bankService = bankService;
            _branchService = branchService;
            _staffService = staffService;
            _customerService = customerService;
            _commonHelperService = commonHelperService;
            _transactionChargeService = transactionChargeService;
            _validateInputs = validateInputs;
            _transactionService = transactionService;
            _currencyService = currencyService;
        }
        Message message = new Message();
        public void SelectedOption(ushort Option, string managerBankId, string managerBranchId, string managerAccountId)
        {
            switch (Option)
            {
                case 1: //Open Staff Account
                    bool case1Pending = true;
                    while (case1Pending)
                    {
                        string staffName = _commonHelperService.GetName(Miscellaneous.staff, _validateInputs);
                        string staffPassword = _commonHelperService.GetPassword(Miscellaneous.staff, _validateInputs);
                        Console.WriteLine("Choose Staff Roles from Below:");
                        foreach (StaffRole option in Enum.GetValues(typeof(StaffRole)))
                        {
                            Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                        }

                        StaffRole staffRole = 0;
                        bool isInvalidStaffRole = true;
                        while (isInvalidStaffRole)
                        {
                            Console.WriteLine("Enter Staff Role:");

                            StaffRole.TryParse(Console.ReadLine(), out staffRole);
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

                        message = _staffService.OpenStaffAccount(managerBankId, managerBranchId, staffName, staffPassword, staffRole);
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
                        ushort rtgsSameBank = 0;
                        while (rtgsSameBankPending)
                        {
                            bool isValidValue = ushort.TryParse(Console.ReadLine(), out rtgsSameBank);

                            if (!isValidValue)
                            {
                                Console.WriteLine($"Entered Value should not be Empty and Contain Only Numbers");
                            }
                            else if (isValidValue && rtgsSameBank >= 100)
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
                        ushort rtgsOtherBank = 0;
                        while (rtgsOtherBankPending)
                        {
                            Console.WriteLine("Enter RtgsOtherBank Charge in %");

                            bool isValidValue = ushort.TryParse(Console.ReadLine(), out rtgsOtherBank);
                            if (!isValidValue)
                            {
                                Console.WriteLine($"Entered Value should not be Empty and Contain Only Numbers");
                            }
                            else if (isValidValue && rtgsOtherBank >= 100)
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
                        ushort impsSameBank = 0;
                        while (impsSameBankPending)
                        {
                            Console.WriteLine("Enter ImpsSameBank Charge in %");

                            bool isValidValue = ushort.TryParse(Console.ReadLine(), out impsSameBank);
                            if (!isValidValue)
                            {
                                Console.WriteLine($"Entered Value should not be Empty and Contain Only Numbers");
                            }
                            else if (isValidValue && impsSameBank >= 100)
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
                        ushort impsOtherBank = 0;
                        while (impsOtherBankPending)
                        {
                            Console.WriteLine("Enter ImpsOtherBank Charge in %");

                            bool isValidValue = ushort.TryParse(Console.ReadLine(), out impsOtherBank);
                            if (!isValidValue)
                            {
                                Console.WriteLine($"Entered Value should not be Empty and Contain Only Numbers");
                            }
                            else if (isValidValue && impsOtherBank >= 100)
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

                        message = _transactionChargeService.AddTransactionCharges(managerBankId, managerBranchId, rtgsSameBank, rtgsOtherBank, impsSameBank, impsOtherBank);
                        if (message.Result)
                        {
                            Console.WriteLine(message.ResultMessage);
                            Console.WriteLine();
                            case2Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
                        }
                    }
                    break;

                case 3: //OpenCustomerAccount

                    bool case3Pending = true;
                    while (case3Pending)
                    {
                        string customerName = _commonHelperService.GetName(Miscellaneous.customer, _validateInputs);
                        string customerPassword = _commonHelperService.GetPassword(Miscellaneous.customer, _validateInputs);
                        string customerPhoneNumber = _commonHelperService.GetPhoneNumber(Miscellaneous.customer, _validateInputs);
                        string customerEmailId = _commonHelperService.GetEmailId(Miscellaneous.customer, _validateInputs);
                        int customerAccountType = _commonHelperService.GetAccountType(Miscellaneous.customer, _validateInputs);
                        string customerAddress = _commonHelperService.GetAddress(Miscellaneous.customer, _validateInputs);
                        string customerDOB = _commonHelperService.GetDateOfBirth(Miscellaneous.customer, _validateInputs);
                        int customerGender = _commonHelperService.GetGender(Miscellaneous.customer, _validateInputs);

                        Message isCustomerAccountOpened = _customerService.OpenCustomerAccount(managerBankId,
                        managerBranchId, customerName, customerPassword, customerPhoneNumber, customerEmailId,
                        customerAccountType, customerAddress, customerDOB, customerGender);
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
                        string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer, _validateInputs);
                        message = _customerService.IsCustomersExist(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            message = _customerService.IsAccountExist(managerBankId, managerBranchId, customerAccountId);
                            if (message.Result)
                            {
                                string passbookDetatils = _customerService.GetPassbook(managerBankId, managerBranchId, customerAccountId);
                                Console.WriteLine("Passbook Details:");
                                Console.WriteLine(passbookDetatils);

                                bool invalidCustomerName = true;
                                string customerName = string.Empty;
                                while (invalidCustomerName)
                                {
                                    Console.WriteLine("Update Customer Name");
                                    customerName = Console.ReadLine() ?? string.Empty;
                                    if (customerName != string.Empty)
                                    {
                                        message = _validateInputs.ValidateNameFormat(customerName);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
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

                                bool invalidCustomerPassword = true;
                                string customerPassword = string.Empty;
                                while (invalidCustomerPassword)
                                {
                                    Console.WriteLine("Update Customer Password");
                                    customerPassword = Console.ReadLine() ?? string.Empty;
                                    if (customerPassword != string.Empty)
                                    {
                                        message = _validateInputs.ValidatePasswordFormat(customerPassword);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
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

                                bool invalidCustomerPhoneNumber = true;
                                string customerPhoneNumber = string.Empty;
                                while (invalidCustomerPhoneNumber)
                                {
                                    Console.WriteLine("Update Customer Phone Number");
                                    customerPhoneNumber = Console.ReadLine() ?? string.Empty;
                                    if (customerPhoneNumber != string.Empty)
                                    {
                                        message = _validateInputs.ValidatePhoneNumberFormat(customerPhoneNumber);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
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

                                bool invalidCustomerEmailId = true;
                                string customerEmailId = string.Empty;
                                while (invalidCustomerEmailId)
                                {
                                    Console.WriteLine("Update Customer Email Id");
                                    customerEmailId = Console.ReadLine() ?? string.Empty;
                                    if (customerEmailId != string.Empty)
                                    {
                                        message = _validateInputs.ValidateEmailIdFormat(customerEmailId);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
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

                                bool invalidCustomerAccountType = true;
                                int customerAccountType = 0;
                                while (invalidCustomerAccountType)
                                {
                                    Console.WriteLine("Choose From Below Menu Options To Update");
                                    foreach (AccountType option in Enum.GetValues(typeof(AccountType)))
                                    {
                                        Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                    }
                                    bool isValid = int.TryParse(Console.ReadLine(), out customerAccountType);
                                    if (isValid && customerAccountType != 0)
                                    {
                                        message = _validateInputs.ValidateAccountTypeFormat(customerAccountType);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
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

                                bool invalidCustomerAddress = true;
                                string customerAddress = string.Empty;
                                while (invalidCustomerAddress)
                                {
                                    Console.WriteLine("Update Customer Address");
                                    customerAddress = Console.ReadLine() ?? string.Empty;
                                    if (customerAddress != string.Empty)
                                    {
                                        message = _validateInputs.ValidateAddressFormat(customerAddress);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
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

                                bool invalidCustomerDob = true;
                                string customerDOB = string.Empty;
                                while (invalidCustomerDob)
                                {
                                    Console.WriteLine("Update Customer Date Of Birth");
                                    customerDOB = Console.ReadLine() ?? string.Empty;
                                    if (customerDOB != string.Empty)
                                    {
                                        message = _validateInputs.ValidateDateOfBirthFormat(customerDOB);
                                        if (message.Result == false)
                                        {
                                            Console.WriteLine(message.ResultMessage);
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

                                bool invalidCustomerGender = true;
                                int customerGender = 0;
                                while (invalidCustomerGender)
                                {
                                    Console.WriteLine("Choose From Below Menu Options To Update");
                                    foreach (Gender option in Enum.GetValues(typeof(Gender)))
                                    {
                                        Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                    }
                                    bool isValid = int.TryParse(Console.ReadLine(), out customerGender);
                                    if (customerGender != 0)
                                    {
                                        message = _validateInputs.ValidateGenderFormat(customerGender);
                                        if (message.Result == false)
                                        {
                                            Console.WriteLine(message.ResultMessage);
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

                                message = _customerService.UpdateCustomerAccount(managerBankId, managerBranchId, customerAccountId, customerName, customerPassword, customerPhoneNumber,
                                    customerEmailId, customerAccountType, customerAddress, customerDOB, customerGender);
                                if (message.Result)
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    case4Pending = false;
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
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            case4Pending = false;
                            break;
                        }
                    }
                    break;

                case 5://DeleteCustomerAccount
                    bool case5Pending = true;
                    while (case5Pending)
                    {

                        message = _customerService.IsCustomersExist(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer, _validateInputs);
                            message = _customerService.DeleteCustomerAccount(managerBankId, managerBranchId, customerAccountId);
                            if (message.Result)
                            {
                                Console.WriteLine(message.ResultMessage);
                                case5Pending = false;
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
                            case5Pending = false;
                            break;
                        }

                    }
                    break;

                case 6://Displaying Customer Transaction History
                    bool case6Pending = true;
                    while (case6Pending)
                    {

                        message = _customerService.IsCustomersExist(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer, _validateInputs);
                            message = _customerService.IsAccountExist(managerBankId, managerBranchId, customerAccountId);
                            if (message.Result)
                            {
                                message = _transactionService.IsTransactionsAvailable(managerBankId, managerBranchId, customerAccountId);
                                if (message.Result)
                                {
                                    List<string> transactions = _transactionService.GetTransactionHistory(managerBankId, managerBranchId, customerAccountId);
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
                                    Console.WriteLine(message.ResultMessage);
                                    case6Pending = false;
                                    break;
                                }

                            }
                            else
                            {
                                Console.WriteLine(message.ResultMessage);
                                case6Pending = false;
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            case6Pending = false;
                            break;
                        }

                    }
                    break;

                case 7://Revert Customer Transaction
                    bool case7Pending = true;
                    while (case7Pending)
                    {

                        message = _customerService.IsCustomersExist(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            string fromCustomerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer, _validateInputs);
                            message = _customerService.IsAccountExist(managerBankId, managerBranchId, fromCustomerAccountId);

                            if (message.Result)
                            {
                                message = _transactionService.IsTransactionsAvailable(managerBankId, managerBranchId, fromCustomerAccountId);
                                if (message.Result)
                                {
                                    string toCustomerBankId = _commonHelperService.GetBankId(Miscellaneous.toCustomer, _bankService, _validateInputs);
                                    string toCustomerBranchId = _commonHelperService.GetBranchId(Miscellaneous.toCustomer, _branchService, _validateInputs);
                                    string toCustomerAccountId = _commonHelperService.GetAccountId(Miscellaneous.toCustomer, _validateInputs);

                                    message = _customerService.AuthenticateToCustomerAccount(toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                                    if (message.Result)
                                    {
                                        string transactionId = _commonHelperService.ValidateTransactionIdFormat();
                                        message = _transactionService.RevertTransaction(transactionId, managerBankId, managerBranchId, fromCustomerAccountId, toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                                        Console.WriteLine(message.ResultMessage);
                                        case5Pending = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    case7Pending = false;
                                    break;
                                }

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
                            case7Pending = false;
                            break;
                        }

                    }
                    break;

                case 8://Check Customer Account Balance
                    bool case8Pending = true;
                    while (case8Pending)
                    {
                        message = _customerService.IsCustomersExist(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer, _validateInputs);
                            message = _customerService.IsAccountExist(managerBankId, managerBranchId, customerAccountId);
                            if (message.Result)
                            {
                                message = _customerService.CheckAccountBalance(managerBankId, managerBranchId, customerAccountId);
                                if (message.Result)
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    case8Pending = false;
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
                                case8Pending = false;
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            case8Pending = false;
                            break;
                        }

                    }
                    break;

                case 9:// Get ExchangeRates
                    bool case9Pending = true;
                    while (case9Pending)
                    {
                        message = _bankService.GetExchangeRates(managerBankId);

                        Dictionary<string, decimal>? exchangeRates = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(message.Data);

                        if (exchangeRates != null)
                        {
                            Console.WriteLine("Available Exchange Rates:");
                            foreach (KeyValuePair<string, decimal> rates in exchangeRates)
                            {
                                Console.WriteLine($"{rates.Key} : {rates.Value} Rupees");
                            }
                            Console.WriteLine();
                            case9Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
                        }
                    }
                    break;

                case 10:// Get TransactionCharges
                    bool case10Pending = true;
                    while (case10Pending)
                    {
                        message = _branchService.GetTransactionCharges(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            Console.WriteLine(message.Data);
                            case10Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
                        }
                    }
                    break;

                case 11://Deposit Amount in Customer Account
                    bool case11Pending = true;
                    while (case11Pending)
                    {
                        message = _customerService.IsCustomersExist(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer, _validateInputs);
                            message = _customerService.IsAccountExist(managerBankId, managerBranchId, customerAccountId);
                            if (message.Result)
                            {
                                decimal depositAmount = _commonHelperService.ValidateAmount();
                                string currencyCode = _commonHelperService.ValidateCurrency(managerBankId, _currencyService, _validateInputs);
                                message = _customerService.DepositAmount(managerBankId, managerBranchId, customerAccountId, depositAmount, currencyCode);
                                if (message.Result)
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    case11Pending = false;
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
                                case8Pending = false;
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            case8Pending = false;
                            break;
                        }
                    }
                    break;

                case 12:// Transfer Amount 
                    bool case12Pending = true;
                    while (case12Pending)
                    {
                        message = _customerService.IsCustomersExist(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            string fromCustomerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer, _validateInputs);
                            message = _customerService.IsAccountExist(managerBankId, managerBranchId, fromCustomerAccountId);
                            if (message.Result)
                            {
                                int transferMethod = _commonHelperService.ValidateTransferMethod();
                                decimal amount = _commonHelperService.ValidateAmount();
                                message = _customerService.IsAccountExist(managerBankId, managerBranchId, fromCustomerAccountId);

                                if (message.Result)
                                {
                                    bool isInvalidToCustomer = true;
                                    while (isInvalidToCustomer)
                                    {
                                        string toCustomerBankId = _commonHelperService.GetBankId(Miscellaneous.toCustomer, _bankService, _validateInputs);
                                        string toCustomerBranchId = _commonHelperService.GetBranchId(Miscellaneous.toCustomer, _branchService, _validateInputs);
                                        string toCustomerAccountId = _commonHelperService.GetAccountId(Miscellaneous.toCustomer, _validateInputs);
                                        message = _customerService.IsAccountExist(toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                                        if (message.Result)
                                        {
                                            message = _customerService.TransferAmount(managerBankId, managerBranchId, fromCustomerAccountId,
                                                toCustomerBankId, toCustomerBranchId, toCustomerAccountId, amount, transferMethod);
                                            if (message.Result)
                                            {
                                                Console.WriteLine(message.ResultMessage);
                                                isInvalidToCustomer = false;
                                                case12Pending = false;
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
                                            Console.WriteLine(message.Result);
                                            continue;
                                        }
                                    }

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
                                case8Pending = false;
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            case12Pending = false;
                            break;
                        }

                    }
                    break;

                case 13: //UpdateTransactionCharges
                    bool case13Pending = true;
                    while (case13Pending)
                    {
                        message = _branchService.GetTransactionCharges(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            Console.WriteLine("Enter RtgsSameBank Charge in %");
                            bool rtgsSameBankPending = true;
                            ushort rtgsSameBank = 0;

                            while (rtgsSameBankPending)
                            {
                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out rtgsSameBank);

                                if (isValidValue && rtgsSameBank >= 100 )
                                {
                                    Console.WriteLine($"Entered {rtgsSameBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else if (!isValidValue)
                                {
                                    rtgsSameBank = 101;
                                    rtgsSameBankPending = false;
                                    break;
                                }
                                else
                                {
                                    rtgsSameBankPending = false;
                                    break;
                                }
                            }

                            bool rtgsOtherBankPending = true;
                            ushort rtgsOtherBank =0;
                            while (rtgsOtherBankPending)
                            {
                                Console.WriteLine("Enter RtgsOtherBank Charge in %");

                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out rtgsOtherBank);
                                if (!isValidValue && rtgsOtherBank >= 100)
                                {
                                    Console.WriteLine($"Entered {rtgsOtherBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else if (!isValidValue)
                                {
                                    rtgsOtherBank = 101;
                                    rtgsOtherBankPending = false;
                                    break;
                                }
                                else
                                {
                                    rtgsOtherBankPending = false;
                                    break;
                                }
                            }

                            bool impsSameBankPending = true;
                            ushort impsSameBank = 0;
                            while (impsSameBankPending)
                            {
                                Console.WriteLine("Enter ImpsSameBank Charge in %");

                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out impsSameBank);

                                if (!isValidValue && impsSameBank >= 100 )
                                {
                                    Console.WriteLine($"Entered {impsSameBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else if (!isValidValue)
                                {
                                    impsSameBank = 101;
                                    impsSameBankPending = false;
                                    break;
                                }
                                else
                                {
                                    impsSameBankPending = false;
                                    break;
                                }
                            }

                            bool impsOtherBankPending = true;
                            ushort impsOtherBank = 0;
                            while (impsOtherBankPending)
                            {
                                Console.WriteLine("Enter ImpsOtherBank Charge in %");

                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out impsOtherBank);

                                if (!isValidValue && impsOtherBank >= 100)
                                {
                                    Console.WriteLine($"Entered {impsOtherBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else if (!isValidValue)
                                {
                                    impsOtherBank = 101;
                                    impsOtherBankPending = false;
                                    break;
                                }
                                else
                                {
                                    impsOtherBankPending = false;
                                    break;
                                }
                            }

                            message = _transactionChargeService.UpdateTransactionCharges(managerBankId, managerBranchId, rtgsSameBank, rtgsOtherBank, impsSameBank, impsOtherBank);
                            if (message.Result)
                            {
                                Console.WriteLine(message.ResultMessage);
                                case13Pending = false;
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
                            case13Pending = false;
                            break;
                        }

                    }
                    break;
                case 14: //DeleteTransactionCharges
                    bool case14Pending = true;
                    while (case14Pending)
                    {
                        message = _branchService.GetTransactionCharges(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            message = _transactionChargeService.DeleteTransactionCharges(managerBankId, managerBranchId);
                            if (message.Result)
                            {
                                Console.WriteLine(message.ResultMessage);
                            }
                            else
                            {
                                Console.WriteLine(message.ResultMessage);
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            case14Pending = false;
                            break;
                        }
                    }
                    break;

                case 15: //UpdateStaffAccount
                    bool case15Pending = true;
                    while (case15Pending)
                    {
                        message = _staffService.IsStaffExist(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            string staffAccountId = _commonHelperService.GetAccountId(Miscellaneous.staff, _validateInputs);
                            message = _staffService.IsAccountExist(managerBankId, managerBranchId, staffAccountId);
                            if (message.Result)
                            {
                                string staffDetatils = _staffService.GetStaffDetails(managerBankId, managerBranchId, staffAccountId);
                                Console.WriteLine("Staff Details:");
                                Console.WriteLine(staffDetatils);

                                string staffName = string.Empty;
                                bool invalidStaffName = true;
                                while (invalidStaffName)
                                {
                                    Console.WriteLine("Update Staff Name");
                                    staffName = Console.ReadLine() ?? string.Empty;
                                    if (staffName != string.Empty)
                                    {
                                        message = _validateInputs.ValidateNameFormat(staffName);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            continue;
                                        }
                                        else
                                        {

                                            invalidStaffName = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        invalidStaffName = false;
                                        break;
                                    }
                                }

                                string staffPassword = string.Empty;
                                bool invalidStaffPassword = true;
                                while (invalidStaffPassword)
                                {
                                    Console.WriteLine("Update Staff Password");
                                    staffPassword = Console.ReadLine() ?? string.Empty;
                                    if (staffPassword != string.Empty)
                                    {
                                        message = _validateInputs.ValidatePasswordFormat(staffPassword);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            continue;
                                        }
                                        else
                                        {

                                            invalidStaffPassword = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        invalidStaffPassword = false;
                                        break;
                                    }
                                }

                                bool invalidStaffRole = true;
                                ushort staffRole = 0;
                                while (invalidStaffRole)
                                {
                                    Console.WriteLine("Choose From Below Menu Options To Update");
                                    foreach (AccountType option in Enum.GetValues(typeof(StaffRole)))
                                    {
                                        Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                    }
                                    bool isValid = ushort.TryParse(Console.ReadLine(), out staffRole);
                                    if (!isValid || staffRole != 0 || staffRole > 5)
                                    {
                                        Console.WriteLine("please Enter a Valid Option");
                                        continue;
                                    }
                                    else
                                    {
                                        invalidStaffRole = false;
                                        break;
                                    }
                                }

                                message = _staffService.UpdateStaffAccount(managerBankId, managerBranchId, staffAccountId, staffName, staffPassword, staffRole);

                                if (message.Result)
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    case15Pending = false;
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
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            case15Pending = false;
                            break;
                        }
                    }
                    break;
                case 16: //DeleteStaffAccount
                    bool case16Pending = true;
                    while (case16Pending)
                    {
                        message = _staffService.IsStaffExist(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            string staffAccountId = _commonHelperService.GetAccountId(Miscellaneous.staff, _validateInputs);

                            message = _staffService.DeleteStaffAccount(managerBankId, managerBranchId, staffAccountId);
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
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            case16Pending = false;
                            break;
                        }
                    };
                    break;
            }
        }

    }
}

