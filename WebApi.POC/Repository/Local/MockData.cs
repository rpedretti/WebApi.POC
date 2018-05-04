using NHibernate;
using NHibernate.Linq;
using System;
using System.Threading.Tasks;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository.Local
{
    /// <summary>
    /// Class to generate mock data for debugging
    /// </summary>
    public static class MockData
    {
        /// <summary>
        /// Seeds the database with mock data
        /// </summary>
        /// <param name="session"></param>
        public static async Task Initialize(ISession session)
        {
            using (var transaction = session.BeginTransaction())
            {
                if (await session.Query<User>().AnyAsync())
                {
                    return;
                }

                var users = new User[]
                {
                    new User {
                        Id = 1,
                        Username = "fulano",
                        Password = "55ED885708721EDD3B5575988EFC21103F4194D18F685D0F76147E26E1E17CE3",
                        Role = Role.USER
                    },
                    new User {
                        Id = 0,
                        Username = "admin",
                        Password = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918",
                        Role = Role.ADMIN
                    }
                };

                foreach (var user in users)
                {
                    await session.SaveAsync(user);
                }

                await session.FlushAsync();

                var demands = new ServiceDemand[]
                {
                new ServiceDemand
                {
                    CreatedAt = DateTime.Now,
                    Description = "Service 1",
                    LastEdit = DateTime.Now,
                    Status = Status.CREATED,
                    Owner = users[0]
                },
                new ServiceDemand
                {
                    CreatedAt = DateTime.Now,
                    Description = "Service 2",
                    LastEdit = DateTime.Now,
                    Status = Status.CREATED,
                    Owner = users[0]
                },
                new ServiceDemand
                {
                    CreatedAt = DateTime.Now,
                    Description = "Service 3",
                    LastEdit = DateTime.Now,
                    Status = Status.IN_ANALISYS,
                    Owner = users[1]
                },
                new ServiceDemand
                {
                    CreatedAt = DateTime.Now,
                    Description = "Service 4",
                    LastEdit = DateTime.Now,
                    Status = Status.IN_PROGRESS,
                    Owner = users[1]
                }
                };

                foreach (var demand in demands)
                {
                    await session.SaveAsync(demand);
                }

                await session.FlushAsync();

                await transaction.CommitAsync();
            }
        }
    }
}
