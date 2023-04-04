using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationServices.IServices;
using Newtonsoft.Json;

namespace BankApplication
{
    public class CustomerHelperService : ICustomerHelperService
    {
        readonly ICustomerService _customerService;
        readonly IBankService _bankService;
        readonly IBranchService _branchService;
        readonly ITransactionService _transactionService;
        readonly ICommonHelperService _commonHelperService;
        readonly IValidateInputs _validateInputs;
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

        public void SelectedOption(ushort Option, string bankId, string branchId, string accountId)
        {
            switch (Option)
            {
                case 1: //CheckAccountBalance
                    while (true)
                    {
                        Message isBalanceFetchSuccesful = _customerService.CheckAccountBalance(bankId, branchId, accountId);
                        if (isBalanceFetchSuccesful.Result)
                        {
                            Console.WriteLine(isBalanceFetchSuccesful.ResultMessage);
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
                    while (true)
                    {
                        List<string> transactions = _transactionService.GetTransactionHistory(bankId, branchId, accountId);
                        foreach (string transaction in transactions)
                        {
                            Console.WriteLine();
                            Console.WriteLine(transaction);
                        }
                        break;
                    }
                    break;

                case 3: //ViewExchangeRates
                    while (true)
                    {
                        Message message = new();
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
                    while (true)
                    {
                        Message message = new();
                        message = _branchService.GetTransactionCharges(bankId, branchId);
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

                case 5: //WithdrawAmount
                    while (true)
                    {
                        Console.Write("Enter Amount:");
                        bool result = decimal.TryParse(Console.ReadLine(), out decimal amount);
                        if (result)
                        {
                            Message isAmountWithdrawn = _customerService.WithdrawAmount(bankId, branchId, accountId, amount);
                            if (isAmountWithdrawn.Result)
                            {
                                Console.WriteLine(isAmountWithdrawn.ResultMessage);
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
                    while (true)
                    {
                        Message message = new();
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


                case 7: // Get Passbook
                    while (true)
                    {
                        Message message = new();
                        message = _customerService.IsCustomersExist(bankId, branchId);
                        if (message.Result)
                        {
                            message = _customerService.IsAccountExist(bankId, branchId, accountId);
                            if (message.Result)
                            {
                                string passbookDetatils = _customerService.GetPassbook(bankId, branchId, accountId);
                                Console.WriteLine("Passbook Details:");
                                Console.WriteLine(passbookDetatils);
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
                    break;
            }
        }
    }
}
