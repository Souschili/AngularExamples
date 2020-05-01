using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.Api.Configuration;
using WebApp.Api.Models;
using WebApp.Api.Services;

namespace WebApp.Api.Utillits
{
    public class TokenGenerator : ITokenGenerator
    {
        //инжектим файл с настройками он приватный и неизменяемый (настройки неменяются и нам ненадо)
        private readonly IOptions<JwtOptions> options;

        public TokenGenerator(IOptions<JwtOptions> opt)
        {
            options = opt;
        }

        public async Task<string> GenerateJwtTokenAsync(MockUser user)
        {
            //ключ щифрования токена
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Secret));
            var algoritm = SecurityAlgorithms.HmacSha256;
            // подпись токена 
            var credentials = new SigningCredentials(key, algoritm);

            // нужен пакет Identity.Model.Jwt 
            //клаймы юзера в токене (тело токена)
            var claims = new[]
            {
                //кастомный клайм для хранения айди
                new Claim("ID",user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.UserRole),
                //new Claim("Role",user.UserRole),
                new Claim(JwtRegisteredClaimNames.Email,user.Login),
                new Claim(JwtRegisteredClaimNames.Nbf,DateTime.Now.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,DateTime.Now.AddHours(options.Value.ExpireTime).ToString()),
            };

            //создаем токен
            var securityToken = new JwtSecurityToken(
                issuer: options.Value.Issuer,
                audience: options.Value.Audience,
                claims:claims,
                notBefore:DateTime.Now,
                expires:DateTime.Now.AddHours(options.Value.ExpireTime),
                signingCredentials:credentials
                );


            return await Task.Run(()=> new JwtSecurityTokenHandler().WriteToken(securityToken));
        }
    }
}
