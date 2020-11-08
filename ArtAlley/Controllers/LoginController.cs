using ArtAlley.Data.Entities;
using ArtAlley.Data.Repositories;
using ArtAlley.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Controllers
{
    [Route("/login")]
    public class LoginController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public LoginController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel model)
        {
            var user = mapper.Map<User>(model);
            var entity = userRepository.Login(user);
            if (entity != null)
            {
                await AuthenticateAsync(entity);
                return RedirectToAction("Index", "Admin");
            }
            return View(model);
        }

        private async Task AuthenticateAsync(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
