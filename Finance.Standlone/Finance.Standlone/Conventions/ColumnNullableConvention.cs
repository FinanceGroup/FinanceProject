using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Finance.Standlone.Conventions
{
    public class ColumnNullableConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            MemberInfo[] myMemberInfos = ((PropertyInstance)(instance)).EntityType.GetMember(instance.Name);

            if (myMemberInfos.Length > 0)
            {
                object[] myCustomAttrs = myMemberInfos[0].GetCustomAttributes(false);

                if (myCustomAttrs.Length > 0)
                {
                    var requireAttr =(RequiredAttribute)myCustomAttrs.FirstOrDefault(x => x is RequiredAttribute);

                    if (requireAttr != null)
                    {
                        if (!requireAttr.AllowEmptyStrings)
                        {
                            instance.Not.Nullable();
                        }
                    }
                }
            }
        }
    }
}