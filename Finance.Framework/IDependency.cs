using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Framework
{
    public interface IDependency { }
    public interface ISingletonDependency : IDependency { }
    public interface ITransientDependency : IDependency { }
    public interface IUnitOfWorkDependency : IDependency { }
}
