using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class HeadManager
    {
        [RegularExpression("^[a-zA-Z]+$")]
        public string Name { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string Password { get; set; }
        public string AccountId { get; set; }

        [RegularExpression("^[01]+$")]
        public ushort IsActive { get; set; }
    }
}
