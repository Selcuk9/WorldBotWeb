using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InstaBotWeb.Models;
using InstaBotWeb.Models.DataBaseContext;
using InstaBotWeb.ViewsModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace InstaBotWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext db;
        public AccountController(ApplicationContext context)
        {
            db = context;
        }
        [HttpGet]
        [Authorize]
        public IActionResult AOuth(string serializedModel)
        {
            // Deserialize your model back to a list again here.
            List<InstaUser> model = null;
            try
            {
                model = JsonConvert.DeserializeObject<List<InstaUser>>(serializedModel);
            }
            catch (Exception ex){}
          
            ViewBag.UserMail = User.Identity.Name;
            return View("Profile", model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.DbUser.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if(user != null)
                {
                    await Authenticate(model.Email); // // аутентификация
                    return RedirectToAction("AOuth", "Account");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.DbUser.FirstOrDefaultAsync(u => u.Email == model.Email);

                if(user == null)
                {
                    db.DbUser.Add(new User() {Email = model.Email , Password = model.Password });
                    await db.SaveChangesAsync();
                    await Authenticate(model.Email); // аутентификация

                    return RedirectToAction("AOuth", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }
            return View(model);
        }


        /// <summary>
        /// it's needs will learn
        /// </summary>
        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}