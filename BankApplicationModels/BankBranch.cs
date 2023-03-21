using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationModels
{
    public class BankBranch
    {
        [RegularExpression("^[a-zA-Z]+$")]
        public string BranchName { get; set; }
        public string BranchId { get; set; }
        public string BranchAddress { get; set; }
        [RegularExpression("^\\d{10}$")]
        public string BranchPhoneNumber { get; set; }
        public List<BranchManager> Managers { get; set; }
        public List<TransactionCharges> Charges { get; set; }
        public List<BranchStaff> Staffs { get; set; }
        public List<BranchCustomer> Customers { get; set; }
    }
}
