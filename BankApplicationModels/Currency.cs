using System.ComponentModel.DataAnnotations;

namespace BankApplicationModels
{
    public class Currency
    {
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }

        public string DefaultCurrencyCode = "INR";

        public short DefaultCurrencyExchangeRate = 1;
        [RegularExpression("^[01]+$")]
        public ushort IsActive { get; set; }
    }
}
