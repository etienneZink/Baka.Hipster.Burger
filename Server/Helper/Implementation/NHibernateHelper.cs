using System.CodeDom;
using System.Reflection;
using System.Security.Policy;
using Baka.Hipster.Burger.Server.Helper.Interfaces;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Baka.Hipster.Burger.Server.Helper.Implementation
{
    public class NHibernateHelper: INHibernateHelper
    {
        private static ISessionFactory _mSessionFactory;
        public static string DatabaseFile;

        private ISessionFactory SessionFactory
        {
            get
            {
                if (_mSessionFactory == null) InitializeFactory();
                return _mSessionFactory;
            }
        }

        public ISession OpenSession() => SessionFactory.OpenSession();

        private void InitializeFactory()
        {
            _mSessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(DatabaseFile)) //ohne .ShowSql() da dies verwirrende Outputs in das Terminal printed, während der Benutzung
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())
                    .Conventions.Add(FluentNHibernate.Conventions.Helpers.DefaultLazy.Never()))
                .BuildSessionFactory();
        }
    }
}