using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationModels.Enums
{
    internal enum CustomerOptions
    {
        CheckAccountBalance = 1,
        ViewTransactionHistory = 2,
        ViewExchangeRates = 3,
        ViewTransactionCharges = 4,
        WithdrawAmount = 5,
        TransferAmount = 6,
        ViewPassBook = 7
    }
}
