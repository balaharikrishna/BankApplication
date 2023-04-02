using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class Bank
    {
        [RegularExpression("^[a-zA-Z]+$")]
        public string BankName { get; set; }
        public string BankId { get; set; }
        [RegularExpression("^[01]+$")]
        public ushort IsActive { get; set; }    
        public List<Branch> Branches { get; set; }
        public List<HeadManager> HeadManagers { get; set; }
        public List<Currency> Currency { get; set; }
    }
}
