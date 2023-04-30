using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    [Table("Managers")]
    public class Manager : User
    {
        [Required]
        [StringLength(17)]
        [Column(TypeName = "varchar")]
        [ForeignKey("Branch")]
        public string BranchId { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
