using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Finance.Standlone.Conventions
{
    public class ClassNameConvention : IClassConvention
    {
        public virtual void Apply(IClassInstance instance)
        {
            var tableName = instance.EntityType.Name.Replace("Record", "");
            instance.Table(tableName);
        }
    }
}