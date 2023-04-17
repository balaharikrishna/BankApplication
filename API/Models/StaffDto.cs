using BankApplicationModels.Enums;

namespace API.Models
{
    public class StaffDto : HeadManagerDto
    {
        public StaffRole Role { get; set; }
    }
}
