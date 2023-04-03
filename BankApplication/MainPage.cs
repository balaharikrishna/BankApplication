using BankApplication;
using BankApplication.Exceptions;
using BankApplication.IHelperServices;
using BankApplicationHelperMethods;
using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;
using Microsoft.Extensions.DependencyInjection;

internal class MainPage
{
    public static readonly IServiceProvider services = DIContainerBuilder.Build();

    private static void Main(string[] args)
    {
        IBankService bankService = services.GetService<IBankService>()!;
        IBranchService branchService = services.GetService<IBranchService>()!;
        ICustomerService customerService = services.GetService<ICustomerService>()!;
        IHeadManagerService headManagerService = services.GetService<IHeadManagerService>()!;
        IManagerService managerService = services.GetService<IManagerService>()!;
        IReserveBankManagerService reserveBankManagerService = services.GetService<IReserveBankManagerService>()!;
        IStaffService staffService = services.GetService<IStaffService>()!;
        ICommonHelperService commonHelperService = services.GetService<ICommonHelperService>()!;
        ICustomerHelperService customerHelperService = services.GetService<ICustomerHelperService>()!;
        IHeadManagerHelperService headManagerHelperService = services.GetService<IHeadManagerHelperService>()!;
        IManagerHelperService managerHelperService = services.GetService<IManagerHelperService>()!;
        IReserveBankManagerHelperService reserveBankManagerHelperService = services.GetService<IReserveBankManagerHelperService>()!;
        IStaffHelperService staffHelperService = services.GetService<IStaffHelperService>()!;
        IValidateInputs validateInputs = services.GetService<IValidateInputs>()!;
        bool pendingTask = true;
        Message message = new Message();
        while (pendingTask)
        {
            try
            {
                Console.WriteLine("Welcome... Please Choose the Following Options..");
                foreach (MainPageOptions option in Enum.GetValues(typeof(MainPageOptions)))
                {
                    Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                }

                ushort Option = commonHelperService.GetOption(Miscellaneous.mainPage);

                switch (Option)
                {
                    case 1: //customer Login
                        bool customerloginPending = true;
                        while (customerloginPending)
                        {
                            bool inValidBankId = true;
                            while (inValidBankId)
                            {
                                string customerBankId = commonHelperService.GetBankId(Miscellaneous.customer, bankService, validateInputs);
                                message = bankService.AuthenticateBankId(customerBankId);
                                if (message.Result)
                                {
                                    message = branchService.IsBranchesExist(customerBankId);
                                    if (message.Result)
                                    {
                                        bool inValidBranchId = true;
                                        while (inValidBranchId)
                                        {
                                            string customerBranchId = commonHelperService.GetBranchId(Miscellaneous.customer, branchService, validateInputs);
                                            message = branchService.AuthenticateBranchId(customerBankId, customerBranchId);
                                            if (message.Result)
                                            {
                                                message = customerService.IsCustomersExist(customerBankId, customerBranchId);
                                                if (message.Result)
                                                {
                                                    bool inValidAccountId = true;
                                                    while (inValidAccountId)
                                                    {
                                                        string customerAccountId = commonHelperService.GetAccountId(Miscellaneous.customer, validateInputs);
                                                        message = customerService.IsAccountExist(customerBankId, customerBranchId, customerAccountId);
                                                        if (message.Result)
                                                        {
                                                            string customerPassword = commonHelperService.GetPassword(Miscellaneous.customer, validateInputs);
                                                            message = customerService.AuthenticateCustomerAccount(customerBankId, customerBranchId, customerAccountId, customerPassword);

                                                            if (message.Result)
                                                            {
                                                                bool isCustomerActionsPending = true;
                                                                while (isCustomerActionsPending)
                                                                {
                                                                    Console.WriteLine("Choose From Below Menu Options");
                                                                    foreach (CustomerOptions option in Enum.GetValues(typeof(CustomerOptions)))
                                                                    {
                                                                        Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                                                    }
                                                                    Console.WriteLine("Enter 0 For Main Menu");
                                                                    ushort selectedOption = commonHelperService.GetOption(Miscellaneous.customer);
                                                                    if (selectedOption == 0)
                                                                    {
                                                                        isCustomerActionsPending = false;
                                                                        customerloginPending = false;
                                                                        inValidAccountId = false;
                                                                        inValidBranchId = false;
                                                                        inValidBankId = false;
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        customerHelperService.SelectedOption(selectedOption, customerBankId, customerBranchId, customerAccountId);
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
                                                    inValidBranchId = false;
                                                    inValidBankId = false;
                                                    customerloginPending = false;
                                                    break;
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
                                        inValidBankId = false;
                                        customerloginPending = false;
                                        break;
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

                    case 2: //staff Login
                        bool staffloginPending = true;
                        while (staffloginPending)
                        {
                            bool inValidBankId = true;
                            while (inValidBankId)
                            {
                                string staffBankId = commonHelperService.GetBankId(Miscellaneous.staff, bankService, validateInputs);
                                message = bankService.AuthenticateBankId(staffBankId);
                                if (message.Result)
                                {
                                    message = branchService.IsBranchesExist(staffBankId);
                                    if (message.Result)
                                    {
                                        bool inValidBranchId = true;
                                        while (inValidBranchId)
                                        {
                                            string staffBranchId = commonHelperService.GetBranchId(Miscellaneous.staff, branchService, validateInputs);
                                            message = branchService.AuthenticateBranchId(staffBankId, staffBranchId);
                                            if (message.Result)
                                            {
                                                message = staffService.IsStaffExist(staffBankId, staffBranchId);
                                                if (message.Result)
                                                {
                                                    bool inValidAccountId = true;
                                                    while (inValidAccountId)
                                                    {
                                                        string staffAccountId = commonHelperService.GetAccountId(Miscellaneous.staff, validateInputs);
                                                        message = staffService.IsAccountExist(staffBankId, staffBranchId, staffAccountId);
                                                        if (message.Result)
                                                        {
                                                            string staffPassword = commonHelperService.GetPassword(Miscellaneous.staff, validateInputs);
                                                            message = staffService.AuthenticateStaffAccount(staffBankId, staffBranchId, staffAccountId, staffPassword);

                                                            if (message.Result)
                                                            {
                                                                bool isStaffActionsPending = true;
                                                                while (isStaffActionsPending)
                                                                {
                                                                    Console.WriteLine("Choose From Below Menu Options");
                                                                    foreach (StaffOptions option in Enum.GetValues(typeof(StaffOptions)))
                                                                    {
                                                                        Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                                                    }
                                                                    Console.WriteLine("Enter 0 For Main Menu");
                                                                    ushort selectedOption = commonHelperService.GetOption(Miscellaneous.staff);
                                                                    if (selectedOption == 0)
                                                                    {
                                                                        isStaffActionsPending = false;
                                                                        staffloginPending = false;
                                                                        inValidAccountId = false;
                                                                        inValidBranchId = false;
                                                                        inValidBankId = false;
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        staffHelperService.SelectedOption(selectedOption, staffBankId, staffBranchId);
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
                                                    inValidBranchId = false;
                                                    inValidBankId = false;
                                                    staffloginPending = false;
                                                    break;
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
                                        inValidBankId = false;
                                        staffloginPending = false;
                                        break;
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

                    case 3: //Manager Login
                        bool managerloginPending = true;
                        while (managerloginPending)
                        {
                            bool inValidBankId = true;
                            while (inValidBankId)
                            {
                                string managerBankId = commonHelperService.GetBankId(Miscellaneous.branchManager, bankService, validateInputs);
                                message = bankService.AuthenticateBankId(managerBankId);
                                if (message.Result)
                                {
                                    message = branchService.IsBranchesExist(managerBankId);
                                    if (message.Result)
                                    {
                                        bool inValidBranchId = true;
                                        while (inValidBranchId)
                                        {
                                            string managerBranchId = commonHelperService.GetBranchId(Miscellaneous.branchManager, branchService, validateInputs);
                                            message = branchService.AuthenticateBranchId(managerBankId, managerBranchId);
                                            if (message.Result)
                                            {
                                                message = managerService.IsManagersExist(managerBankId, managerBranchId);
                                                if (message.Result)
                                                {
                                                    bool inValidAccountId = true;
                                                    while (inValidAccountId)
                                                    {
                                                        string managerAccountId = commonHelperService.GetAccountId(Miscellaneous.branchManager, validateInputs);
                                                        message = managerService.IsAccountExist(managerBankId, managerBranchId, managerAccountId);
                                                        if (message.Result)
                                                        {
                                                            string managerPassword = commonHelperService.GetPassword(Miscellaneous.branchManager, validateInputs);
                                                            message = managerService.AuthenticateManagerAccount(managerBankId, managerBranchId, managerAccountId, managerPassword);

                                                            if (message.Result)
                                                            {
                                                                bool isManagerActionsPending = true;
                                                                while (isManagerActionsPending)
                                                                {
                                                                    Console.WriteLine("Choose From Below Menu Options");
                                                                    foreach (ManagerOptions option in Enum.GetValues(typeof(ManagerOptions)))
                                                                    {
                                                                        Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                                                    }
                                                                    Console.WriteLine("Enter 0 For Main Menu");
                                                                    ushort selectedOption = commonHelperService.GetOption(Miscellaneous.branchManager);
                                                                    if (selectedOption == 0)
                                                                    {
                                                                        isManagerActionsPending = false;
                                                                        inValidAccountId = false;
                                                                        managerloginPending = false;
                                                                        inValidBranchId = false;
                                                                        inValidBankId = false;
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        managerHelperService.SelectedOption(selectedOption, managerBankId, managerBranchId, managerAccountId);
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
                                                    inValidBranchId = false;
                                                    inValidBankId = false;
                                                    managerloginPending = false;
                                                    break;
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
                                        inValidBankId = false;
                                        managerloginPending = false;
                                        break;
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

                    case 4: //Head Manager Login
                        bool headManagerloginPending = true;
                        while (headManagerloginPending)
                        {
                            bool inValidBankId = true;
                            while (inValidBankId)
                            {
                                string headManagerBankId = commonHelperService.GetBankId(Miscellaneous.headManager, bankService, validateInputs);
                                message = bankService.AuthenticateBankId(headManagerBankId);
                                if (message.Result)
                                {
                                    message = headManagerService.IsHeadManagersExist(headManagerBankId);
                                    if (message.Result)
                                    {
                                        bool inValidAccountId = true;
                                        while (inValidAccountId)
                                        {
                                            string headManagerAccountId = commonHelperService.GetAccountId(Miscellaneous.headManager, validateInputs);
                                            message = headManagerService.IsHeadManagerExist(headManagerBankId, headManagerAccountId);
                                            if (message.Result)
                                            {
                                                string haedManagerPassword = commonHelperService.GetPassword(Miscellaneous.customer, validateInputs);
                                                message = headManagerService.AuthenticateHeadManager(headManagerBankId, headManagerAccountId, haedManagerPassword);

                                                if (message.Result)
                                                {
                                                    bool isHeadManagerActionsPending = true;
                                                    while (isHeadManagerActionsPending)
                                                    {
                                                        Console.WriteLine("Choose From Below Menu Options");
                                                        foreach (HeadManagerOptions option in Enum.GetValues(typeof(HeadManagerOptions)))
                                                        {
                                                            Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                                        }
                                                        Console.WriteLine("Enter 0 For Main Menu");
                                                        ushort selectedOption = commonHelperService.GetOption(Miscellaneous.headManager);
                                                        if (selectedOption == 0)
                                                        {
                                                            isHeadManagerActionsPending = false;
                                                            headManagerloginPending = false;
                                                            inValidAccountId = false;
                                                            inValidBankId = false;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            headManagerHelperService.SelectedOption(selectedOption, headManagerBankId);
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
                                        inValidBankId = false;
                                        headManagerloginPending = false;
                                        break;
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
                    case 5: //Reserve Bank Login
                        bool reserveBankMangerLoginPending = true;
                        while (reserveBankMangerLoginPending)
                        {
                            string ReserveBankManagerName = commonHelperService.GetName(Miscellaneous.reserveBankManager, validateInputs);
                            string ReserveBankManagerPassword = commonHelperService.GetPassword(Miscellaneous.reserveBankManager, validateInputs);

                            Message isReserveManagerExist = reserveBankManagerService.AuthenticateReserveBankManager(ReserveBankManagerName, ReserveBankManagerPassword);
                            if (isReserveManagerExist.Result)
                            {
                                bool isReserveBankManagerActionsPending = true;
                                while (isReserveBankManagerActionsPending)
                                {
                                    Console.WriteLine("Choose From Below Menu Options");
                                    foreach (ReserveBankManagerOptions option in Enum.GetValues(typeof(ReserveBankManagerOptions)))
                                    {
                                        Console.WriteLine("Enter {0} For {1}", (int)option, option.ToString());
                                    }
                                    Console.WriteLine("Enter 0 For Main Menu");
                                    ushort selectedOption = commonHelperService.GetOption(Miscellaneous.reserveBankManager);
                                    if (selectedOption == 0)
                                    {
                                        isReserveBankManagerActionsPending = false;
                                        reserveBankMangerLoginPending = false;
                                        break;
                                    }
                                    else
                                    {
                                        reserveBankManagerHelperService.SelectedOption(selectedOption);
                                        continue;
                                    }

                                }
                            }
                            else
                            {
                                Console.WriteLine(isReserveManagerExist.ResultMessage);
                                continue;
                            }
                        }
                        break;
                    default:
                        throw new InvalidOptionException(Option);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                continue;
            }
        }

    }
}