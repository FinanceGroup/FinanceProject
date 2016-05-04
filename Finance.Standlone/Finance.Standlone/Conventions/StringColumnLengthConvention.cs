using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Finance.Standlone.Conventions
{
    public class StringColumnLengthConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => x.Type == typeof(string)).Expect(x => x.Length == 0);
        }

        public void Apply(IPropertyInstance instance)
        {
            MemberInfo[] myMemberInfos = ((PropertyInstance)(instance)).EntityType.GetMember(instance.Name);

            if (myMemberInfos.Length > 0)
            {
                object[] myCustomAttrs = myMemberInfos[0].GetCustomAttributes(false);

                if (myCustomAttrs.Length > 0)
                {
                    var lengthAttr = (StringLengthAttribute)myCustomAttrs.FirstOrDefault(x => x is StringLengthAttribute);

                    if (lengthAttr != null)
                    {
                        instance.Length(lengthAttr.MaximumLength);
                    }
                }
            }
            
        }
    }
}