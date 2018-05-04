using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.POC.Repository.Local
{
    public abstract class BaseNHContext
    {
        private ISession _session;

        public BaseNHContext(ISession session)
        {
            _session = session;
        }

        public async Task<T> WithAutoTransaction<T>(Func<ISession, Task<T>> action)
        {
            using (var transaction = _session.BeginTransaction())
            {
                var result = await action(_session);
                transaction.Commit();
                return result;
            }
        }

        public async Task WithAutoTransaction(Func<ISession, Task> action)
        {
            using (var transaction = _session.BeginTransaction())
            {
                await action(_session);
                transaction.Commit();
            }
        }

        public async Task<T> WithSessionAsync<T>(Func<ISession, Task<T>> action)
        {
            return await action(_session);
        }

        public T WithSession<T>(Func<ISession, T> action)
        {
            return action(_session);
        }

        public IQueryable<T> Read<T>()
        {
            return _session.Query<T>().WithOptions(o =>
            {
                o.SetReadOnly(true);
            });
        }
    }
}
