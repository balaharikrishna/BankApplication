using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    [Table("Staffs")]
    public class Staff : User
    {
        [Required]
        [StringLength(17)]
        [Column(TypeName = "varchar")]
        [ForeignKey("Branch")]
        public string BranchId { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
