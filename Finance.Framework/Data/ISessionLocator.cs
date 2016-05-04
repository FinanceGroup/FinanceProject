using System;
using NHibernate;

namespace Finance.Framework.Data {
    public interface ISessionLocator : IDependency {        
        ISession For(Type entityType);
    }
}