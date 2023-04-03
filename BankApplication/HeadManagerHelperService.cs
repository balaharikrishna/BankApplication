using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplication
{
    internal class HeadManagerHelperService : IHeadManagerHelperService
    {
        IBranchService _branchService;
        ICommonHelperService _commonHelperService;
        IManagerService _managerService;
        ICurrencyService _currencyService;
        IValidateInputs _validateInputs;
        IBankService _bankService;
        public HeadManagerHelperService(IBranchService branchService, ICommonHelperService commonHelperService,
            IManagerService managerService, ICurrencyService currencyService, IValidateInputs validateInputs, IBankService bankService)
        {
            _branchService = branchService;
            _commonHelperService = commonHelperService;
            _managerService = managerService;
            _currencyService = currencyService;
            _validateInputs = validateInputs;
            _bankService = bankService;
        }
        public void SelectedOption(ushort Option, string headManagerBankId)
        {
            switch (Option)
            {
                case 1: //CreateBranch
                    bool branchPendingStatus = true;
                    while (branchPendingStatus)
                    {
                        string branchName = _commonHelperService.GetName(Miscellaneous.branch, _validateInputs);
                        string branchPhoneNumber = _commonHelperService.GetPhoneNumber(Miscellaneous.branch, _validateInputs);
                        string branchAddress = _commonHelperService.GetAddress(Miscellaneous.branch, _validateInputs);

                        Message message = _branchService.CreateBranch(headManagerBankId, branchName, branchPhoneNumber, branchAddress);
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
                        Message message = _branchService.IsBranchesExist(headManagerBankId);
                        if (message.Result)
                        {
                            string branchId = _commonHelperService.GetBranchId(Miscellaneous.branchManager, _branchService, _validateInputs);

                            string branchManagerName = _commonHelperService.GetName(Miscellaneous.branchManager, _validateInputs);
                            string branchManagerPassword = _commonHelperService.GetPassword(Miscellaneous.branchManager, _validateInputs);

                            message = _managerService.OpenManagerAccount(headManagerBankId, branchId, branchManagerName, branchManagerPassword);
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
                            OpenManagerAccountPending = false;
                            Console.WriteLine(message.ResultMessage);
                            break;
                        }
                    }
                    break;

                case 3: //UpdateManagerAccount 
                    bool case3Pending = true;
                    while (case3Pending)
                    {
                        message = _branchService.IsBranchesExist(headManagerBankId);
                        if (message.Result)
                        {
                            string branchId = _commonHelperService.GetBranchId(Miscellaneous.branchManager, _branchService, _validateInputs);
                            message = _managerService.IsManagersExist(headManagerBankId, branchId);
                            if (message.Result)
                            {
                                string managerAccountId = _commonHelperService.GetAccountId(Miscellaneous.branchManager, _validateInputs);

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
                            else
                            {
                                Console.WriteLine(message.ResultMessage);
                                case3Pending = false;
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            case3Pending = false;
                            break;
                        }
                    }
                    break;

                case 4://DeleteManagerAccount
                    bool deleteManagerAccountPending = true;
                    while (deleteManagerAccountPending)
                    {
                        message = _branchService.IsBranchesExist(headManagerBankId);
                        if (message.Result)
                        {
                            string branchId = _commonHelperService.GetBranchId(Miscellaneous.branchManager, _branchService, _validateInputs);
                            message = _managerService.IsManagersExist(headManagerBankId, branchId);
                            if (message.Result)
                            {
                                string managerAccountId = _commonHelperService.GetAccountId(Miscellaneous.branchManager, _validateInputs);

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
                            else
                            {
                                Console.WriteLine(message.ResultMessage);
                                deleteManagerAccountPending = false;
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            deleteManagerAccountPending = false;
                            break;
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
                            string currencyCode = Console.ReadLine()?.ToUpper() ?? string.Empty;
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
                                        bool isValid = decimal.TryParse(Console.ReadLine(), out exchangeRate);

                                        if (isValid && exchangeRate == 0)
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
                    break;

                case 6: //UpdateCurrency with exchange Rates
                    bool UpdateExchangeRatesPendingStatus = true;
                    while (UpdateExchangeRatesPendingStatus)
                    {
                        message = _bankService.GetExchangeRates(headManagerBankId);
                        if (message.Result)
                        {
                            Console.WriteLine(message.ResultMessage);
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
                                            bool isValid = decimal.TryParse(Console.ReadLine(), out exchangeRate);

                                            if (isValid && exchangeRate == 0)
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
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            UpdateExchangeRatesPendingStatus = false;
                            break;
                        }
                    }
                    break;

                case 7: //DeleteExchangeRates
                    bool deleteExchangeRatesPendingStatus = true;
                    while (deleteExchangeRatesPendingStatus)
                    {
                        message = _bankService.GetExchangeRates(headManagerBankId);
                        if (message.Result)
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
                            deleteExchangeRatesPendingStatus = false;
                            break;
                        }

                    }
                    break;
            }
        }
    }
}
