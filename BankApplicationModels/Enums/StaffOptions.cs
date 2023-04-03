using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationModels.Enums
{
    public enum StaffOptions
    {
        OpenCustomerAccount = 1, 
        UpdateCustomerAccount = 2, 
        DeleteCustomerAccount = 3, 
        DisplayCustomerTransactionHistory = 4,  
        RevertCustomerTransaction = 5,   
        CheckCustomerAccountBalance = 6,  
        GetExchangeRates = 7, 
        GetTransactionCharges = 8, 
        DepositAmountInCustomerAccount = 9, 
        TransferAmount = 10 
    }
}
