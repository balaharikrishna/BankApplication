using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class Staff : HeadManager
    {
        [Required]
        public StaffRole Role { get; set; }
    }
}
