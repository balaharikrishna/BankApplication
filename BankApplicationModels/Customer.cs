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
    public class Customer : HeadManager
    {
        public decimal Balance { get; set; }

        [RegularExpression("^\\d{10}$")]
        public string PhoneNumber { get; set; }
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public string EmailId { get; set; }
        public AccountType AccountType { get; set; }
        public string Address { get; set; }
        [RegularExpression(@"^(0[1-9]|[1-2][0-9]|3[0-1])/(0[1-9]|1[0-2])/(\d{4})$")]
        public string DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public DateTime PassbookIssueDate { get; set; }
        public List<Transaction> Transactions { get; set; }

        public override string ToString()
        {
            return $"\nAccountID: {AccountId}  Name:{Name}  Avl.Bal:{Balance}  PhoneNumber:{PhoneNumber}\nEmailId:{EmailId}  AccountType:{AccountType}  Address:{Address}  DateOfBirth:{DateOfBirth}\nGender:{Gender}  PassbookIssueDate:{PassbookIssueDate}\n";
        }
    }
}
