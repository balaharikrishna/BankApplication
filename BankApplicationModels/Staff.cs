using BankApplicationModels.Enums;

namespace BankApplicationModels
{
    public class Staff : HeadManager
    {
        public StaffRole Role { get; set; }
    }
}
