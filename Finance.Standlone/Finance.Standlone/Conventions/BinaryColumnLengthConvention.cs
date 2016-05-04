using System;

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Finance.Standlone.Conventions
{
    public class BinaryColumnLengthConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Type == typeof(byte[]));
        }

        public void Apply(IPropertyInstance instance)
        {
            instance.Length(Int32.MaxValue);
            instance.CustomSqlType("varbinary(MAX)");
        }
    }
}