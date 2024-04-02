using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectCouchPotatoV1.Models;
using ProjectCouchPotatoV1.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using static Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.LoginModel;

namespace ProjectCouchPotatoV1.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        //[HttpGet]
        //public IActionResult Login(string returnUrl = null)
        //{
        //    // Your login page logic
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        //{
        //    // Your login post logic
        //    return RedirectToAction("Index", "Home");
        //}
    }
}

