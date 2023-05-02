using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    [Table("Staffs")]
    public class Staff : User
    {
        [Required]
        [StringLength(18)]
        [Column(TypeName = "varchar")]
        public string BranchId { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
    }
}
