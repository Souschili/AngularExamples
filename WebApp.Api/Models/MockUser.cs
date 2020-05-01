using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Api.Models
{
    // фальшивый юзер пока нет айдентитиюзера
    public class MockUser
    {
        public Guid Id { get; set; } = System.Guid.NewGuid(); // по умолчанию каждый объект имеет свой айди
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }

    }
}
