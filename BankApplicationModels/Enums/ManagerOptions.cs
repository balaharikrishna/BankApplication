using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationModels.Enums
{
    internal enum ManagerOptions
    {
        OpenStaffAccount = 1,
        AddTransactionCharges = 2,
        OpenCustomerAccount = 3, //
        UpdateCustomerAccount = 4, // 
        DeleteCustomerAccount = 5,  // 
        DisplayCustomerTransactionHistory = 6,  //    
        RevertCustomerTransaction = 7,   //   
        CheckCustomerAccountBalance = 8,  // 
        GetExchangeRates = 9, // 
        GetTransactionCharges = 10, // 
        DepositAmountInCustomerAccount = 11, // 
        TransferAmount = 12 // 


    }
}
