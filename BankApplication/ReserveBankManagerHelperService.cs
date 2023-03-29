using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;
using BankApplicationServices.Services;

namespace BankApplication
{
    internal class ReserveBankManagerHelperService : IReserveBankManagerHelperService
    {
        IBankService _bankService;
        IHeadManagerService _headManagerService;
        ICommonHelperService _commonHelperService;
        IValidateInputs _validateInputs;
        public ReserveBankManagerHelperService(IBankService bankService, IHeadManagerService headManagerService,
            ICommonHelperService commonHelperService,IValidateInputs validateInputs)
        {
            _bankService = bankService;
            _headManagerService = headManagerService;
            _commonHelperService = commonHelperService;
            _validateInputs = validateInputs;
        }
        Message message = new Message();
    
        public void SelectedOption(ushort Option)
        {
            switch (Option)
            {
                case 1: //create Bank
                    bool case1Pending = true;
                    while (case1Pending)
                    {
                        string bankName = _commonHelperService.GetName(Miscellaneous.bank);

                        message = _bankService.CreateBank(bankName);
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
                    break;

                case 2: //create HeadManager
                    bool headManagerCreateStatus = true;
                    while (headManagerCreateStatus)
                    {
                        string bankHeadManagerName = _commonHelperService.GetName(Miscellaneous.headManager);
                        string bankHeadManagerPassword = _commonHelperService.GetPassword(Miscellaneous.headManager);
                        string bankId = _commonHelperService.GetBankId(Miscellaneous.bank,_bankService);

                        message = _headManagerService.OpenHeadManagerAccount(bankId, bankHeadManagerName, bankHeadManagerPassword);
                        if (message.Result)
                        {
                            Console.WriteLine(message.ResultMessage);
                            headManagerCreateStatus = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
                        }
                    }
                    break;

                case 3: //UpdateHeadManager
                    bool case3Pending = true;
                    while (case3Pending)
                    {
                        string headManagerAccountId = _commonHelperService.GetAccountId(Miscellaneous.headManager);
                        string bankId = _commonHelperService.GetBankId(Miscellaneous.bank, _bankService);
                        message = _headManagerService.IsHeadManagerExist(bankId, headManagerAccountId);
                        if (message.Result)
                        {
                            string headManagerDetatils = _headManagerService.GetHeadManagerDetails(bankId, headManagerAccountId);
                            Console.WriteLine("Head Manager Details:");
                            Console.WriteLine(headManagerDetatils);

                            string headManagerName = string.Empty;
                            bool invalidHeadManagerName = true;
                            while (invalidHeadManagerName)
                            {
                                Console.WriteLine("Enter Head Manager Name");
                                headManagerName = Console.ReadLine() ?? string.Empty;
                                if (headManagerName != string.Empty)
                                {
                                    message = _validateInputs.ValidateNameFormat(headManagerName);
                                    if (!message.Result)
                                    {
                                        Console.WriteLine(message.ResultMessage);
                                        continue;
                                    }
                                    else
                                    {

                                        invalidHeadManagerName = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    invalidHeadManagerName = false;
                                    break;
                                }
                            }

                            string headManagerPassword = string.Empty;
                            bool invalidHeadManagerPassword = true;
                            while (invalidHeadManagerPassword)
                            {
                                Console.WriteLine("Update Staff Password");
                                headManagerPassword = Console.ReadLine() ?? string.Empty;
                                if (headManagerPassword != string.Empty)
                                {
                                    message = _validateInputs.ValidatePasswordFormat(headManagerPassword);
                                    if (!message.Result)
                                    {
                                        Console.WriteLine(message.ResultMessage);
                                        continue;
                                    }
                                    else
                                    {

                                        invalidHeadManagerPassword = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    invalidHeadManagerPassword = false;
                                    break;
                                }
                            }

                            message = _headManagerService.UpdateHeadManagerAccount(bankId, headManagerAccountId, headManagerName, headManagerPassword);

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
                    
                case 4: //DeleteHeadManager
                    bool bankHeadManagerUpdateStatus = true;
                    while (bankHeadManagerUpdateStatus)
                    {
                        string headManagerAccountId = _commonHelperService.GetAccountId(Miscellaneous.headManager);
                        string headManagerName = _commonHelperService.GetName(Miscellaneous.headManager);
                        string headManagerPassword = _commonHelperService.GetPassword(Miscellaneous.headManager);
                        string bankId = _commonHelperService.GetBankId(Miscellaneous.bank,_bankService);

                        message = _headManagerService.UpdateHeadManagerAccount(bankId, headManagerAccountId, headManagerName, headManagerPassword);
                        if (message.Result)
                        {
                            Console.WriteLine(message.ResultMessage);
                            bankHeadManagerUpdateStatus = false;
                            break;
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

