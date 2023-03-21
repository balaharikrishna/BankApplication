namespace BankApplicationHelperMethods
{
    internal class HeadManagerHelperMethod
    {
        public static void SelectedOption(short Option, string headManagerBankId, string headManagerBranchId)
        {

            BankHeadManagerService bankHeadManagerService = new BankHeadManagerService();
            Message message = new Message();

            switch (Option)
            {
                case 1: //CreateBankBranch
                    bool branchPendingStatus = true;
                    while (branchPendingStatus)
                    {
                        string branchName = CommonHelperMethods.GetName(Miscellaneous.branch);
                        string branchPhoneNumber = CommonHelperMethods.GetPhoneNumber(Miscellaneous.branch);
                        string branchAddress = CommonHelperMethods.GetAddress(Miscellaneous.branch);

                        message = bankHeadManagerService.CreateBankBranch(branchName, branchPhoneNumber, branchAddress);
                        if (message.Result)
                        {
                            Console.WriteLine(message.ResultMessage);
                            Console.WriteLine("Enter 0 to Go Back");
                            short userInput = 0;
                            short.TryParse(Console.ReadLine(), out userInput);
                            if (userInput == 0)
                            {
                                branchPendingStatus = false;
                                break;
                            }

                            else if (userInput != 0)
                            {
                                Console.WriteLine($"Entered Value {userInput} is invalid please provide valid input");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(message.ResultMessage);
                            continue;
                        }

                    }
                    break;
                case 2: //OpenBranchManagerAcccount

                    bool branchManagerAccountPending = true;
                    while (branchManagerAccountPending)
                    {
                        string branchManagerName = CommonHelperMethods.GetName(Miscellaneous.branchManager);
                        string branchManagerPassword = CommonHelperMethods.GetPassword(Miscellaneous.branchManager);

                        message = bankHeadManagerService.OpenBranchManagerAccount(headManagerBranchId, branchManagerName, branchManagerPassword);
                        if (message.Result)
                        {
                            Console.WriteLine(message.ResultMessage);
                            Console.WriteLine("Enter 0 to Go Back");
                            short userInput = short.Parse(Console.ReadLine());
                            if (userInput == 0)
                            {
                                branchManagerAccountPending = false;
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
                case 3: //AddExchangeRates
                    bool exchangeRatesPendingStatus = true;
                    while (exchangeRatesPendingStatus)
                    {
                        bool CurrencyCodePending = true;
                        while (CurrencyCodePending)
                        {
                            string currencyCode = CommonHelperMethods.ValidateCurrency(headManagerBankId);

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
                                    message = bankHeadManagerService.AddCurrencyWithExchangeRate(currencyCode, exchangeRate);
                                    if (message.Result)
                                    {
                                        Console.WriteLine($"currency {currencyCode} with ExchangeRate {exchangeRate} added Successfully");
                                        exchangeRatePending = false;
                                        CurrencyCodePending = false;
                                        exchangeRatesPendingStatus = false;
                                        break;

                                    }

                                }


                            }

                        }

                    }
                    break;
            }

        }
    }
}
