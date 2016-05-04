using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.DAL.Records
{
    //[DataServiceKey("Id")]
    public abstract class BaseRecord
    {
        public virtual string Id { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string CreatedBy { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string CreatedByDisplayName { get; set; }
        [Required]
        public virtual DateTime CreatedTime { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string UpdatedBy { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string UpdatedByDisplayName { get; set; }
        [Required]
        public virtual DateTime UpdatedTime { get; set; }
    }
}
