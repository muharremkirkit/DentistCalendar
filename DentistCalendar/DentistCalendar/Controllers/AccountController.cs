using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentistCalendar.Data.Entity;
using DentistCalendar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace DentistCalendar.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager; //kullanıcı ekliyeceğim
        private SignInManager<AppUser> _sıgnInManager; //oturum açtıracağım
        private RoleManager<AppRole> _roleManager; //role ekleyeceğim
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _sıgnInManager = signInManager;
            _roleManager = roleManager;

        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>  Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //kullanıcı usermanagerde kayıtlı mı
            var user = await _userManager.FindByNameAsync(model.UserName); //asekron olduğu için await dedik
            if (user==null) //kullanıcı kayıtlı değilse
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı Bulunamadı");
                return View(model);
            }
            //kullanıcıyı sıgnInUserda şifre kontrol edicek ve beni hatılaya bakacak. false ise yanlış deneme sayısı olmucak. true olursa belirli bir yanlış deneme sayısı olucak
            var result = await _sıgnInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded) //oturum açma başarılı ise
            {
                return RedirectToAction("Index", "Profile");
            }
            ModelState.AddModelError(string.Empty, "Oturum Açılamadı"); //hata varsa
            return View(model);

        }


        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) //model valid değilse modeli döndür
            {
                return View(model);
            }
            AppUser user = new AppUser() //model valid ise user oluşturcaz //entity oluşturduk
            {
                UserName = model.UserName,
                Name = model.Name,
                Surname = model.SurName,
                Email = model.Email,
                Color = model.Color,
                IsDentist = model.IsDentist,
            };
            IdentityResult result = _userManager.CreateAsync(user, model.Password).Result;
            //Usermanager kullanarak veri tabanına ekliyoruz create methoduyla
            if (result.Succeeded)
            {
                bool roleCheck = model.IsDentist ? AddRole("Dentist") : AddRole("Secretary");
                //eğer kullanıcı dişçiyse dişçi rolü ekle değilse sekreter ekle
                if (!roleCheck) //eğer role eklenmediyse, hata varsa
                {
                    return View("Error");
                }
                _userManager.AddToRoleAsync(user, model.IsDentist ? "Dentist" : "Secretary").Wait();
                //rol ekleme başarılı ise veya role varsa user dişçi ise dişçi rolu ekle ve işlem bitene kadar bekle
                return RedirectToAction("Index", "Home");
            }
            return View("Error");
        }

        public IActionResult LogOut()
        {
            _sıgnInManager.SignOutAsync().Wait(); //sıgnInManagerden out methodunu çalıştır
            return RedirectToAction("Login");
        }
        public IActionResult Denied()
        {

            return View();
        }

        private bool AddRole(string roleName)
        {
            if (!_roleManager.RoleExistsAsync(roleName).Result) //rolun veritabında olup olmadığını kontrol ediyor
            {
                AppRole role = new AppRole() //veritabında yoksa yeni role oluşturcaz
                {
                    Name = roleName
                };
                IdentityResult result = _roleManager.CreateAsync(role).Result; //rolemanager create methoyla eklicez
                return result.Succeeded;
            }
            return true; //role varsa zaten true dönecek
        }
    }
}