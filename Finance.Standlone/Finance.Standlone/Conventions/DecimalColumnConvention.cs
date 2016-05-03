using System.Linq;
using System.Reflection;
using Finance.DAL.Attributes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Finance.Standlone.Conventions
{
    public class DecimalColumnConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Type == typeof(decimal));
        }

        public void Apply(IPropertyInstance instance)
        {
            MemberInfo[] myMemberInfos = ((PropertyInstance)(instance)).EntityType.GetMember(instance.Name);

            if (myMemberInfos.Length > 0)
            {
                object[] myCustomAttrs = myMemberInfos[0].GetCustomAttributes(false);

                if (myCustomAttrs.Length > 0)
                {
                    var decimalFormat = (DecimalFormatAttribute)myCustomAttrs.FirstOrDefault(x => x is DecimalFormatAttribute);

                    if (decimalFormat != null)
                    {
                        instance.Precision(decimalFormat.Precision);
                        instance.Scale(decimalFormat.Scale);
                    }
                }
            }
        }
    }
}