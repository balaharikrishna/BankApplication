namespace BankApplicationHelperMethods
{
    internal class ReserveBankManagerHelperMethod
    {
        public static void SelectedOption(ushort Option)
        {

            ReserveBankService reserveBankService = new ReserveBankService();
            Message message = new Message();

            switch (Option)
            {
                case 1: //create Bank
                    bool case1Pending = true;
                    while (case1Pending)
                    {
                        string bankName = CommonHelperMethods.GetName(Miscellaneous.bank);

                        message = reserveBankService.CreateBank(bankName);
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

                case 2: //create BankHeadManager
                    bool bankHeadManagerCreateStatus = true;
                    while (bankHeadManagerCreateStatus)
                    {

                        string bankHeadManagerName = CommonHelperMethods.GetName(Miscellaneous.headManager);
                        string bankHeadManagerPassword = CommonHelperMethods.GetPassword(Miscellaneous.headManager);
                        string bankId = CommonHelperMethods.GetBankId(Miscellaneous.bank);

                        message = reserveBankService.CreateBankHeadManagerAccount(bankId, bankHeadManagerName, bankHeadManagerPassword);
                        if (message.Result)
                        {
                            Console.WriteLine(message.ResultMessage);
                            bankHeadManagerCreateStatus = false;
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

