using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using InstaBotWeb.Classes;
using InstaBotWeb.CookiesUser;
using InstaBotWeb.Models;
using InstaBotWeb.Models.DataBaseContext;
using InstaBotWeb.ViewsModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SimpleAuthorize.Crypto;

namespace InstaBotWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext db;
        private readonly IHashMethod hash;
        private readonly IWebHostEnvironment environment;
        private int idUser;

        public AccountController(ApplicationContext context, 
            IHashMethod hash, 
            IWebHostEnvironment environment)
        {
            db = context;
            this.hash = hash;
            this.environment = environment;
        }

        [HttpGet]
        [Authorize]
        public IActionResult AOuth()
        {
            idUser = ClaimUser.GetIdUser(User);
            //user = JsonConvert.DeserializeObject<User>(serializedModel);
            var userData = db.DbUsers.FirstOrDefault(ud => ud.Id == idUser);
                var dataProfile = new UserProfile()
            {
                LastName = userData.LastName,
                FirstName = userData.FirstName,
                Email = userData.Email,
                AvatarPath = userData.AvatarProfile
            };
            return View("Profile", dataProfile);
        }


        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AOuth", "Account");
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
           
            if (ModelState.IsValid)
            {
                var typeHash = MD5.Create();
                var passHash = hash.GetHashCode(typeHash, model.Password);
                User user = await db.DbUsers.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == passHash);

                if (user != null)
                {
                    string userId = user.Id.ToString();
                    await Authenticate(model.Email, userId, model.RememberMe); // аутентификация
                    return RedirectToAction("AOuth", "Account");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AOuth", "Account");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.DbUsers.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    var typeHash = MD5.Create();
                    var passHash = hash.GetHashCode(typeHash, model.Password);
                    string email = model.Email;
                    db.DbUsers.Add(new User() { Email = email,
                        Password = passHash,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        AvatarProfile = @"/img/photousers/avatar_icon.svg",
                        
                    });

                    await db.SaveChangesAsync();

                    User userReady = await db.DbUsers.FirstOrDefaultAsync(u => u.Email == model.Email);
                    string userId = userReady.Id.ToString();

                    await Authenticate(model.Email, userId,true); // аутентификация

                    return RedirectToAction("AOuth", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким адресом электронной почты уже существует");
                }
            }
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="idUser">Ид пользователя, который идентифицирует пользователя в БД</param>
        /// <param name="isPersistent">переменная состоянии сессси</param>
        /// <returns></returns>
        private async Task Authenticate(string userName, string idUser,bool isPersistent)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim("BaseId",idUser)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            var aouthProp = new AuthenticationProperties() { IsPersistent = isPersistent, AllowRefresh = true};
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id), aouthProp);
        }

        [Authorize]
        public IActionResult ChangesProfile()
        {

            return View("ChangeProfile");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangesProfile(UserProfile profile)
        {
            idUser = ClaimUser.GetIdUser(User);
            var updateUser = new InsideProfileChanges(idUser,profile);
            var path = await updateUser.ChangePhoto(environment);
            var firstname = updateUser.ChangeName();
            var lastName = updateUser.ChangeLastName();
            
            var chUser = db.DbUsers.Where(uId => uId.Id == idUser).FirstOrDefault();
            
            var newPassword = updateUser.ChangePassword(chUser.Password);

            if (firstname !="")
            {
                chUser.FirstName = firstname;
                
            }

            if (lastName != "")
            {
                chUser.LastName = lastName;
            }

            if (path != "Fail" && path != "")
            {
                chUser.AvatarProfile = @path;
            }

            if (newPassword != "" && newPassword != null && newPassword !="Fail")
            {

                chUser.Password = newPassword;
            }
            await db.SaveChangesAsync();

            return RedirectToAction("AOuth", "Account");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        //public async Task<IActionResult> AuthenticateNew(LoginModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await signInManager.PasswordSignInAsync(model.Email,model.Password,model.RememberMe,false);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("AOuth", "Account");
        //        }
        //        ModelState.AddModelError(string.Empty, "Некорректные логин и(или) пароль");
        //    }
        //    return View(model);
        //}
    }
}