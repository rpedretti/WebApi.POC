using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local
{
    /// <summary>
    /// Class to access a local Database to handle user requests
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly PocDbContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">A reference to the database context</param>
        public UserRepository(PocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Searches for a user at the repository
        /// </summary>
        /// <param name="username">The username to look for</param>
        /// <returns>Returns a task wich result yields a User</returns>
        public async Task<User> GetUserAsync(string username)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
