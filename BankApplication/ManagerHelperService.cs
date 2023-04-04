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
        readonly IBankService _bankService;
        readonly IBranchService _branchService;
        readonly IStaffService _staffService;
        readonly ICustomerService _customerService;
        readonly ICommonHelperService _commonHelperService;
        readonly ITransactionChargeService _transactionChargeService;
        readonly IValidateInputs _validateInputs;
        readonly ITransactionService _transactionService;
        readonly ICurrencyService _currencyService;
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
                    while (true)
                    {
                        string staffName = _commonHelperService.GetName(Miscellaneous.staff, _validateInputs);
                        string staffPassword = _commonHelperService.GetPassword(Miscellaneous.staff, _validateInputs);
                        Console.WriteLine("Choose Staff Roles from Below:");
                        foreach (StaffRole option in Enum.GetValues(typeof(StaffRole)))
                        {
                            Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                        }

                        StaffRole staffRole = 0;
                        while (true)
                        {
                            Console.WriteLine("Enter Staff Role:");

                            bool isValid = StaffRole.TryParse(Console.ReadLine(), out staffRole);
                            if (!isValid && staffRole == 0)
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
                    while (true)
                    {
                        message = _branchService.GetTransactionCharges(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            Console.WriteLine("Charges Already Available");
                            Console.WriteLine();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Enter RtgsSameBank Charge in %");
                            ushort rtgsSameBank;
                            while (true)
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
                                    break;
                                }
                            }

                            ushort rtgsOtherBank;
                            while (true)
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
                                    break;
                                }
                            }

                            ushort impsSameBank;
                            while (true)
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
                                    break;
                                }
                            }

                            ushort impsOtherBank;
                            while (true)
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
                                    break;
                                }
                            }

                            message = _transactionChargeService.AddTransactionCharges(managerBankId, managerBranchId, rtgsSameBank, rtgsOtherBank, impsSameBank, impsOtherBank);
                            if (message.Result)
                            {
                                Console.WriteLine(message.ResultMessage);
                                Console.WriteLine();
                                break;
                            }
                            else
                            {
                                Console.WriteLine(message.ResultMessage);
                                continue;
                            }
                        }
                    }
                    break;

                case 3: //OpenCustomerAccount

                    while (true)
                    {
                        string customerName = _commonHelperService.GetName(Miscellaneous.customer, _validateInputs);
                        string customerPassword = _commonHelperService.GetPassword(Miscellaneous.customer, _validateInputs);
                        string customerPhoneNumber = _commonHelperService.GetPhoneNumber(Miscellaneous.customer, _validateInputs);
                        string customerEmailId = _commonHelperService.GetEmailId(Miscellaneous.customer, _validateInputs);
                        int customerAccountType = _commonHelperService.GetAccountType(Miscellaneous.customer, _validateInputs);
                        string customerAddress = _commonHelperService.GetAddress(Miscellaneous.customer, _validateInputs);
                        string customerDOB = _commonHelperService.GetDateOfBirth(Miscellaneous.customer, _validateInputs);
                        int customerGender = _commonHelperService.GetGender(Miscellaneous.customer, _validateInputs);

                        message = _customerService.OpenCustomerAccount(managerBankId,
                        managerBranchId, customerName, customerPassword, customerPhoneNumber, customerEmailId,
                        customerAccountType, customerAddress, customerDOB, customerGender);
                        if (message.Result)
                        {
                            Console.WriteLine(message.ResultMessage);
                            Console.WriteLine();
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            Console.WriteLine();
                            continue;
                        }
                    };
                    break;

                case 4: //UpdateCustomerAccount
                    while (true)
                    {
                        message = _customerService.IsCustomersExist(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer, _validateInputs);
                            message = _customerService.IsAccountExist(managerBankId, managerBranchId, customerAccountId);
                            if (message.Result)
                            {
                                string passbookDetatils = _customerService.GetPassbook(managerBankId, managerBranchId, customerAccountId);
                                Console.WriteLine("Passbook Details:");
                                Console.WriteLine(passbookDetatils);

                                string customerName;
                                while (true)
                                {
                                    Console.WriteLine("Update Customer Name");
                                    customerName = Console.ReadLine() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(customerName))
                                    {
                                        message = _validateInputs.ValidateNameFormat(customerName);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            Console.WriteLine();
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                string customerPassword;
                                while (true)
                                {
                                    Console.WriteLine("Update Customer Password");
                                    customerPassword = Console.ReadLine() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(customerPassword))
                                    {
                                        message = _validateInputs.ValidatePasswordFormat(customerPassword);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            Console.WriteLine();
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                string customerPhoneNumber;
                                while (true)
                                {
                                    Console.WriteLine("Update Customer Phone Number");
                                    customerPhoneNumber = Console.ReadLine() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(customerPhoneNumber))
                                    {
                                        message = _validateInputs.ValidatePhoneNumberFormat(customerPhoneNumber);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            Console.WriteLine();
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                string customerEmailId;
                                while (true)
                                {
                                    Console.WriteLine("Update Customer Email Id");
                                    customerEmailId = Console.ReadLine() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(customerEmailId))
                                    {
                                        message = _validateInputs.ValidateEmailIdFormat(customerEmailId);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            Console.WriteLine();
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                int customerAccountType;
                                while (true)
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
                                            Console.WriteLine();
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                string customerAddress;
                                while (true)
                                {
                                    Console.WriteLine("Update Customer Address");
                                    customerAddress = Console.ReadLine() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(customerAddress))
                                    {
                                        message = _validateInputs.ValidateAddressFormat(customerAddress);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            Console.WriteLine();
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                string customerDOB;
                                while (true)
                                {
                                    Console.WriteLine("Update Customer Date Of Birth");
                                    customerDOB = Console.ReadLine() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(customerDOB))
                                    {
                                        message = _validateInputs.ValidateDateOfBirthFormat(customerDOB);
                                        if (message.Result == false)
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            Console.WriteLine();
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }

                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                int customerGender;
                                while (true)
                                {
                                    Console.WriteLine("Choose From Below Menu Options To Update");
                                    foreach (Gender option in Enum.GetValues(typeof(Gender)))
                                    {
                                        Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                    }
                                    bool isValid = int.TryParse(Console.ReadLine(), out customerGender);
                                    if (isValid && customerGender != 0)
                                    {
                                        message = _validateInputs.ValidateGenderFormat(customerGender);
                                        if (message.Result == false)
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            Console.WriteLine();
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                message = _customerService.UpdateCustomerAccount(managerBankId, managerBranchId, customerAccountId, customerName, customerPassword, customerPhoneNumber,
                                    customerEmailId, customerAccountType, customerAddress, customerDOB, customerGender);
                                if (message.Result)
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    Console.WriteLine();
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    Console.WriteLine();
                                    continue;
                                }
                            }
                            else
                            {
                                Console.WriteLine(message.ResultMessage);
                                Console.WriteLine();
                                continue;
                            }

                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            Console.WriteLine();
                            break;
                        }
                    }
                    break;

                case 5://DeleteCustomerAccount
                    while (true)
                    {
                        message = _customerService.IsCustomersExist(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer, _validateInputs);
                            message = _customerService.DeleteCustomerAccount(managerBankId, managerBranchId, customerAccountId);
                            if (message.Result)
                            {
                                Console.WriteLine(message.ResultMessage);
                                Console.WriteLine();
                                break;
                            }
                            else
                            {
                                Console.WriteLine(message.ResultMessage);
                                Console.WriteLine();
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            Console.WriteLine();
                            break;
                        }

                    }
                    break;

                case 6://Displaying Customer Transaction History
                    while (true)
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
                                        Console.WriteLine();
                                        Console.WriteLine(transaction);
                                        Console.WriteLine();
                                    }
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine(message.ResultMessage);
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine(message.ResultMessage);
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            break;
                        }
                    }
                    break;

                case 7://Revert Customer Transaction
                    while (true)
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
                                    message = _bankService.AuthenticateBankId(toCustomerBankId);
                                    if (message.Result)
                                    {
                                        string toCustomerBranchId = _commonHelperService.GetBranchId(Miscellaneous.toCustomer, _branchService, _validateInputs);
                                        message = _branchService.AuthenticateBranchId(toCustomerBankId, toCustomerBranchId);
                                        if (message.Result)
                                        {
                                            string toCustomerAccountId = _commonHelperService.GetAccountId(Miscellaneous.toCustomer, _validateInputs);

                                            message = _customerService.AuthenticateToCustomerAccount(toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                                            if (message.Result)
                                            {
                                                string transactionId = _commonHelperService.ValidateTransactionIdFormat();
                                                message = _transactionService.RevertTransaction(transactionId, managerBankId, managerBranchId, fromCustomerAccountId, toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                                                Console.WriteLine(message.ResultMessage);
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
                                        continue;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(message.ResultMessage);
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
                            break;
                        }
                    }
                    break;

                case 8://Check Customer Account Balance
                    while (true)
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
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            break;
                        }
                    }
                    break;

                case 9:// Get ExchangeRates
                    while (true)
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
                    while (true)
                    {
                        message = _branchService.GetTransactionCharges(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            Console.WriteLine(message.Data);
                            Console.WriteLine();
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            Console.WriteLine();
                            break;
                        }
                    }
                    break;

                case 11://Deposit Amount in Customer Account
                    while (true)
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
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            break;
                        }
                    }
                    break;

                case 12:// Transfer Amount 
                    while (true)
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
                                    while (true)
                                    {
                                        string toCustomerBankId = _commonHelperService.GetBankId(Miscellaneous.toCustomer, _bankService, _validateInputs);
                                        message = _bankService.AuthenticateBankId(toCustomerBankId);
                                        if (message.Result)
                                        {
                                            string toCustomerBranchId = _commonHelperService.GetBranchId(Miscellaneous.toCustomer, _branchService, _validateInputs);
                                            message = _branchService.AuthenticateBranchId(toCustomerBankId, toCustomerBranchId);
                                            if (message.Result)
                                            {
                                                string toCustomerAccountId = _commonHelperService.GetAccountId(Miscellaneous.toCustomer, _validateInputs);
                                                message = _customerService.IsAccountExist(toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                                                if (message.Result)
                                                {
                                                    message = _customerService.TransferAmount(managerBankId, managerBranchId, fromCustomerAccountId,
                                                        toCustomerBankId, toCustomerBranchId, toCustomerAccountId, amount, transferMethod);
                                                    if (message.Result)
                                                    {
                                                        Console.WriteLine(message.ResultMessage);
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
                                                continue;
                                            }

                                        }
                                        else
                                        {
                                            Console.WriteLine(message.ResultMessage);
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
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            break;
                        }
                    }
                    break;

                case 13: //UpdateTransactionCharges
                    while (true)
                    {
                        message = _branchService.GetTransactionCharges(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            Console.WriteLine("Enter RtgsSameBank Charge in %");
                            ushort rtgsSameBank;

                            while (true)
                            {
                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out rtgsSameBank);

                                if (isValidValue && rtgsSameBank >= 100)
                                {
                                    Console.WriteLine($"Entered {rtgsSameBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else if (!isValidValue)
                                {
                                    rtgsSameBank = 101;
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            ushort rtgsOtherBank;
                            while (true)
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
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            ushort impsSameBank;
                            while (true)
                            {
                                Console.WriteLine("Enter ImpsSameBank Charge in %");

                                bool isValidValue = ushort.TryParse(Console.ReadLine(), out impsSameBank);

                                if (!isValidValue && impsSameBank >= 100)
                                {
                                    Console.WriteLine($"Entered {impsSameBank} Value Should not be Greater Than 100%");
                                    continue;
                                }
                                else if (!isValidValue)
                                {
                                    impsSameBank = 101;
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            ushort impsOtherBank;
                            while (true)
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
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            message = _transactionChargeService.UpdateTransactionCharges(managerBankId, managerBranchId, rtgsSameBank, rtgsOtherBank, impsSameBank, impsOtherBank);
                            if (message.Result)
                            {
                                Console.WriteLine(message.ResultMessage);
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
                            break;
                        }

                    }
                    break;
                case 14: //DeleteTransactionCharges
                    while (true)
                    {
                        message = _branchService.GetTransactionCharges(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            message = _transactionChargeService.DeleteTransactionCharges(managerBankId, managerBranchId);
                            if (message.Result)
                            {
                                Console.WriteLine(message.ResultMessage);
                                break;
                            }
                            else
                            {
                                Console.WriteLine(message.ResultMessage);
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            break;
                        }
                    }
                    break;

                case 15: //UpdateStaffAccount
                    while (true)
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

                                string staffName;
                                while (true)
                                {
                                    Console.WriteLine("Update Staff Name");
                                    staffName = Console.ReadLine() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(staffName))
                                    {
                                        message = _validateInputs.ValidateNameFormat(staffName);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                string staffPassword;
                                while (true)
                                {
                                    Console.WriteLine("Update Staff Password");
                                    staffPassword = Console.ReadLine() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(staffPassword))
                                    {
                                        message = _validateInputs.ValidatePasswordFormat(staffPassword);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                ushort staffRole;
                                while (true)
                                {
                                    Console.WriteLine("Choose From Below Menu Options To Update");
                                    foreach (StaffRole option in Enum.GetValues(typeof(StaffRole)))
                                    {
                                        Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                    }
                                    bool isValid = ushort.TryParse(Console.ReadLine(), out staffRole);
                                    if (!isValid || staffRole != 0 && staffRole > 5)
                                    {
                                        Console.WriteLine("please Enter a Valid Option");
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                message = _staffService.UpdateStaffAccount(managerBankId, managerBranchId, staffAccountId, staffName, staffPassword, staffRole);

                                if (message.Result)
                                {
                                    Console.WriteLine(message.ResultMessage);
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
                            break;
                        }
                    }
                    break;
                case 16: //DeleteStaffAccount
                    while (true)
                    {
                        message = _staffService.IsStaffExist(managerBankId, managerBranchId);
                        if (message.Result)
                        {
                            string staffAccountId = _commonHelperService.GetAccountId(Miscellaneous.staff, _validateInputs);

                            message = _staffService.DeleteStaffAccount(managerBankId, managerBranchId, staffAccountId);
                            if (message.Result)
                            {
                                Console.WriteLine(message.ResultMessage);
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
                            break;
                        }
                    };
                    break;
            }
        }

    }
}

