using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplication
{
    public class CustomerHelperService : ICustomerHelperService
    {
        ICustomerService _CustomerService;
        IBankService _bankService;
        IBranchService _branchService;
        ITransactionService _transactionService;
        ICommonHelperService _commonHelperService;
        IValidateInputs _validateInputs;
        public CustomerHelperService(ICustomerService customerService, IBankService bankService, IBranchService branchService,
            ITransactionService transactionService,ICommonHelperService commonHelperService,IValidateInputs validateInputs)
        {
            _CustomerService = customerService;
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
                        Message isBalanceFetchSuccesful = _CustomerService.CheckAccountBalance(bankId, branchId, accountId);
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
                            Console.WriteLine();
                        }
                        case2Pending = false;
                        break;
                    }
                    break;

                case 3: //ViewExchangeRates
                    bool case3Pending = true;
                    while (case3Pending)
                    {
                        Message message = _bankService.GetExchangeRates(bankId);
                        Dictionary<string, decimal> exchangeRates = message.Data.Cast<KeyValuePair<string, decimal>>().ToDictionary(x => x.Key, x => x.Value);

                        if (message.Result)
                        {
                            Console.WriteLine("Available Exchange Rates:");
                            foreach (KeyValuePair<string, decimal> rates in exchangeRates)
                            {
                                Console.WriteLine($"{rates.Key}:{rates.Value}Rupees");
                            }
                            case3Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.Result);
                            continue;
                        }
                    }
                    break;

                case 4: //ViewTransactionCharges
                    bool case4Pending = true;
                    while (case4Pending)
                    {
                        Message message = _branchService.GetTransactionCharges(bankId, branchId);
                        if (message.Result)
                        {
                            Console.WriteLine(message.Data);
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
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
                            Message isAmountWithdrawn = _CustomerService.WithdrawAmount(bankId, branchId, accountId, amount);
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
                        int transferMethod = _commonHelperService.ValidateTransferMethod();
                        decimal amount = _commonHelperService.ValidateAmount();

                        bool isInvalidToCustomer = true;
                        while (isInvalidToCustomer)
                        {
                            string toCustomerBankId = _commonHelperService.GetBankId(Miscellaneous.toCustomer, _bankService,_validateInputs);
                            string toCustomerBranchId = _commonHelperService.GetBranchId(Miscellaneous.toCustomer, _branchService, _validateInputs);
                            string toCustomerAccountId = _commonHelperService.GetAccountId(Miscellaneous.toCustomer, _validateInputs);
                            Message isToCustomerAccountExist = _CustomerService.AuthenticateToCustomerAccount(toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                            if (isToCustomerAccountExist.Result)
                            {
                                Message isTransferSuccessful = _CustomerService.TransferAmount(bankId, branchId, accountId, toCustomerBankId, toCustomerBranchId, toCustomerAccountId, amount, transferMethod);
                                if (isTransferSuccessful.Result)
                                {
                                    Console.WriteLine(isTransferSuccessful.ResultMessage);
                                    isInvalidToCustomer = false;
                                    case6Pending = false;
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
                    break;
                case 7: // Get Passbook
                    bool case7Pending = true;
                    while (case7Pending)
                    {
                        string passbookDetatils = _CustomerService.GetPassbook(bankId, branchId, accountId);
                        Console.WriteLine("Passbook Details:");
                        Console.WriteLine(passbookDetatils);
                        case7Pending = false;
                        break;
                    }
                    break;

            }

        }
    }
}
