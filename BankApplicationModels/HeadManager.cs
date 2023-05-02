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
        public string BankId { get; set; }

        [ForeignKey("BankId")]
        public virtual Bank Bank { get; set; }
    }
}
