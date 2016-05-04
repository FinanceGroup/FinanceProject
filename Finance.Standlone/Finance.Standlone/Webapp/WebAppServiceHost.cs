using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Finance.DAL.Records;
using Finance.Framework;
using Finance.Framework.OAuth;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.StaticFiles;

using Owin;

namespace Finance.Standlone.Webapp
{
    public class WebAppServiceHost : OwinWebAppServiceHost
    {
        public static WebAppServiceHost Instance { get; private set; }

        public bool IsLaunched { get; private set; }

        private IDisposable _server = null;
        private System.ComponentModel.IContainer components = null;

        static WebAppServiceHost()
        {
            Instance = new WebAppServiceHost();
        }
        private WebAppServiceHost()
        {
            InitializeComponent();
            Init();
        }

        public bool TryStartApplication(string[] args)
        {
            if (IsLaunched)
            {
                return false;
            }

            OnStart(args);
            return true;
        }

        public void StopApplication()
        {
            OnStop();
        }

        protected override void OnStart(string[] args)
        {
            _server = WebApp.Start(string.Format(WebConstants.WebSiteAddress, "8000"), StartService);
        }

        protected override void OnStop()
        {
            if (_server != null)
            {
                _server.Dispose();
            }
            base.OnStop();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_server != null)
                {
                    _server.Dispose();
                }
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        protected override void ConfigureAuth(IAppBuilder appBuilder, IContainer container)
        {
            appBuilder.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString(WebConstants.TokenPath),
                Provider = container.Resolve<IApplicationOAuthServerProvider>() as OAuthAuthorizationServerProvider,
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(7),
                AllowInsecureHttp = true,
            });
            appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                AuthenticationType = WebConstants.AuthenticationType,
                AuthenticationMode = AuthenticationMode.Active,
            });
            base.ConfigureAuth(appBuilder, container);
        }

        protected override HttpConfiguration ConstructHttpConfiguration()
        {
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);

            return httpConfiguration;
        }

        protected override FileServerOptions GetOpenFileOperations()
        {
            string path = GetWebPagePath();
            var options = new FileServerOptions() { FileSystem = new PhysicalFileSystem(path), EnableDefaultFiles = true };
            options.DefaultFilesOptions.DefaultFileNames = new[] { WebConstants.DefaultPage };

            return options;
        }

        protected override void UseMiddleOwinMiddleware(IAppBuilder appBuilder)
        {
            base.UseMiddleOwinMiddleware(appBuilder);
        }

        protected override IList<Assembly> GetRegisterAssemblies()
        {
            return new List<Assembly>
            {
                Assembly.GetAssembly(typeof(IDependency)),
                Assembly.GetAssembly(typeof(BaseRecord)),
                Assembly.GetExecutingAssembly()
            };
        }

        protected override IList<Assembly> ControllersAssemblies()
        {
            return new List<Assembly>
            {
                Assembly.GetExecutingAssembly()
            };
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "Finance";
        }
        private void Init()
        {
            IsLaunched = false;
        }

        private static string GetWebPagePath()
        {
            var binDirs = new[] { "\\bin\\Debug", "\\bin\\Release", "\\bin" };
            string path = Path.Combine(Environment.CurrentDirectory, WebConstants.WebPageDirectoryName);
            path = binDirs.Aggregate(path, (current, bin) => current.Replace(bin, string.Empty));
            if (!Directory.Exists(path))
            {
                throw new Exception(string.Format(WebConstants.PathNotExist, path));
            }

            return path;
        }
    }
}
