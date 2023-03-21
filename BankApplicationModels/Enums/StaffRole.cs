using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationModels.Enums
{
    public enum StaffRole
    {
        AssistantManager = 1,///can do all the works of Clerk and Cashier.
        Cashier = 2, // will do withdraws, deposits , transactions .
        Clerk = 3,  ///will do the paper work and all other help related works
        HouseLoanOfficer = 4,
        GoldLoanOfficer = 5,
    }
}
