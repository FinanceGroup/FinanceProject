using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.DAL.Records
{
    public class CurrencyRecord : BaseRecord
    {
        public CurrencyRecord()
        {
            Id = Guid.NewGuid().ToString();
        }
        [Required]
        [StringLength(50)]
        public virtual string CurrencyCode { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string CurrencyName { get; set; }
        [Required]
        public virtual decimal AccountingRates { get; set; }
        [Required]
        public virtual int DecimalPlaces { get; set; }
        [Required]
        public virtual bool IsActive { get; set; }
    }
}
