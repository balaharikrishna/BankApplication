using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class Bank
    {
        [RegularExpression("^[a-zA-Z]+$")]
        public string BankName { get; set; }
        public string BankId { get; set; }
        public List<BankBranch> Branches { get; set; }
        public List<BankHeadManager> HeadManagers { get; set; }
        public List<Currency> Currency { get; set; }


    }
}
