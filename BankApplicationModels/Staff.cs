using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class Staff : HeadManager
    {
        public StaffRole Role { get; set; }
        //[RegularExpression("^[a-zA-Z]+$")]
        //public string StaffName { get; set; }
        //public string StaffAccountId { get; set; }

        //[Required(ErrorMessage = "Staff Password is required")]
        //[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",ErrorMessage ="Password is weak")]
        //public string StaffPassword { get; set; }

        //[RegularExpression("^[01]+$")]
        //public ushort IsActive { get; set; }
    }
}
