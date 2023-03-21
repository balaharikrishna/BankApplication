using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationModels
{
    public class BranchManager
    {
        [RegularExpression("^[a-zA-Z]+$")]
        public string BranchManagerName { get; set; }
        public string BranchManagerAccountId { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]
        public string BranchMangerPassword { get; set; }
    }
}
