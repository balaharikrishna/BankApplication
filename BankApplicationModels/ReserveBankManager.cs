using BankApplicationModels.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationModels
{
    [Table("ReserveBankManagers")]
    public class ReserveBankManager : HeadManager
    {
        [Required]
        [Range(1, 5)]
        [Column(TypeName = "Smallint")]
        public new Roles Role = Roles.ReserveBankManager;
    }
}
