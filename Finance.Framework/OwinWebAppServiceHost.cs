using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Finance.Framework.Extensions;
using Finance.Framework.Logging;
using Finance.Framework.Data;

namespace Finance.Framework
{
    public class OwinWebAppServiceHost : ServiceBase
    {
        protected virtual void StartService(IAppBuilder appBuilder)
        {
            var httpConfiguration = ConstructHttpConfiguration();

            var container = CreateContainer();

            ConfigureAuth(appBuilder, container);

            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            
            appBuilder.UseFileServer(GetOpenFileOperations());
            appBuilder.UseAutofacMiddleware(container);
            appBuilder.UseAutofacWebApi(httpConfiguration);
            appBuilder.UseWebApi(httpConfiguration);
        }

        protected virtual void ConfigureAuth(IAppBuilder appBuilder, IContainer container) { }

        protected virtual HttpConfiguration ConstructHttpConfiguration() { return null; }

        protected virtual FileServerOptions GetOpenFileOperations() { return null; }

        protected virtual void UseMiddleOwinMiddleware(IAppBuilder appBuilder) { }

        protected virtual IList<Assembly> GetRegisterAssemblies() { return null; }

        protected virtual IList<Assembly> ControllersAssemblies() { return null; }

        private IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            foreach (var item in ControllersAssemblies())
            {
                builder.RegisterApiControllers(item);
            }
            builder.RegisterModule(new LoggingModule());
            builder.RegisterModule(new DataModule());
            builder.RegisterDependencies(GetRegisterAssemblies());

            return builder.Build();
        }
    }
}
