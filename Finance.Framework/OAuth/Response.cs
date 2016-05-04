using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Framework.OAuth
{
    public class Response
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public object Output { get; set; }
    }
}
