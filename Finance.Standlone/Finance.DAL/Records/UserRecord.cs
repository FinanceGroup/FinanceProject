using System.ComponentModel.DataAnnotations;

namespace Finance.DAL.Records
{
    public class UserRecord : BaseRecord
    {
        [Required]
        [StringLength(50)]
        public virtual string UserName { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string Password { get; set; }
        [Required]
        public virtual bool IsActive { get; set; }
    }
}
