using AutoMapper;
using BL.DTOs.Auth;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.ViewModels.Auth;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _service;
        private readonly IMapper _mapper;

        public AuthController(AuthService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: AuthController/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: AuthController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            try
            {
                if(!ModelState.IsValid)
                    return View(loginVM);

                var user = _service.ValidateUser(loginVM.Username, loginVM.Password);
                if(user == null)
                {
                    ModelState.AddModelError("", "Incorrect username or password");
                    return View(loginVM);
                }

                // 1. build claims - these get encrypted into the cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.RoleTitle)
                };

                // 2. wrap in identity - scheme must match what you registered in Program.cs
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // 3. sign in - this creates the cookie and sends it to the browser
                // From this point every subsequent request from that browser will include the cookie,
                // ASP.NET will decrypt it and populate User automatically
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity)
                );

                if (user.RoleTitle == "Admin")
                    return RedirectToAction("Index", "Food");

                return RedirectToAction("Index", "Menu");
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthController/Register
        public ActionResult Register()
        {  return View();
        }

        // POST: AuthController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM registerVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(registerVM);

                _service.Register(_mapper.Map<UserRegisterDto>(registerVM));

                return RedirectToAction(nameof(Login));
            }
            catch(Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(registerVM);
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Profile()
        {
            try
            {
                var username = User.Identity.Name;
                ProfileVM profileVM = _mapper.Map<ProfileVM>(_service.GetByUsername(username));
                return View(profileVM);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

    }
}
