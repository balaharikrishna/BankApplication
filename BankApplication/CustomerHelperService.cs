using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Newtonsoft.Json;

namespace BankApplication
{
    public class CustomerHelperService : ICustomerHelperService
    {
        ICustomerService _customerService;
        IBankService _bankService;
        IBranchService _branchService;
        ITransactionService _transactionService;
        ICommonHelperService _commonHelperService;
        IValidateInputs _validateInputs;
        public CustomerHelperService(ICustomerService customerService, IBankService bankService, IBranchService branchService,
            ITransactionService transactionService, ICommonHelperService commonHelperService, IValidateInputs validateInputs)
        {
            _customerService = customerService;
            _bankService = bankService;
            _branchService = branchService;
            _transactionService = transactionService;
            _commonHelperService = commonHelperService;
            _validateInputs = validateInputs;
        }
        Message message = new Message();
        public void SelectedOption(ushort Option, string bankId, string branchId, string accountId)
        {
            switch (Option)
            {
                case 1: //CheckAccountBalance
                    bool case1Pending = true;
                    while (case1Pending)
                    {
                        Message isBalanceFetchSuccesful = _customerService.CheckAccountBalance(bankId, branchId, accountId);
                        if (isBalanceFetchSuccesful.Result)
                        {
                            Console.WriteLine(isBalanceFetchSuccesful.ResultMessage);
                            case1Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine(isBalanceFetchSuccesful.ResultMessage);
                            continue;
                        }
                    }
                    break;

                case 2: //ViewTransactionHistory
                    bool case2Pending = true;
                    while (case2Pending)
                    {
                        List<string> transactions = _transactionService.GetTransactionHistory(bankId, branchId, accountId);
                        foreach (string transaction in transactions)
                        {
                            Console.WriteLine();
                            Console.WriteLine(transaction);
                        }
                        case2Pending = false;
                        break;
                    }
                    break;

                case 3: //ViewExchangeRates
                    bool case3Pending = true;
                    while (case3Pending)
                    {
                        message = _bankService.GetExchangeRates(bankId);

                        Dictionary<string, decimal>? exchangeRates = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(message.Data);

                        if (exchangeRates != null)
                        {
                            Console.WriteLine("Available Exchange Rates:");
                            foreach (KeyValuePair<string, decimal> rates in exchangeRates)
                            {
                                Console.WriteLine($"{rates.Key} : {rates.Value} Rupees");
                            }
                            Console.WriteLine();
                            case3Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
                        }
                    }
                    break;

                case 4: //ViewTransactionCharges
                    bool case4Pending = true;
                    while (case4Pending)
                    {
                        message = _branchService.GetTransactionCharges(bankId, branchId);
                        if (message.Result)
                        {
                            Console.WriteLine(message.Data);
                            Console.WriteLine();
                            case4Pending = false;
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

                case 5: //WithdrawAmount
                    bool case5Pending = true;
                    while (case5Pending)
                    {
                        Console.Write("Enter Amount:");
                        decimal amount = 0;
                        bool result = decimal.TryParse(Console.ReadLine(), out amount);
                        if (result)
                        {
                            Message isAmountWithdrawn = _customerService.WithdrawAmount(bankId, branchId, accountId, amount);
                            if (isAmountWithdrawn.Result)
                            {
                                Console.WriteLine(isAmountWithdrawn.ResultMessage);
                                case5Pending = false;
                                break;
                            }
                            else
                            {
                                Console.WriteLine(isAmountWithdrawn.ResultMessage);
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Amount Shouldn't Contain Any Special Charecters.");
                        }

                    }
                    break;

                case 6:  //TransferAmount
                    bool case6Pending = true;
                    while (case6Pending)
                    {
                        string fromCustomerAccountId = accountId;
                        int transferMethod = _commonHelperService.ValidateTransferMethod();
                        decimal amount = _commonHelperService.ValidateAmount();
                        message = _customerService.IsAccountExist(bankId, branchId, fromCustomerAccountId);

                        if (message.Result)
                        {
                            bool isInvalidToCustomer = true;
                            while (isInvalidToCustomer)
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
                                            message = _customerService.TransferAmount(bankId, branchId, fromCustomerAccountId,
                                                toCustomerBankId, toCustomerBranchId, toCustomerAccountId, amount, transferMethod);
                                            if (message.Result)
                                            {
                                                Console.WriteLine(message.ResultMessage);
                                                isInvalidToCustomer = false;
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
                    break;
                case 7: // Get Passbook
                    bool case7Pending = true;
                    while (case7Pending)
                    {
                        message = _customerService.IsCustomersExist(bankId, branchId);
                        if (message.Result)
                        { 
                            message = _customerService.IsAccountExist(bankId, branchId, accountId);
                            if (message.Result)
                            {
                                string passbookDetatils = _customerService.GetPassbook(bankId, branchId, accountId);
                                Console.WriteLine("Passbook Details:");
                                Console.WriteLine(passbookDetatils);
                                case7Pending = false;
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
                    break;
            }
        }
    }
}
