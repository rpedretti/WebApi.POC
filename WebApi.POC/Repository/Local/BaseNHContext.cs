using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.POC.Repository.Local
{
    /// <summary>
    /// Class to be used as base for acessing the NHibernate session
    /// </summary>
    public abstract class BaseNHContext
    {
        private ISession _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseNHContext"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public BaseNHContext(ISession session)
        {
            _session = session;
        }

        /// <summary>
        /// Runs the <paramref name="action"/> within a context with auto commit
        /// </summary>
        /// <typeparam name="T">The response type of the action</typeparam>
        /// <param name="action">The action to be executed.</param>
        /// <returns></returns>
        public async Task<T> WithAutoTransaction<T>(Func<ISession, Task<T>> action)
        {
            using (var transaction = _session.BeginTransaction())
            {
                var result = await action(_session);
                transaction.Commit();
                return result;
            }
        }

        /// <summary>
        /// Runs the <paramref name="action"/> within a context with auto commit
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        /// <returns></returns>
        public async Task WithAutoTransaction(Func<ISession, Task> action)
        {
            using (var transaction = _session.BeginTransaction())
            {
                await action(_session);
                transaction.Commit();
            }
        }

        /// <summary>
        /// Runs the <paramref name="action"/> within a session asynchronously
        /// </summary>
        /// <typeparam name="T">The response type of the action</typeparam>
        /// <param name="action">The action to be executed.</param>
        /// <returns></returns>
        public async Task<T> WithSessionAsync<T>(Func<ISession, Task<T>> action)
        {
            return await action(_session);
        }

        /// <summary>
        /// Runs the <paramref name="action"/> within a session
        /// </summary>
        /// <typeparam name="T">The response type of the action</typeparam>
        /// <param name="action">The action to be executed.</param>
        /// <returns></returns>
        public T WithSession<T>(Func<ISession, T> action)
        {
            return action(_session);
        }

        /// <summary>
        /// Gets a query with read-only entities.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <returns>A IQueryable of the especified type</returns>
        public IQueryable<T> Read<T>()
        {
            return _session.Query<T>().WithOptions(o =>
            {
                o.SetReadOnly(true);
            });
        }
    }
}
