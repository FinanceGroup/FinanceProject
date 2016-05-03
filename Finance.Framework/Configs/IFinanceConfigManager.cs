using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Framework.Configs
{
    public interface IFinanceConfigManager : IDependency
    {
        int Port { get; set; }
        string SiteUrl { get; set; }
    }
}
