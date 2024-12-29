//using AutoMapper.Configuration;
using Lesson21_API_.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lesson21_API_.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;

        public JwtService(IConfiguration config,UserManager<AppUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }
        public string Generate(IList<string> roles, AppUser admin)
        {
            //token generation 
            //sonra tokeni qaytar
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, admin.Id));
            claims.Add(new Claim(ClaimTypes.Name, admin.UserName));
            claims.Add(new Claim("FullName", admin.FullName));

            //var roles = await _userManager.GetRolesAsync(admin);

            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList());
            //foreach (var item in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role,item));
            //}
            string secretKey = _config.GetSection("JWT:secret").Value;
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));//security codumuzu yaradiriq=secretKey keyi encode edirik

            SigningCredentials creds = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);//Encode olunmus secret keyimizle SigningCredentials ya    //headerim olusmus olacaq

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
            (
                    signingCredentials: creds,
                    claims: claims,
                    issuer: _config.GetSection("JWT:issuer").Value,//projecte aid spesifik bir sey yerlesdiririk generally url
                    audience: _config.GetSection("JWT:audience").Value,
                    expires: DateTime.Now.AddDays(3)
            );//string formasinda deil
            string tokenStr = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return tokenStr;
        }
    }
}
