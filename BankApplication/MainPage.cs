using BankApplication;
using BankApplicationModels.Enums;
using BankApplicationServices.Interfaces;
using BankApplicationServices.Services;

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

                ushort Option = CommonHelper.GetOption(Miscellaneous.mainPage);

                switch (Option)
                {
                    case 1: //customer Login
                        bool customerloginPending = true;
                        while (customerloginPending)
                        {
                            string customerBankId = CommonHelper.GetBankId(Miscellaneous.customer);
                            string customerBranchId = CommonHelper.GetBranchId(Miscellaneous.customer);
                            string customerAccountId = CommonHelper.GetAccountId(Miscellaneous.customer);
                            string customerPassword = CommonHelper.GetPassword(Miscellaneous.customer);

                            CustomerService branchCustomerService = new BranchCustomerService();
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
                                    ushort selectedOption = CommonHelper.GetOption(Miscellaneous.customer);
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
                            string staffbankId = CommonHelper.GetBankId(Miscellaneous.staff);
                            string staffbranchId = CommonHelper.GetBranchId(Miscellaneous.staff);
                            string staffAccountId = CommonHelper.GetAccountId(Miscellaneous.staff);
                            string staffPassword = CommonHelper.GetPassword(Miscellaneous.staff);

                            StaffService branchStaffService = new BranchStaffService();
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
                                    ushort selectedOption = CommonHelper.GetOption(Miscellaneous.staff);
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
                            string managerbankId = CommonHelper.GetBankId(Miscellaneous.branchManager);
                            string managerbranchId = CommonHelper.GetBranchId(Miscellaneous.branchManager);
                            string branchManagerAccountId = CommonHelper.GetAccountId(Miscellaneous.branchManager);
                            string branchManagerPassword = CommonHelper.GetPassword(Miscellaneous.branchManager);

                            ManagerService branchManagerService = new BranchManagerService();
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
                                    ushort selectedOption = CommonHelper.GetOption(Miscellaneous.branchManager);
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
                            string headManagerbankId = CommonHelper.GetBankId(Miscellaneous.headManager);
                            string headManagerbranchId = CommonHelper.GetBranchId(Miscellaneous.headManager);
                            string headManagerAccountId = CommonHelper.GetAccountId(Miscellaneous.headManager);
                            string headManagerPassword = CommonHelper.GetPassword(Miscellaneous.headManager);

                            HeadManagerService bankHeadManagerService = new BankHeadManagerService();
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
                                    ushort selectedOption = CommonHelper.GetOption(Miscellaneous.headManager);
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
                            string ReserveBankManagerName = CommonHelper.GetName(Miscellaneous.reserveBankManager);
                            string ReserveBankManagerPassword = CommonHelper.GetPassword(Miscellaneous.reserveBankManager);
                            ReserveBankManagerService reserveBank = new ReserveBankService();
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
                                    ushort selectedOption = CommonHelper.GetOption(Miscellaneous.reserveBankManager);
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