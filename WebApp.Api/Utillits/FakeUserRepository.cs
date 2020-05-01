using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Api.Models;
using WebApp.Api.Services;

namespace WebApp.Api.Utillits
{
    public class FakeUserRepository : IUserRepository
    {
        public IEnumerable<MockUser> Users =>
            new List<MockUser> {
            new MockUser{Login="Samurai",Password="123456",UserRole="Admin"},
            new MockUser{Login="Killer",Password="1234556",UserRole="User"},
            new MockUser{Login="Samuil",Password="1141123456",UserRole="User"},
            new MockUser{Login="Killer",Password="123456",UserRole="User"},
            new MockUser{Login="Sauron",Password="123456",UserRole="User"},
            };

        public async Task<MockUser> GetUser(string login)
        {
            var user = await Task.Run(() => Users.FirstOrDefault(x=> login==x.Login));
            return user;
        }
    }
}
