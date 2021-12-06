using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OktaDemo.Services;
using System.Threading.Tasks;
using System.Linq;
using OktaDemo.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace OktaAspNetCoreMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsersService usersService;

        public AccountController(UsersService usersService)
        {
            this.usersService = usersService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.First(c => c.Type == "preferred_username").Value;
            var user = await usersService.GetUserAsync(userId);

            var viewModel = new UserViewModel
            {
                Email = userId,
                FirstName = user.Profile.FirstName,
                LastName = user.Profile.LastName,
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserRequest model)
        {
            model.Email = User.Claims.First(c => c.Type == "preferred_username").Value;
            var result = await usersService.UpdateUser(model);

            TempData.Add("success", "User profile updated successfully!");
            return Redirect("/");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return new SignOutResult(new[] { OpenIdConnectDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme },
                new AuthenticationProperties { RedirectUri = "/" });
        }
    }
}
