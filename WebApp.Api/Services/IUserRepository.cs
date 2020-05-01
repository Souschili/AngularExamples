using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Api.Models;

namespace WebApp.Api.Services
{
    public interface IUserRepository
    {
        IEnumerable<MockUser> Users { get; }
        Task<MockUser> GetUser(string login);
    }
}
