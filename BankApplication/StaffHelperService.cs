using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;
using BankApplicationServices.Services;

namespace BankApplication
{
    public class StaffHelperService : IStaffHelperService
    {
        IStaffService _staffService;
        ICustomerService _customerService;
        IBankService _bankService;
        IBranchService _branchService;
        ICommonHelperService _commonHelperService;
        IValidateInputs _validateInputs;
        ITransactionService _transactionService;
        ICurrencyService _currencyService;
        public StaffHelperService(IBankService bankService, IBranchService branchService,
        IStaffService staffService, ICustomerService customerService, ICommonHelperService commonHelperService,
        IValidateInputs validateInputs, ITransactionService transactionService, ICurrencyService currencyService)
        {
            _bankService = bankService;
            _branchService = branchService;
            _commonHelperService = commonHelperService;
            _transactionService = transactionService;
            _currencyService = currencyService;
            _staffService = staffService;
            _customerService = customerService;
            _validateInputs = validateInputs;
        }
        Message message = new Message();
        public void SelectedOption(ushort Option, string staffBankId, string staffBranchId)
        {
            switch (Option)
            {

                case 1: //OpenCustomerAccount

                    bool case1Pending = true;
                    while (case1Pending)
                    {
                        string customerName = _commonHelperService.GetName(Miscellaneous.customer);
                        string customerPassword = _commonHelperService.GetPassword(Miscellaneous.customer);
                        string customerPhoneNumber = _commonHelperService.GetPhoneNumber(Miscellaneous.customer);
                        string customerEmailId = _commonHelperService.GetEmailId(Miscellaneous.customer);
                        int customerAccountType = _commonHelperService.GetAccountType(Miscellaneous.customer);
                        string customerAddress = _commonHelperService.GetAddress(Miscellaneous.customer);
                        string customerDOB = _commonHelperService.GetDateOfBirth(Miscellaneous.customer);
                        int customerGender = _commonHelperService.GetGender(Miscellaneous.customer);

                        message = _customerService.OpenCustomerAccount(staffBankId, staffBranchId,
                            customerName, customerPassword, customerPhoneNumber, customerEmailId, customerAccountType,
                            customerAddress, customerDOB, customerGender);

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

                case 2: //UpdateCustomerAccount
                    bool case2Pending = true;
                    while (case2Pending)
                    {
                        string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer);
                        message = _customerService.IsAccountExist(staffBankId, staffBranchId, customerAccountId);
                        if (message.Result)
                        {
                            string passbookDetatils = _customerService.GetPassbook(staffBankId, staffBranchId, customerAccountId);
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

                            string customerPassword = string.Empty;
                            bool invalidCustomerPassword = true;
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

                           
                            string customerPhoneNumber = string.Empty;
                            bool invalidCustomerPhoneNumber = true;
                            while (invalidCustomerPhoneNumber)
                            {
                                Console.WriteLine("Update Customer Phone Number");
                                customerPhoneNumber = Console.ReadLine() ?? string.Empty;
                                if (customerPhoneNumber != string.Empty)
                                {
                                    Message isValidPassword = _validateInputs.ValidatePhoneNumberFormat(customerPhoneNumber);
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


                            string customerEmailId = string.Empty;
                            bool invalidCustomerEmailId = true;
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

                            int customerAccountType=0;
                            bool invalidCustomerAccountType = true;
                            while (invalidCustomerAccountType)
                            {
                                Console.WriteLine("Choose From Below Menu Options To Update");
                                foreach (AccountType option in Enum.GetValues(typeof(AccountType)))
                                {
                                    Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                }
                                int.TryParse(Console.ReadLine(), out customerAccountType);
                                if (customerAccountType != 0)
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

                            string customerAddress = string.Empty;
                            bool invalidCustomerAddress = true;
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

                            string customerDOB =  string.Empty;
                            bool invalidCustomerDob = true;
                            while (invalidCustomerDob)
                            {
                                Console.WriteLine("Update Customer Date Of Birth");
                                customerDOB = Console.ReadLine() ?? string.Empty;
                                if (customerDOB != string.Empty)
                                {
                                    message = _validateInputs.ValidateDateOfBirthFormat(customerDOB);
                                    if (!message.Result)
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

                            
                            int customerGender=0;
                            bool invalidCustomerGender = true;
                            while (invalidCustomerGender)
                            {
                                Console.WriteLine("Choose From Below Menu Options To Update");
                                foreach (Gender option in Enum.GetValues(typeof(Gender)))
                                {
                                    Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                }
                                int.TryParse(Console.ReadLine(), out customerGender);
                                if (customerGender != 0)
                                {
                                    message = _validateInputs.ValidateGenderFormat(customerGender);
                                    if (!message.Result)
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


                            message = _customerService.UpdateCustomerAccount(staffBankId,staffBranchId,customerAccountId, customerName, customerPassword, customerPhoneNumber,
                                customerEmailId, customerAccountType, customerAddress, customerDOB, customerGender);
                            if (message.Result)
                            {
                                Console.WriteLine(message.ResultMessage);
                                case2Pending = false;
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
                            Console.WriteLine("Account Id:{customerAccountId} is not a Valid Account,Please Enter Correct Account Id");
                            continue;
                        }

                    }
                    break;

                case 3://DeleteCustomerAccount
                    bool case3Pending = true;
                    while (case3Pending)
                    {
                        string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer);
                        Message isCustomerAccountDeleted = _customerService.DeleteCustomerAccount(staffBankId, staffBranchId, customerAccountId);
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
                        string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer);
                        message = _customerService.IsAccountExist(staffBankId, staffBranchId, customerAccountId);
                        if (message.Result)
                        {
                            List<string> transactions = _transactionService.GetTransactionHistory(staffBankId, staffBranchId, customerAccountId);
                            foreach (string transaction in transactions)
                            {
                                Console.WriteLine();
                                Console.WriteLine(transaction);
                                Console.WriteLine();
                            }
                            case4Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            case4Pending = false;
                            break;
                        }

                    }
                    break;

                case 5://Revert Customer Transaction
                    bool case5Pending = true;
                    while (case5Pending)
                    {
                        string fromCustomerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer);

                        message = _customerService.IsAccountExist(staffBankId, staffBranchId, fromCustomerAccountId);
                        if (message.Result)
                        {
                            string toCustomerBankId = _commonHelperService.GetBankId(Miscellaneous.toCustomer,_bankService);
                            string toCustomerBranchId = _commonHelperService.GetBranchId(Miscellaneous.toCustomer,_branchService);
                            string toCustomerAccountId = _commonHelperService.GetAccountId(Miscellaneous.toCustomer);

                            message = _customerService.AuthenticateToCustomerAccount(toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                            if (message.Result)
                            {
                                string transactionId = _commonHelperService.ValidateTransactionIdFormat();
                                message = _transactionService.RevertTransaction(transactionId, staffBankId, staffBranchId, fromCustomerAccountId, toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
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
                        string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer);
                        message = _customerService.IsAccountExist(staffBankId, staffBranchId, customerAccountId);
                        if (message.Result)
                        {
                            message = _customerService.CheckAccountBalance(staffBankId, staffBranchId, customerAccountId);
                            if (message.Result)
                            {
                                Console.WriteLine(message.ResultMessage);
                                case6Pending = false;
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
                            case4Pending = false;
                            break;
                        }

                    }
                    break;

                case 7:// Get ExchangeRates
                    bool case7Pending = true;
                    while (case7Pending)
                    {
                        message = _bankService.GetExchangeRates(staffBankId);
                        Dictionary<string, decimal> exchangeRates = message.Data.Cast<KeyValuePair<string, decimal>>().ToDictionary(x => x.Key, x => x.Value);
                        if (exchangeRates != null)
                        {
                            Console.WriteLine("Available Exchange Rates:");
                            foreach (KeyValuePair<string, decimal> rates in exchangeRates)
                            {
                                Console.WriteLine($"{rates.Key}:{rates.Value}Rupees");
                            }
                            case7Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
                        }
                    }
                    break;

                case 8:// Get TransactionCharges
                    bool case8Pending = true;
                    while (case8Pending)
                    {
                        message = _branchService.GetTransactionCharges(staffBankId, staffBranchId);
                        if (message.Result)
                        {
                            Console.WriteLine(message.Data);
                            case8Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
                        }
                    }
                    break;

                case 9://Deposit Amount in Customer Account
                    bool case9Pending = true;
                    while (case9Pending)
                    {
                        string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer);
                        decimal depositAmount = _commonHelperService.ValidateAmount();
                        string currencyCode = _commonHelperService.ValidateCurrency(staffBankId,_currencyService);
                        Message isDepositSuccesful = _customerService.DepositAmount(staffBankId,staffBranchId,customerAccountId, depositAmount, currencyCode);
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
                        string fromCustomerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer);

                        int transferMethod = _commonHelperService.ValidateTransferMethod();
                        decimal amount = _commonHelperService.ValidateAmount();
                        message = _customerService.IsAccountExist(staffBankId, staffBranchId, fromCustomerAccountId);

                        if (message.Result)
                        {
                            bool isInvalidToCustomer = true;
                            while (isInvalidToCustomer)
                            {
                                string toCustomerBankId = _commonHelperService.GetBankId(Miscellaneous.toCustomer, _bankService);
                                string toCustomerBranchId = _commonHelperService.GetBranchId(Miscellaneous.toCustomer, _branchService);
                                string toCustomerAccountId = _commonHelperService.GetAccountId(Miscellaneous.toCustomer);
                                message = _customerService.IsAccountExist(toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                                if (message.Result)
                                {
                                    message = _customerService.TransferAmount(staffBankId, staffBranchId, fromCustomerAccountId,
                                        toCustomerBankId, toCustomerBranchId, toCustomerAccountId, amount, transferMethod);
                                    if (message.Result)
                                    {
                                        Console.WriteLine(message.ResultMessage);
                                        isInvalidToCustomer = false;
                                        case10Pending = false;
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
                    break;
            }

        }
    }
}
