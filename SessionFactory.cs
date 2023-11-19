using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Data.SqlClient;

namespace Mappings
{
    public class SessionFactory
    {
        public static ISessionFactory CreateSessionFactory()
        {
            var builder = new SqlConnectionStringBuilder();
            if (System.Environment.GetEnvironmentVariables().Contains("RetailStore.Server"))
            {
                builder["server"] = System.Environment.GetEnvironmentVariable("RetailStore.Server");
            }
            else
            {
                builder["server"] = "(LocalDB)\\MSSQLLocalDB";
            }

            builder["Integrated Security"] = false;
            builder["Initial Catalog"] = System.Environment.GetEnvironmentVariable("RetailStore.DB");
            builder["User Id"] = System.Environment.GetEnvironmentVariable("RetailStore.UID");
            builder["Password"] = System.Environment.GetEnvironmentVariable("RetailStore.PW");

            return Fluently.Configure()
              .Database(
                MsSqlConfiguration.MsSql2012
                  .ConnectionString(builder.ConnectionString)
                  .DefaultSchema("RetailStore")
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<EmployeeMap>())
                .ExposeConfiguration(x => x.CurrentSessionContext<ThreadLocalSessionContext>())
                .BuildSessionFactory();
        }
    }
}
