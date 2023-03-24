using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication.Exceptions
{
    internal class InvalidOptionException : Exception
    {
        public ushort InvalidKey { get; set; }
        public InvalidOptionException(ushort invalidption) : base($"Entered '{invalidption}'is Invalid Please Enter the Appropriate Option.")
        {
            InvalidKey = invalidption;
        }

    }

}
