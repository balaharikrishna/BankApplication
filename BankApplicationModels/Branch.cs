using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class Branch
    {
        [RegularExpression("^[a-zA-Z]+$")]
        public string BranchName { get; set; }
        public string BranchId { get; set; }
        public string BranchAddress { get; set; }

        [RegularExpression("^\\d{10}$")]
        public string BranchPhoneNumber { get; set; }
        [RegularExpression("^[01]+$")]
        public ushort IsActive { get; set; }
        public List<Manager> Managers { get; set; }
        public List<TransactionCharges> Charges { get; set; }
        public List<Staff> Staffs { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
