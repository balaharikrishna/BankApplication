using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    [Table("HeadManagers")]
    public class HeadManager : User
    {
        [Required]
        [StringLength(12)]
        [Column(TypeName = "varchar")]
        [ForeignKey("Bank")]
        public string BankId { get; set; }
        public virtual Bank Bank { get; set; }
    }
}
