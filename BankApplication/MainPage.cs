using BankApplication;
using BankApplication.Exceptions;
using BankApplication.IHelperServices;
using BankApplicationModels;
using BankApplicationModels.Enums;
using BankApplicationServices.IServices;
using Microsoft.Extensions.DependencyInjection;

internal class MainPage
{
    public static readonly IServiceProvider services = DIContainerBuilder.Build();

    private static void Main(string[] args)
    {
        IBankService? bankService = services.GetService<IBankService>();
        IBranchService? branchService = services.GetService<IBranchService>();
        ICustomerService? customerService = services.GetService<ICustomerService>();
        IHeadManagerService? headManagerService = services.GetService<IHeadManagerService>();
        IManagerService? managerService = services.GetService<IManagerService>();
        IReserveBankManagerService? reserveBankManagerService = services.GetService<IReserveBankManagerService>();
        IStaffService? staffService = services.GetService<IStaffService>();
        ICommonHelperService? commonHelperService = services.GetService<ICommonHelperService>();
        ICustomerHelperService? customerHelperService = services.GetService<ICustomerHelperService>();
        IHeadManagerHelperService? headManagerHelperService = services.GetService<IHeadManagerHelperService>();
        IManagerHelperService? managerHelperService = services.GetService<IManagerHelperService>();
        IReserveBankManagerHelperService? reserveBankManagerHelperService = services.GetService<IReserveBankManagerHelperService>();
        IStaffHelperService? staffHelperService = services.GetService<IStaffHelperService>();
        bool pendingTask = true;
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
                            string customerBankId = commonHelperService.GetBankId(Miscellaneous.customer, bankService);
                            string customerBranchId = commonHelperService.GetBranchId(Miscellaneous.customer, branchService);
                            string customerAccountId = commonHelperService.GetAccountId(Miscellaneous.customer);
                            string customerPassword = commonHelperService.GetPassword(Miscellaneous.customer);

                            Message isCustomerExist = customerService.AuthenticateCustomerAccount(customerBankId, customerBranchId, customerAccountId, customerPassword);

                            if (isCustomerExist.Result)
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
                                Console.WriteLine(isCustomerExist.ResultMessage);
                                continue;
                            }
                        }
                        break;
                    case 2: //staff Login
                        bool stafloginPending = true;
                        while (stafloginPending)
                        {
                            string staffbankId = commonHelperService.GetBankId(Miscellaneous.staff, bankService);
                            string staffbranchId = commonHelperService.GetBranchId(Miscellaneous.staff, branchService);
                            string staffAccountId = commonHelperService.GetAccountId(Miscellaneous.staff);
                            string staffPassword = commonHelperService.GetPassword(Miscellaneous.staff);


                            Message isStaffExist = staffService.AuthenticateStaffAccount(staffbankId, staffbranchId, staffAccountId, staffPassword);

                            if (isStaffExist.Result)
                            {
                                bool isStaffPending = true;
                                while (isStaffPending)
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
                                        isStaffPending = false;
                                        stafloginPending = false;
                                        break;
                                    }
                                    else
                                    {
                                        staffHelperService.SelectedOption(selectedOption, staffbankId, staffbranchId);
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine(isStaffExist.ResultMessage);
                                continue;
                            }
                        }
                        break;
                    case 3: //Manager Login
                        bool managerLoginPending = true;
                        while (managerLoginPending)
                        {
                            string managerbankId = commonHelperService.GetBankId(Miscellaneous.branchManager, bankService);
                            string managerbranchId = commonHelperService.GetBranchId(Miscellaneous.branchManager, branchService);
                            string managerAccountId = commonHelperService.GetAccountId(Miscellaneous.branchManager);
                            string managerPassword = commonHelperService.GetPassword(Miscellaneous.branchManager);


                            Message isBranchManagerAccountExist = managerService.AuthenticateManagerAccount(managerbankId, managerbranchId, managerAccountId, managerPassword);
                            if (isBranchManagerAccountExist.Result)
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
                                        managerLoginPending = false;
                                        break;
                                    }
                                    else
                                    {
                                        managerHelperService.SelectedOption(selectedOption, managerbankId, managerbranchId, managerAccountId);
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine(isBranchManagerAccountExist.ResultMessage);
                                continue;
                            }
                        }
                        break;
                    case 4: //Head Manager Login
                        bool headManagerLoginPending = true;
                        while (headManagerLoginPending)
                        {
                            string headManagerbankId = commonHelperService.GetBankId(Miscellaneous.headManager, bankService);
                            string headManagerAccountId = commonHelperService.GetAccountId(Miscellaneous.headManager);
                            string headManagerPassword = commonHelperService.GetPassword(Miscellaneous.headManager);

                            Message isHeadManagerExist = headManagerService.AuthenticateHeadManager(headManagerbankId, headManagerAccountId, headManagerPassword);
                            if (isHeadManagerExist.Result)
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
                                        headManagerLoginPending = false;
                                        break;
                                    }
                                    else
                                    {
                                        headManagerHelperService.SelectedOption(selectedOption, headManagerbankId);
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine(isHeadManagerExist.ResultMessage);
                                continue;
                            }
                        }
                        break;
                    case 5: //Reserve Bank Login
                        bool reserveBankMangerLoginPending = true;
                        while (reserveBankMangerLoginPending)
                        {
                            string ReserveBankManagerName = commonHelperService.GetName(Miscellaneous.reserveBankManager);
                            string ReserveBankManagerPassword = commonHelperService.GetPassword(Miscellaneous.reserveBankManager);

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