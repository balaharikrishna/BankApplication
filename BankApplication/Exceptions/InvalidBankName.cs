using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication.Models.Exceptions
{
    internal class InvalidBankName : Exception
    {
        public string BankName { get; }
        public InvalidBankName(string input):base($"Entered '{input}'is Invalid., Only Charecters are Allowed ex:Name")
        {
            this.BankName = input;
        }
    }
}
