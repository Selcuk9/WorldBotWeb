using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using InstaBotWeb.Models;
using InstaBotWeb.Models.DataBaseContext;
using InstaBotWeb.ViewsModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        public AccountController(ApplicationContext context, IHashMethod hash, IWebHostEnvironment environment)
        {
            db = context;
            this.hash = hash;
            this.environment = environment;
        }

        private int GetIdUser()
        {
           // ViewBag.UserMail = User.Identity.Name;
            var claim = User.Claims.ToList();
            var idUser = claim[1].Type == "BaseId" ? claim[1].Value : null;
            return int.Parse(idUser);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AOuth()
        {
            //user = JsonConvert.DeserializeObject<User>(serializedModel);

            var idUser = GetIdUser();
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
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AOuth", "Account");
            }
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
                    await Authenticate(model.Email, userId); // аутентификация
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

                    await Authenticate(model.Email, userId); // аутентификация

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
        private async Task Authenticate(string userName, string idUser)
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
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
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
            string fileName = profile.Avatar.FileName;
            string path = @"/img/photoUsers/" + fileName;
            string pathPhoto = environment.WebRootPath + path;
            using (var photo = new FileStream(pathPhoto, FileMode.Create))
            {
                await profile.Avatar.CopyToAsync(photo);
            }
            var chUser = db.DbUsers.Where(uId => uId.Id == GetIdUser()).FirstOrDefault();
            chUser.LastName = profile.LastName;
            chUser.FirstName = profile.FirstName;
            chUser.AvatarProfile = @$"/img/photoUsers/{fileName}";
            await db.SaveChangesAsync();

            return RedirectToAction("AOuth", "Account");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}