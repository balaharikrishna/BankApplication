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
        Message message = new();
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
                    case 1: //Customer Login
                        commonHelperService.LoginAccountHolder(Miscellaneous.customer,bankService,branchService,validateInputs,customerHelperService,
                            null,null,null,customerService, null, null, null);
                        break;
                    
                    case 2: //Staff Login
                        commonHelperService.LoginAccountHolder(Miscellaneous.staff, bankService, branchService, validateInputs, null,
                            staffHelperService, null, null, null, staffService, null, null);
                        break;

                    case 3: //Manager Login
                        commonHelperService.LoginAccountHolder(Miscellaneous.staff, bankService, branchService, validateInputs, null,
                            null, managerHelperService, null, null, null, managerService, null);
                        break;

                    case 4: //Head Manager Login
                        commonHelperService.LoginAccountHolder(Miscellaneous.staff, bankService, branchService, validateInputs, null,
                            null, null, headManagerHelperService, null, null, null, headManagerService);
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