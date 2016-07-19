using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Contract.Requests
{
    public class SaveCurrencyRequest
    {
        public string currencyCode { get; set; }
        public string currencyName { get; set; }
        public decimal AccountingRates { get; set; }
        public int decimalPlaces { get; set; }
        public string userName { get; set; }
    }
}
