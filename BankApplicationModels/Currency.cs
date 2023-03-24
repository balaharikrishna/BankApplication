using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationModels
{
    public class Currency
    {
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }

        public string DefaultCurrencyCode = "INR";

        public short DefaultCurrencyExchangeRate = 1;
        [RegularExpression("^[01]+$")]
        public ushort IsDeleted { get; set; }
    }
}
