using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.POC.Domain;

namespace WebApi.POC.Repository
{
    public sealed class MockUserRepository : IUserRepository
    {
        private static List<User> mock = new List<User>()
        {
            new User {
                Id = 1,
                Username = "fulano",
                Password = "55ED885708721EDD3B5575988EFC21103F4194D18F685D0F76147E26E1E17CE3",
                Roles = new Role[] { Role.USER }
            },
            new User {
                Id = 0,
                Username = "admin",
                Password = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918",
                Roles = new Role[] { Role.ADMIN }
            }
        };

        public Task<User> GetUserAsync(string username)
        {
            return Task.FromResult(mock.FirstOrDefault(u => u.Username == username));
        }
    }
}
