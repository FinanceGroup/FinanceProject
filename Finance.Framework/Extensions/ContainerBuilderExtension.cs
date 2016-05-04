using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Framework.Extensions
{
    public static class ContainerBuilderExtension
    {
        public static void RegisterDependencies(this ContainerBuilder builder, IList<Assembly> assemblies)
        {
            if (null == assemblies)
            {
                return;
            }

            var types = new List<Type>();

            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.ExportedTypes.Where(o => typeof(IDependency).IsAssignableFrom(o)));
            }

            foreach (var type in types)
            {
                var registration = builder.RegisterType(type)
                    .InstancePerLifetimeScope();

                var interfaceTypes = type.GetInterfaces();
                if (interfaceTypes != null && interfaceTypes.Length >0)
                {
                    foreach (var interfaceType in interfaceTypes.Where(o=> typeof(IDependency).IsAssignableFrom(o)))
                    {
                        registration = registration.As(interfaceType);
                        if (typeof(ISingletonDependency).IsAssignableFrom(interfaceType))
                        {
                            registration.SingleInstance();
                        }
                        else if (typeof(IUnitOfWorkDependency).IsAssignableFrom(interfaceType))
                        {
                            registration.InstancePerMatchingLifetimeScope();
                        }
                        else if (typeof(ITransientDependency).IsAssignableFrom(interfaceType))
                        {
                            registration.InstancePerDependency();
                        }
                        else
                        {
                            //Nothing to do.
                        }
                    }
                }
            }
        }
    }
}
