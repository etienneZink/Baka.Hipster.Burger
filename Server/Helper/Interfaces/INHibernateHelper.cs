using NHibernate;

namespace Baka.Hipster.Burger.Server.Helper.Interfaces
{
    public interface INHibernateHelper
    {
        public ISession OpenSession();
    }
}