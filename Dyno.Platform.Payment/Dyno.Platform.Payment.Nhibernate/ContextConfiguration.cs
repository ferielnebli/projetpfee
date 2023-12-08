using Microsoft.Extensions.DependencyInjection;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using Platform.Shared.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyno.Platform.Payment.Nhibernate
{
    public static class ContextConfiguration
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string? connectionString)
        {


            var configuration = new Configuration();

            var mapper = new ModelMapper();
            mapper.AddMapping<QRCodeMap>();
            mapper.AddMapping<TransactionMap>();
            mapper.AddMapping<WalletMap>();

            configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());




            configuration.DataBaseIntegration(c =>
            {

                c.Driver<NpgsqlDriver>();
                c.Dialect<PostgreSQL83Dialect>();
                c.ConnectionProvider<DriverConnectionProvider>();
                c.ConnectionString = connectionString;
                c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                c.SchemaAction = SchemaAutoAction.Validate;
                c.LogFormattedSql = true;
                c.LogSqlInConsole = true;
            });
            var export = new SchemaUpdate(configuration);
            export.Execute(false, true);

            var sessionFactory = configuration.BuildSessionFactory();

            services.AddSingleton(sessionFactory);
            services.AddScoped(factory =>
            sessionFactory.OpenSession());
            services.AddScoped(typeof(IMapperSession<>), typeof(MapperSession<>));
            return services;
        }
    }

}
