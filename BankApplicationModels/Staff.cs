using BankApplicationModels.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplicationModels
{
    [Table("Staffs")]
    public class Staff : HeadManager
    {
        [Required]
        [Range(1, 5)]
        [Column(TypeName = "Smallint")]
        public new Roles Role = Roles.Staff;
    }
}
