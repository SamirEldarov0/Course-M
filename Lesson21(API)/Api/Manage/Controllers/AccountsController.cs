//using AutoMapper.Configuration;
using Lesson21_API_.Api.Manage.Dtos.AccountDtos;
using Lesson21_API_.Data.Entities;
using Lesson21_API_.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lesson21_API_.Api.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IJwtService _jwtService;

        public AccountsController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager,IConfiguration config,IJwtService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;//using Microsoft.Extensions.Configuration;
            _jwtService = jwtService;
        }

        [HttpGet("role")]
        public async Task<IActionResult> AddRoleAsync()
        {
            AppUser appUser =await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == "samir.eldarov");
            await _userManager.AddToRoleAsync(appUser, "SuperAdmin");
            return Ok(appUser);
        }

        [HttpGet("test")]
        public async Task<IActionResult> CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole("Member"));
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            
            AppUser appUser = new AppUser()
            {
                UserName = "samir.eldarov",
                FullName = "Samir Eldarov"   
            };
          
            await _userManager.CreateAsync(appUser, "Samir2003");
            await _userManager.AddToRoleAsync(appUser, "Admin");
            return Ok(appUser);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginDto adminLoginDto)
        {
            AppUser admin =await _userManager.FindByNameAsync(adminLoginDto.UserName);
            if (admin==null)
            {
                return NotFound();
            }
            if (!await _userManager.CheckPasswordAsync(admin,adminLoginDto.Password))
            {
                return Unauthorized();
            }

            string token = _jwtService.Generate(_userManager.GetRolesAsync(admin).Result, admin);
            return Ok(token);
            return StatusCode(200,token);
        }


        //[Authorize(Roles ="SuperAdmin")]
        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync()
        {
            AppUser appUser =await _userManager.FindByNameAsync(User.Identity.Name);
            return Ok(
            new
            {
                UserName=User.Identity.Name,
                FullName=User.FindFirst("FullName").Value
            });
            
        }
    }
}
