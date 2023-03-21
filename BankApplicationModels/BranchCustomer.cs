using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using BankApplicationModels.Enums;

namespace BankApplicationModels
{
    public class BranchCustomer
    {
        public string CustomerAccountID { get; set; }
        [RegularExpression("^[a-zA-Z]+$")]
        public string CustomerName { get; set; }
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string CustomerPassword { get; set; }

        public decimal CustomerAmount { get; set; }

        [RegularExpression("^\\d{10}$")]
        public string CustomerPhoneNumber { get; set; }
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public string CustomerEmailId { get; set; }
        public AccountType CustomerAccountType { get; set; }
        public string CustomerAddress { get; set; }
        [RegularExpression(@"^(0[1-9]|[1-2][0-9]|3[0-1])/(0[1-9]|1[0-2])/(\d{4})$")]
        public string CustomerDateOfBirth { get; set; }
        public Gender CustomerGender { get; set; }
        public DateTime CustomerPassbookIssueDate { get; set; }
        public List<CustomerTransaction> Transactions { get; set; }

        public override string ToString()
        {
            return $"\nAccountID: {CustomerAccountID}  Name:{CustomerName}  Avl.Bal:{CustomerAmount}  PhoneNumber:{CustomerPhoneNumber}\nEmailId:{CustomerEmailId}  AccountType:{CustomerAccountType}  Address:{CustomerAddress}  DateOfBirth:{CustomerDateOfBirth}\nGender:{CustomerGender}  PassbookIssueDate:{CustomerPassbookIssueDate}\n";
        }
    }
}
