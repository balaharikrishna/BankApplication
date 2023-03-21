using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class BankHeadManager
    {
        [RegularExpression("^[a-zA-Z]+$")]
        public string HeadManagerName { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string HeadManagerPassword { get; set; }
        public string HeadManagerAccountId { get; set; }
    }
}
