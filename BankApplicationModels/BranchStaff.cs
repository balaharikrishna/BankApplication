using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class BranchStaff
    {
        [RegularExpression("^[a-zA-Z]+$")]
        public string StaffName { get; set; }
        public string StaffAccountId { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string StaffPassword { get; set; }
        public StaffRole Role { get; set; }
    }
}
