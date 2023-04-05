using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;

namespace BankApplication
{
    public class StaffHelperService : IStaffHelperService
    {
        readonly ICustomerService _customerService;
        readonly IBankService _bankService;
        readonly IBranchService _branchService;
        readonly ICommonHelperService _commonHelperService;
        readonly IValidateInputs _validateInputs;
        readonly ITransactionService _transactionService;
        readonly ICurrencyService _currencyService;
        public StaffHelperService(IBankService bankService, IBranchService branchService, ICustomerService customerService,
        ICommonHelperService commonHelperService, IValidateInputs validateInputs, ITransactionService transactionService,
        ICurrencyService currencyService)
        {
            _bankService = bankService;
            _branchService = branchService;
            _commonHelperService = commonHelperService;
            _transactionService = transactionService;
            _currencyService = currencyService;
            _customerService = customerService;
            _validateInputs = validateInputs;
        }
        Message message = new Message();
        public void SelectedOption(ushort Option, string staffBankId, string staffBranchId)
        {
            switch (Option)
            {
                case 1: //OpenCustomerAccount
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

                        message = _customerService.OpenCustomerAccount(staffBankId, staffBranchId,
                            customerName, customerPassword, customerPhoneNumber, customerEmailId, customerAccountType,
                            customerAddress, customerDOB, customerGender);

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

                case 2: //UpdateCustomerAccount
                    while (true)
                    {
                        message = _customerService.IsCustomersExist(staffBankId, staffBranchId);
                        if (message.Result)
                        {
                            string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer, _validateInputs);
                            message = _customerService.IsAccountExist(staffBankId, staffBranchId, customerAccountId);
                            if (message.Result)
                            {
                                string passbookDetatils = _customerService.GetPassbook(staffBankId, staffBranchId, customerAccountId);
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
                                        Message isValidPassword = _validateInputs.ValidatePhoneNumberFormat(customerPhoneNumber);
                                        if (isValidPassword.Result == false)
                                        {
                                            Console.WriteLine(isValidPassword.ResultMessage);
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

                                message = _customerService.UpdateCustomerAccount(staffBankId, staffBranchId, customerAccountId, customerName, customerPassword, customerPhoneNumber,
                                    customerEmailId, customerAccountType, customerAddress, customerDOB, customerGender);
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
                                Console.WriteLine("Account Id:{customerAccountId} is not a Valid Account,Please Enter Correct Account Id");
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

                case 3://DeleteCustomerAccount
                    while (true)
                    {
                        message = _customerService.IsCustomersExist(staffBankId, staffBranchId);
                        if (message.Result)
                        {
                            string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer, _validateInputs);
                            Message isCustomerAccountDeleted = _customerService.DeleteCustomerAccount(staffBankId, staffBranchId, customerAccountId);
                            if (isCustomerAccountDeleted.Result)
                            {
                                Console.WriteLine(isCustomerAccountDeleted.ResultMessage);
                                Console.WriteLine();
                                break;

                            }
                            else
                            {
                                Console.WriteLine(isCustomerAccountDeleted.ResultMessage);
                                Console.WriteLine();
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

                case 4://Displaying Customer Transaction History
                    _commonHelperService.GetTransactoinHistory(staffBankId, staffBranchId, _customerService, _validateInputs,
                        _transactionService, Miscellaneous.staff, null);
                    break;

                case 5://Revert Customer Transaction
                    while (true)
                    {
                        message = _customerService.IsCustomersExist(staffBankId, staffBranchId);
                        if (message.Result)
                        {
                            string fromCustomerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer, _validateInputs);

                            message = _customerService.IsAccountExist(staffBankId, staffBranchId, fromCustomerAccountId);

                            if (message.Result)
                            {
                                message = _transactionService.IsTransactionsAvailable(staffBankId, staffBranchId, fromCustomerAccountId);
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
                                                message = _transactionService.RevertTransaction(transactionId, staffBankId, staffBranchId, fromCustomerAccountId, toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
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

                case 6://Check Customer Account Balance
                    _commonHelperService.GetCustomerAccountBalance(staffBankId, staffBranchId, _customerService, _validateInputs,
                        Miscellaneous.staff, null);
                    break;

                case 7:// Get ExchangeRates
                    _commonHelperService.GetExchangeRates(staffBankId, _bankService);
                    break;

                case 8:// Get TransactionCharges
                    _commonHelperService.GetTransactionCharges(staffBankId, staffBranchId, _branchService);
                    break;

                case 9://Deposit Amount in Customer Account
                    while (true)
                    {
                        message = _customerService.IsCustomersExist(staffBankId, staffBranchId);
                        if (message.Result)
                        {
                            string customerAccountId = _commonHelperService.GetAccountId(Miscellaneous.customer, _validateInputs);
                            message = _customerService.IsAccountExist(staffBankId, staffBranchId, customerAccountId);
                            if (message.Result)
                            {
                                decimal depositAmount = _commonHelperService.ValidateAmount();
                                string currencyCode = _commonHelperService.ValidateCurrency(staffBankId, _currencyService, _validateInputs);
                                message = _customerService.DepositAmount(staffBankId, staffBranchId, customerAccountId, depositAmount, currencyCode);
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

                case 10:// Transfer Amount 
                    _commonHelperService.TransferAmount(staffBankId, staffBranchId, _branchService, _bankService, _validateInputs,
                        _customerService, Miscellaneous.staff, null);
                    break;
            }
        }
    }
}
