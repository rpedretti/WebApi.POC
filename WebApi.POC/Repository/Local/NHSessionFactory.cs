using NHibernate;
using NHibernate.Cfg;
using WebApi.POC.Repository.Local.Mapping;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace WebApi.POC.Repository.Local
{
    /// <summary>
    /// Database Session Factory for NHibernate
    /// </summary>
    public class NHSessionFactory
    {
        /// <summary>
        /// Gets the factory configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public Configuration Configuration { get; }

        /// <summary>
        /// Gets the session factory.
        /// </summary>
        /// <value>
        /// The session factory.
        /// </value>
        public ISessionFactory SessionFactory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NHSessionFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public NHSessionFactory(string connectionString)
        {
            Configuration = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFileWithPassword("local.db", "pwd1234"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .BuildConfiguration();

            var validator = new SchemaValidator(Configuration);
            try
            {
                validator.Validate();
            }
            catch (SchemaValidationException)
            {
                var exporter = new SchemaExport(Configuration);
                exporter.Execute(true, true, false);
            }

            Configuration.SessionFactory().GenerateStatistics();
            SessionFactory = Configuration.BuildSessionFactory();
        }

        /// <summary>
        /// Set the session to shows the generated SQL.
        /// </summary>
        /// <param name="show">if set to <c>true</c> logs the generated SQL.</param>
        public void ShowSqlLog(bool show)
        {
            Configuration.SetProperty(Environment.ShowSql, show.ToString().ToLower());
            Configuration.SessionFactory().GenerateStatistics();
            SessionFactory = Configuration.BuildSessionFactory();
        }
    }
}
