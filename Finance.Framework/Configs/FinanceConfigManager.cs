using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Framework.Configs
{
    public class FinanceConfigManager : IFinanceConfigManager
    {
        public int Port { get
            { return 8000; } set { } }
        public string SiteUrl
        {
            get
            { return "http://localhost:8000"; }
            set { }
        }
    }
}
