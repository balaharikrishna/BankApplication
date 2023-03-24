using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication.Exceptions
{
    internal class InvalidUser : Exception
    {
        public string userName { get; }
        public InvalidUser(string userName)
        {
            this.userName = userName;
        }
    }
}
