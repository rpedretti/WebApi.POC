using NHibernate;
using NHibernate.Cfg;
using WebApi.POC.Repository.Local.Mapping;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using WebApi.POC.Repository.Interfaces;

namespace WebApi.POC.Repository.Local
{
    public class NHSessionFactory : IDbSessionFactory
    {
        public Configuration Configuration { get; }
        public ISessionFactory SessionFactory { get; private set; }

        public NHSessionFactory(string connectionString)
        {
            Configuration = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile("local.db"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .BuildConfiguration();

            var validator = new SchemaValidator(Configuration);
            try
            {
                validator.Validate();
            }
            catch (SchemaValidationException sve)
            {
                var exporter = new SchemaExport(Configuration);
                exporter.Execute(true, true, false);
            }

            Configuration.SessionFactory().GenerateStatistics();
            SessionFactory = Configuration.BuildSessionFactory();
        }

        public void Validate()
        {
            new SchemaValidator(Configuration).Validate();
        }

        public void ShowSqlLog(bool show)
        {
            Configuration.SetProperty(Environment.ShowSql, show.ToString().ToLower());
            Configuration.SessionFactory().GenerateStatistics();
            SessionFactory = Configuration.BuildSessionFactory();
        }
    }
}
