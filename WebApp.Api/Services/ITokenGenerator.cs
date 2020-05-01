using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Api.Models;

namespace WebApp.Api.Services
{
    // интерфейс генератора токена
    public interface ITokenGenerator
    {
        Task<string> GenerateJwtTokenAsync(MockUser user);
    }
}
