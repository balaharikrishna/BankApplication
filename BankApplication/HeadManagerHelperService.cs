using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;
using BankApplicationServices.Services;

namespace BankApplication
{
    internal class HeadManagerHelperService : IHeadManagerHelperService
    {
        IBranchService _branchService;
        ICommonHelperService _commonHelperService;
        IManagerService _managerService;
        ICurrencyService _currencyService;
        IValidateInputs _validateInputs;
        public HeadManagerHelperService(IBranchService branchService, ICommonHelperService commonHelperService,
            IManagerService managerService,ICurrencyService currencyService,IValidateInputs validateInputs)
        {
            _branchService = branchService;
            _commonHelperService = commonHelperService;
            _managerService = managerService;
            _currencyService = currencyService;
        }
        public void SelectedOption(ushort Option, string headManagerBankId)
        {
            Message message = new Message();

            switch (Option)
            {
                case 1: //CreateBranch
                    bool branchPendingStatus = true;
                    while (branchPendingStatus)
                    {
                        string branchName = _commonHelperService.GetName(Miscellaneous.branch);
                        string branchPhoneNumber = _commonHelperService.GetPhoneNumber(Miscellaneous.branch);
                        string branchAddress = _commonHelperService.GetAddress(Miscellaneous.branch);

                        message = _branchService.CreateBranch(headManagerBankId,branchName, branchPhoneNumber, branchAddress);
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
                    break;

                case 2: //OpenManagerAcccount
                    bool OpenManagerAccountPending = true;
                    while (OpenManagerAccountPending)
                    {
                        string branchManagerName = _commonHelperService.GetName(Miscellaneous.branchManager);
                        string branchManagerPassword = _commonHelperService.GetPassword(Miscellaneous.branchManager);
                        string branchId = _commonHelperService.GetBranchId(Miscellaneous.branchManager, _branchService);

                        message = _managerService.OpenManagerAccount(headManagerBankId, branchId, branchManagerName, branchManagerPassword);
                        if (message.Result)
                        {
                            Console.WriteLine(message.ResultMessage);
                            Console.WriteLine("Enter 0 to Go Back");
                            short userInput = short.Parse(Console.ReadLine()?? string.Empty);
                            if (userInput == 0)
                            {
                                OpenManagerAccountPending = false;
                                break;
                            }

                            else if (userInput != 0)
                            {
                                Console.WriteLine($"Entered Value {userInput} is invalid please provide valid input");
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
                        }

                    }
                    break;

                case 3: //UpdateManagerAccount 
                    bool case3Pending = true;
                    while (case3Pending)
                    {
                        string managerAccountId = _commonHelperService.GetAccountId(Miscellaneous.branchManager);
                        string branchId = _commonHelperService.GetBranchId(Miscellaneous.branchManager, _branchService);
                        message = _managerService.IsAccountExist(headManagerBankId, branchId, managerAccountId);
                        if (message.Result)
                        {
                            string managerDetatils = _managerService.GetManagerDetails(headManagerBankId, branchId, managerAccountId);
                            Console.WriteLine("Manager Details:");
                            Console.WriteLine(managerDetatils);

                            string managerName = string.Empty;
                            bool invalidManagerName = true;
                            while (invalidManagerName)
                            {
                                Console.WriteLine("Update Manager Name");
                                managerName = Console.ReadLine() ?? string.Empty;
                                if (managerName != string.Empty)
                                {
                                    message = _validateInputs.ValidateNameFormat(managerName);
                                    if (!message.Result)
                                    {
                                        Console.WriteLine(message.ResultMessage);
                                        continue;
                                    }
                                    else
                                    {

                                        invalidManagerName = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    invalidManagerName = false;
                                    break;
                                }
                            }

                            string managerPassword = string.Empty;
                            bool invalidManagerPassword = true;
                            while (invalidManagerPassword)
                            {
                                Console.WriteLine("Update Staff Password");
                                managerPassword = Console.ReadLine() ?? string.Empty;
                                if (managerPassword != string.Empty)
                                {
                                    message = _validateInputs.ValidatePasswordFormat(managerPassword);
                                    if (!message.Result)
                                    {
                                        Console.WriteLine(message.ResultMessage);
                                        continue;
                                    }
                                    else
                                    {

                                        invalidManagerPassword = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    invalidManagerPassword = false;
                                    break;
                                }
                            }

                            message = _managerService.UpdateManagerAccount(headManagerBankId, branchId, managerAccountId, managerName, managerPassword);

                            if (message.Result)
                            {
                                Console.WriteLine(message.ResultMessage);
                                case3Pending = false;
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
                    break;

                case 4://DeleteManagerAccount
                    bool deleteManagerAccountPending = true;
                    while (deleteManagerAccountPending)
                    {
                        string managerAccountId = _commonHelperService.GetAccountId(Miscellaneous.branchManager);
                        string branchId = _commonHelperService.GetBranchId(Miscellaneous.branchManager, _branchService);

                        message = _managerService.DeleteManagerAccount(headManagerBankId, branchId, managerAccountId);
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
                    break;
                case 5: //AddCurrency with exchange Rates
                    bool addExchangeRatesPendingStatus = true;
                    while (addExchangeRatesPendingStatus)
                    {
                        bool CurrencyCodePending = true;
                        while (CurrencyCodePending)
                        {
                            Console.WriteLine("Please Enter currency Code");
                            string currencyCode = Console.ReadLine() ?? string.Empty;
                            message = _validateInputs.ValidateCurrencyCodeFormat(currencyCode);
                            if (message.Result)
                            {
                                message = _currencyService.ValidateCurrency(headManagerBankId, currencyCode);
                                if (!message.Result)
                                {
                                    bool exchangeRatePending = true;
                                    while (exchangeRatePending)
                                    {
                                        Console.WriteLine("Please Enter Exchange Rate as Per INR");
                                        decimal exchangeRate = 0;
                                        decimal.TryParse(Console.ReadLine(), out exchangeRate);

                                        if (exchangeRate == 0)
                                        {
                                            Console.WriteLine($"Provided '{exchangeRate}' Should Not be zero or Empty");
                                            continue;
                                        }
                                        else
                                        {
                                            message = _currencyService.AddCurrency(headManagerBankId, currencyCode, exchangeRate);
                                            if (message.Result)
                                            {
                                                Console.WriteLine(message.ResultMessage);
                                                exchangeRatePending = false;
                                                CurrencyCodePending = false;
                                                addExchangeRatesPendingStatus = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(message.Result);
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
                    break;

                case 6: //UpdateExchangeRates
                    bool UpdateExchangeRatesPendingStatus = true;
                    while (UpdateExchangeRatesPendingStatus)
                    {
                        bool CurrencyCodePending = true;
                        while (CurrencyCodePending)
                        {
                            Console.WriteLine("Please Enter currency Code");
                            string currencyCode = Console.ReadLine() ?? string.Empty;
                            message = _validateInputs.ValidateCurrencyCodeFormat(currencyCode);
                            if (message.Result)
                            {
                                message = _currencyService.ValidateCurrency(headManagerBankId, currencyCode);
                                if (message.Result)
                                {
                                    bool exchangeRatePending = true;
                                    while (exchangeRatePending)
                                    {
                                        Console.WriteLine("Please Enter Exchange Rate as Per INR");
                                        decimal exchangeRate = 0;
                                        decimal.TryParse(Console.ReadLine(), out exchangeRate);

                                        if (exchangeRate == 0)
                                        {
                                            Console.WriteLine($"Provided '{exchangeRate}' Should Not be zero or Empty");
                                            continue;
                                        }
                                        else
                                        {
                                            message = _currencyService.UpdateCurrency(headManagerBankId, currencyCode, exchangeRate);
                                            if (message.Result)
                                            {
                                                Console.WriteLine(message.ResultMessage);
                                                exchangeRatePending = false;
                                                CurrencyCodePending = false;
                                                UpdateExchangeRatesPendingStatus = false;
                                                break;

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(message.Result);
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
                    break;

                case 7: //DeleteExchangeRates
                    bool deleteExchangeRatesPendingStatus = true;
                    while (deleteExchangeRatesPendingStatus)
                    {
                        bool CurrencyCodePending = true;
                        while (CurrencyCodePending)
                        {
                            Console.WriteLine("Please Enter currency Code");
                            string currencyCode = Console.ReadLine() ?? string.Empty;
                            message = _validateInputs.ValidateCurrencyCodeFormat(currencyCode);
                            if (message.Result)
                            {
                                message = _currencyService.ValidateCurrency(headManagerBankId, currencyCode);
                                if (message.Result)
                                {
                                    message = _currencyService.DeleteCurrency(headManagerBankId, currencyCode);
                                    if (message.Result)
                                    {
                                        Console.WriteLine(message.ResultMessage);
                                        
                                        CurrencyCodePending = false;
                                        deleteExchangeRatesPendingStatus = false;
                                        break;

                                    }
                                }
                                else
                                {
                                    Console.WriteLine(message.Result);
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
                    break;
            }
        }
    }
}
