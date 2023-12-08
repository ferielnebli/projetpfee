using Dyno.Platform.ReferentialData.BusinessModel.UserRole;
using Dyno.Platform.ReferntialData.DataModel.UserRole;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.ReferentialData.Nhibernate.DefaultData
{
    public class DefaultRole
    {
        private readonly ISessionFactory _sessionFactory;

        public DefaultRole(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void InitializeDefaultRoles()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var roleAdmin = new RoleEntity
                {
                    Id = "8336c464-b380-41db-be29-5dc6ec966362",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                };
                session.Save(roleAdmin);
                var roleClient = new RoleEntity
                {
                    Id = "8336c554-b380-41db-be29-5dc6ec966362",
                    Name = "Client",
                    NormalizedName = "CLIENT"
                };
                session.Save(roleClient);

                transaction.Commit();
            }
        }
    }
}
