using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.POC.Domain;

namespace WebApi.POC.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string username);
    }
}
