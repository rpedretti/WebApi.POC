using System;
using System.Linq;
using WebApi.Shared.Domain;

namespace WebApi.POC.Repository
{
    public static class MockData
    {
        public static void Initialize(PocDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            var users = new User[]
            {
                new User {
                    Id = 1,
                    Username = "fulano",
                    Password = "55ED885708721EDD3B5575988EFC21103F4194D18F685D0F76147E26E1E17CE3",
                    RoleId = Role.USER.Id
                },
                new User {
                    Id = 0,
                    Username = "admin",
                    Password = "8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918",
                    RoleId = Role.ADMIN.Id
                }
            };
            context.AddRange(users);
            context.SaveChanges();

            var demands = new ServiceDemand[]
            {
                new ServiceDemand
                {
                    CreatedAt = DateTime.Now,
                    Description = "Service 1",
                    LastEdit = DateTime.Now,
                    StatusId = Status.CREATED.Id,
                    OwnerId = users[0].Id
                },
                new ServiceDemand
                {
                    CreatedAt = DateTime.Now,
                    Description = "Service 2",
                    LastEdit = DateTime.Now,
                    StatusId = Status.CREATED.Id,
                    OwnerId = users[0].Id
                },
                new ServiceDemand
                {
                    CreatedAt = DateTime.Now,
                    Description = "Service 3",
                    LastEdit = DateTime.Now,
                    StatusId = Status.IN_ANALISYS.Id,
                    OwnerId = users[1].Id
                },
                new ServiceDemand
                {
                    CreatedAt = DateTime.Now,
                    Description = "Service 4",
                    LastEdit = DateTime.Now,
                    StatusId = Status.IN_PROGRESS.Id,
                    OwnerId = users[1].Id
                }
            };

            context.AddRange(demands);
            context.SaveChanges();
        }
    }
}
