using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RealTimeTasks.Data;
using RealTimeTasks.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealTimeTasks.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly string _connectionString;
        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        [HttpPost]
        [Route("Signup")]
        public void Signup(SignupViewModel vm)
        {
            var repo = new UserRepository(_connectionString);
            repo.AddUser(vm, vm.Password);
        }

        [HttpPost]
        [Route("Login")]
        public User Login(LoginViewModel vm)
        {
            var repo = new UserRepository(_connectionString);
            var user = repo.Login(vm.Email, vm.Password);
            if (user == null)
            {
                return null;
            }
            var claims = new List<Claim>
            {
                new Claim("user", vm.Email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return user;
        }

        [HttpGet]
        [Route("getcurrentuser")]
        public User GetCurrentUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }
            var repo = new UserRepository(_connectionString);
            return repo.GetByEmail(User.Identity.Name);
        }

        [HttpGet]
        [Route("logout")]
        public void Logout()
        {
            HttpContext.SignOutAsync().Wait();
        }
        [HttpGet]
        [Route("getNameOfUser")]
        public UserNameModel GetNameOfUserById(int id)
        {
            var repo = new TasksRepository(_connectionString);
            var u = repo.GetUserById(id);
            return new UserNameModel() { Name = $"{u.FirstName} {u.LastName}" };

        }
    }
}
