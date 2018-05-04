using NHibernate;
using NHibernate.Linq;
using System.Threading.Tasks;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local
{
    /// <summary>
    /// Class to access a local Database to handle user requests
    /// </summary>
    public class UserRepository : BaseNHContext, IUserRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="session">A reference to the database context</param>
        public UserRepository(ISession session) : base(session)
        {
        }

        /// <summary>
        /// Searches for a user at the repository
        /// </summary>
        /// <param name="username">The username to look for</param>
        /// <returns>Returns a task wich result yields a User</returns>
        public async Task<User> GetUserAsync(string username)
        {
            return await WithSessionAsync(async s => await s.Query<User>().FirstAsync(u => u.Username == username));
        }
    }
}
