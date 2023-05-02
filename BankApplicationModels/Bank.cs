using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    [Table("Banks")]
    public class Bank
    {
        [Key]
        [Required]
        [StringLength(12)]
        [Column(TypeName = "varchar")]
        public string BankId { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$")]
        [StringLength(30)]
        [Column(TypeName = "varchar")]
        public string BankName { get; set; }

        [Required]
        [RegularExpression("^[01]+$")]
        public bool IsActive { get; set; }

        public virtual ICollection<Branch> Branches { get; set; }
        public virtual ICollection<Currency> Currencies { get; set; }
        public virtual ICollection<HeadManager> HeadManagers { get; set; }
    }
}
