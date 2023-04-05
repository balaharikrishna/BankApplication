using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationServices.IServices;

namespace BankApplication
{
    internal class ReserveBankManagerHelperService : IReserveBankManagerHelperService
    {
        readonly IBankService _bankService;
        readonly IHeadManagerService _headManagerService;
        readonly ICommonHelperService _commonHelperService;
        readonly IValidateInputs _validateInputs;
        public ReserveBankManagerHelperService(IBankService bankService, IHeadManagerService headManagerService,
            ICommonHelperService commonHelperService, IValidateInputs validateInputs)
        {
            _bankService = bankService;
            _headManagerService = headManagerService;
            _commonHelperService = commonHelperService;
            _validateInputs = validateInputs;
        }
        Message message = new();

        public void SelectedOption(ushort Option)
        {
            switch (Option)
            {
                case 1: //create Bank
                    while (true)
                    {
                        string bankName = _commonHelperService.GetName(Miscellaneous.bank, _validateInputs);

                        message = _bankService.CreateBank(bankName);
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

                case 2: //Create HeadManager 
                    while (true)
                    {
                        string bankHeadManagerName = _commonHelperService.GetName(Miscellaneous.headManager, _validateInputs);
                        string bankHeadManagerPassword = _commonHelperService.GetPassword(Miscellaneous.headManager, _validateInputs);
                        string bankId = _commonHelperService.GetBankId(Miscellaneous.bank, _bankService, _validateInputs);

                        message = _headManagerService.OpenHeadManagerAccount(bankId, bankHeadManagerName, bankHeadManagerPassword);
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

                case 3: //UpdateHeadManager
                    while (true)
                    {
                        string bankId = _commonHelperService.GetBankId(Miscellaneous.bank, _bankService, _validateInputs);
                        message = _headManagerService.IsHeadManagersExist(bankId);
                        if (message.Result)
                        {
                            string headManagerAccountId = _commonHelperService.GetAccountId(Miscellaneous.headManager, _validateInputs);

                            message = _headManagerService.IsHeadManagerExist(bankId, headManagerAccountId);
                            if (message.Result)
                            {
                                string headManagerDetatils = _headManagerService.GetHeadManagerDetails(bankId, headManagerAccountId);
                                Console.WriteLine("Head Manager Details:");
                                Console.WriteLine(headManagerDetatils);

                                string headManagerName;
                                while (true)
                                {
                                    Console.WriteLine("Enter Head Manager Name");
                                    headManagerName = Console.ReadLine() ?? string.Empty;
                                    if (string.IsNullOrEmpty(headManagerName))
                                    {
                                        message = _validateInputs.ValidateNameFormat(headManagerName);
                                        if (!message.Result)
                                        {
                                            Console.WriteLine(message.ResultMessage);
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

                                string headManagerPassword;
                                while (true)
                                {
                                    Console.WriteLine("Update Staff Password");
                                    headManagerPassword = Console.ReadLine() ?? string.Empty;
                                    if (!string.IsNullOrEmpty(headManagerPassword))
                                    {
                                        message = _validateInputs.ValidatePasswordFormat(headManagerPassword);
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

                                message = _headManagerService.UpdateHeadManagerAccount(bankId, headManagerAccountId, headManagerName, headManagerPassword);

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

                case 4: //DeleteHeadManager
                    while (true)
                    {
                        string bankId = _commonHelperService.GetBankId(Miscellaneous.bank, _bankService, _validateInputs);
                        message = _headManagerService.IsHeadManagersExist(bankId);
                        if (message.Result)
                        {
                            string headManagerAccountId = _commonHelperService.GetAccountId(Miscellaneous.headManager, _validateInputs);

                            message = _headManagerService.DeleteHeadManagerAccount(bankId, headManagerAccountId);
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
                    break;
            }
        }
    }
}

