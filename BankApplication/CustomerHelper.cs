using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplication
{
    public class CustomerHelper 
    {
        ICustomerService _CustomerService;
        IBankService _bankService;
        IBranchService _branchService;
        public CustomerHelper(ICustomerService branchCustomerService, IBankService bankService
            , IBranchService branchService)
        {
            _CustomerService = branchCustomerService;
            _bankService = bankService;
            _branchService = branchService;
        }
        Message message = new Message();
        public void SelectedOption(ushort Option, string bankId, string branchId)
        {

            switch (Option)
            {
                case 1: //CheckAccountBalance
                    bool case1Pending = true;
                    while (case1Pending)
                    {
                        Message isBalanceFetchSuccesful = _CustomerService.CheckAccountBalance();
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
                        List<string> transactions = _branchCustomerService.GetTransactionHistory();
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
                        Dictionary<string, decimal> exchangeRates = _bankService.GetExchangeRates(bankId);
                        if (exchangeRates != null)
                        {
                            Console.WriteLine("Available Exchange Rates:");
                            foreach (KeyValuePair<string, decimal> rates in exchangeRates)
                            {
                                Console.WriteLine($"{rates.Key}:{rates.Value}₹");
                            }
                            case3Pending = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"ExchangeRates Not Available for {bankId}");
                            continue;
                        }
                    }
                    break;

                case 4: //ViewTransactionCharges
                    bool case4Pending = true;
                    while (case4Pending)
                    {
                        string transactionCharges = _branchService.GetTransactionCharges(bankId, branchId);
                        if (transactionCharges != null)
                        {
                            Console.WriteLine(transactionCharges);
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Transaction Charges not Available for {branchId}");
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
                            Message isAmountWithdrawn = _branchCustomerService.WithdrawAmount(amount);
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
                        int transferMethod = CommonHelper.ValidateTransferMethod();
                        decimal amount = CommonHelper.ValidateAmount();

                        bool isInvalidToCustomer = true;
                        while (isInvalidToCustomer)
                        {
                            string toCustomerBankId = CommonHelper.GetBankId(Miscellaneous.toCustomer);
                            string toCustomerBranchId = CommonHelper.GetBranchId(Miscellaneous.toCustomer);
                            string toCustomerAccountId = CommonHelper.GetAccountId(Miscellaneous.toCustomer);
                            bool isToCustomerAccountExist = _branchCustomerService.ValidateToCustomerAccount(toCustomerBankId, toCustomerBranchId, toCustomerAccountId);
                            if (isToCustomerAccountExist)   
                            {
                                Message isTransferSuccessful = _branchCustomerService.TransferAmount(toCustomerBankId, toCustomerBranchId, toCustomerAccountId, amount, transferMethod);
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
                        string passbookDetatils = _branchCustomerService.GetPassbook();
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
