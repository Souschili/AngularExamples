using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using WebApp.Api.Configuration;
using WebApp.Api.Models;
using WebApp.Api.Services;

namespace WebApp.Api.Controllers
{
    public class TestController : Controller
    {
        // DI
        private readonly IOptions<JwtOptions> options;
        private readonly IUserRepository userRepository;
        private readonly ITokenGenerator tokenGenerator;
        public TestController(IOptions<JwtOptions> opt, IUserRepository repository, ITokenGenerator token)
        {
            this.options = opt;
            userRepository = repository;
            tokenGenerator = token;
        }

        [HttpGet("token")]
        public async Task<IActionResult> IndexAsync()
        {
            var user = await userRepository.GetUser("Samurai");
            var tokenString = await tokenGenerator.GenerateJwtTokenAsync(user);
            // пока херачим в контролере , потом в сервис уберем нормальное создание временное решение
            var myToken = new TokenDTO { AcceseToken = tokenString };
            return Ok(myToken);
            //return Ok(options.Value);
        }

        [HttpGet("secret")]
        [Authorize(Roles ="Admin")]
        public IActionResult Secret()
        {
            return Ok("This is very secret page");
        }
    }
}