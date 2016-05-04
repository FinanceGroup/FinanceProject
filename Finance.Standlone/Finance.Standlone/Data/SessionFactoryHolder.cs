using System;
using System.Reflection;

using Finance.DAL.Records;
using Finance.Framework.Data;
using Finance.Framework.Data.Conventions;
using Finance.Framework.Logging;
using Finance.Standlone.Conventions;

using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace CustomerManagementFrontEnd
{
    public class SessionFactoryHolder : ISessionFactoryHolder, IDisposable
    {
        private ISessionFactory _sessionFactory;

        public SessionFactoryHolder()
        {
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public ISessionFactory GetSessionFactory()
        {
            lock (this)
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = BuildSessionFactory();
                }
            }
            return _sessionFactory;
        }

        private ISessionFactory BuildSessionFactory()
        {
            Logger.Debug("Building session factory");

            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                    //.ConnectionString(c => c.FromConnectionStringWithKey("ConnectionString"))
                    .ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection"))
                    .ShowSql())
                .Mappings(m => m.AutoMappings.Add(CreateAutomappings()))
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                .BuildSessionFactory();

            Logger.Debug("Done building session factory");

            return sessionFactory;
        }

        private static AutoPersistenceModel CreateAutomappings()
        {
            return AutoMap.Assembly(Assembly.GetAssembly(typeof(BaseRecord)), new AutomappingConfiguration())
                .Conventions.Setup(conventions =>
                {
                    //conventions.Add<ClassNameConvention>();
                    conventions.Add<IdConvention>();
                    conventions.Add<StringColumnLengthConvention>();
                    //conventions.Add<ColumnNullableConvention>();
                    conventions.Add<DecimalColumnConvention>();
                    conventions.Add<UtcDateTimeConvention>();
                    conventions.Add<BinaryColumnLengthConvention>();
                });
        }

        public class AutomappingConfiguration : DefaultAutomappingConfiguration
        {
            public override bool ShouldMap(Type type)
            {
                return type.IsSubclassOf(typeof(BaseRecord)) || type.IsSubclassOf(typeof(BaseView));
            }
        }

        public void Dispose()
        {
            if (_sessionFactory != null)
            {
                _sessionFactory.Dispose();
                _sessionFactory = null;
            }
        }
    }
}