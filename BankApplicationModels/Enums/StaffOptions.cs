using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationModels.Enums
{
    internal enum StaffOptions
    {
        OpenCustomerAccount = 1, //wokring
        UpdateCustomerAccount = 2, // wokring
        DeleteCustomerAccount = 3,  //wokring
        DisplayCustomerTransactionHistory = 4,  //working fne 
        RevertCustomerTransaction = 5,   //working fine
        CheckCustomerAccountBalance = 6,  //working
        GetExchangeRates = 7, //working
        GetTransactionCharges = 8, //working
        DepositAmountInCustomerAccount = 9, //working
        TransferAmount = 10 //working
    }
}
