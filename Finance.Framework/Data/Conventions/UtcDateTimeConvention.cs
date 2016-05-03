using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NHibernate.Type;

namespace Finance.Framework.Data.Conventions
{
    public class UtcDateTimeConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Type == typeof(DateTime) || instance.Type == typeof(DateTime?))
            {
                instance.CustomType<UtcDateTimeType>();
            }
        }
    }
}
