using BankApplication;
using BankApplication.Models.Enums;
using BankApplication.Models.Exceptions;
using BankApplication.Models.HelperMethods;
using BankApplication.Services;

internal class MainPage
{
    private static void Main(string[] args)
    {
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

                ushort Option = MainPageHelper.GetOption(Miscellaneous.mainPage);

                switch (Option)
                {
                    case 1: //customer Login
                        bool customerloginPending = true;
                        while (customerloginPending)
                        {
                            string customerBankId = MainPageHelper.GetBankId(Miscellaneous.customer);
                            string customerBranchId = MainPageHelper.GetBranchId(Miscellaneous.customer);
                            string customerAccountId = MainPageHelper.GetAccountId(Miscellaneous.customer);
                            string customerPassword = MainPageHelper.GetPassword(Miscellaneous.customer);

                            BranchCustomerService branchCustomerService = new BranchCustomerService();
                            Message isCustomerExist = branchCustomerService.ValidateCustomerLogin(customerBankId, customerBranchId, customerAccountId, customerPassword);

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
                                    ushort selectedOption = MainPageHelper.GetOption(Miscellaneous.customer);
                                    if (selectedOption == 0)
                                    {
                                        isCustomerActionsPending = false;
                                        customerloginPending = false;
                                        break;
                                    }
                                    else 
                                    {
                                        CustomerHelperMethods.SelectedOption(selectedOption, customerBankId, customerBranchId);
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
                            string staffbankId = MainPageHelper.GetBankId(Miscellaneous.staff);
                            string staffbranchId = MainPageHelper.GetBranchId(Miscellaneous.staff);
                            string staffAccountId = MainPageHelper.GetAccountId(Miscellaneous.staff);
                            string staffPassword = MainPageHelper.GetPassword(Miscellaneous.staff);

                            BranchStaffService branchStaffService = new BranchStaffService();
                            Message isStaffExist = branchStaffService.ValidateBranchStaffAccount(staffbankId, staffbranchId, staffAccountId, staffPassword);

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
                                    ushort selectedOption = MainPageHelper.GetOption(Miscellaneous.staff);
                                    if (selectedOption == 0)
                                    {
                                        isStaffPending = false;
                                        stafloginPending = false;
                                        break;
                                    }
                                    else
                                    {
                                        StaffHelperMethod.SelectedOption(selectedOption, staffbankId, staffbranchId);
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
                            string managerbankId = MainPageHelper.GetBankId(Miscellaneous.branchManager);
                            string managerbranchId = MainPageHelper.GetBranchId(Miscellaneous.branchManager);
                            string branchManagerAccountId = MainPageHelper.GetAccountId(Miscellaneous.branchManager);
                            string branchManagerPassword = MainPageHelper.GetPassword(Miscellaneous.branchManager);

                            BranchManagerService branchManagerService = new BranchManagerService();
                            Message isBranchManagerAccountExist = branchManagerService.ValidateBranchManagerAccount(managerbankId, managerbranchId, branchManagerAccountId, branchManagerPassword);
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
                                    ushort selectedOption = MainPageHelper.GetOption(Miscellaneous.branchManager);
                                    if (selectedOption == 0)
                                    {
                                        isManagerActionsPending = false;
                                        managerLoginPending = false;
                                        break;
                                    }
                                    else
                                    {
                                        StaffHelperMethod.SelectedOption(selectedOption, managerbankId, managerbranchId);
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
                            string headManagerbankId = MainPageHelper.GetBankId(Miscellaneous.headManager);
                            string headManagerbranchId = MainPageHelper.GetBranchId(Miscellaneous.headManager);
                            string headManagerAccountId = MainPageHelper.GetAccountId(Miscellaneous.headManager);
                            string headManagerPassword = MainPageHelper.GetPassword(Miscellaneous.headManager);

                            BankHeadManagerService bankHeadManagerService = new BankHeadManagerService();
                            Message isHeadManagerExist = bankHeadManagerService.ValidateBankHeadManager(headManagerbankId, headManagerbranchId, headManagerAccountId, headManagerPassword);
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
                                    ushort selectedOption = MainPageHelper.GetOption(Miscellaneous.headManager);
                                    if (selectedOption == 0)
                                    {
                                        isHeadManagerActionsPending = false;
                                        headManagerLoginPending = false;
                                        break;
                                    }
                                    else
                                    {
                                        StaffHelperMethod.SelectedOption(selectedOption, headManagerbankId, headManagerbranchId);
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
                            string ReserveBankManagerName = MainPageHelper.GetName(Miscellaneous.reserveBankManager);
                            string ReserveBankManagerPassword = MainPageHelper.GetPassword(Miscellaneous.reserveBankManager);
                            ReserveBankService reserveBank = new ReserveBankService();
                            Message isReserveManagerExist = reserveBank.ValidateReserveBankManager(ReserveBankManagerName, ReserveBankManagerPassword);
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
                                    ushort selectedOption = MainPageHelper.GetOption(Miscellaneous.reserveBankManager);
                                    if (selectedOption == 0)
                                    {
                                        isReserveBankManagerActionsPending = false;
                                        reserveBankMangerLoginPending = false;
                                        break;
                                    }
                                    else
                                    {
                                        ReserveBankManagerHelperMethod.SelectedOption(selectedOption);
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