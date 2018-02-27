using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CoinDriveICO.BusinessLayer.Services;
using CoinDriveICO.DataLayer.Model;
using CoinDriveICO.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoinDriveICO.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUsersService _usersService;
        private readonly IEmailService _emailService;

        public AccountController(SignInManager<AppUser> signInManager, 
            IUsersService usersService, 
            IEmailService emailService)
        {
            _signInManager = signInManager;
            _usersService = usersService;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, bool isPersistant, string returnUrl)
        {
            var user = await _usersService.GetUserByUserName(username);
            var checkResult = await _usersService.CheckPasswordEquality(user, password);
            if (checkResult)
            {
                await _signInManager.SignInAsync(user, isPersistant);
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Dashboard");
            }
            return View("Error");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var changeResult = await _usersService.ChangePassword(userId, oldPassword, newPassword);
            return new JsonResult(changeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Register(int? affiliate)
        {
            var viewModel = new RegisterViewModel();
            if (affiliate.HasValue)
            {
                viewModel.Affiliator = affiliate.Value;
            }
            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel.Password == viewModel.PasswordConfirmation)
            {
                var user = await _usersService.RegisterUserAsync(viewModel.Email, viewModel.FullName, viewModel.UserName, viewModel.Password, viewModel.Affiliator);
                var confirmationToken = await _usersService.GenerateConfirmationToken(user);
                var callbackUrl = Url.Action("Confirm", "Account", new {userId = user.Id, token = confirmationToken}, Request.Scheme);
                var sendingResult = await _emailService.SendRegisterConfirmationMessageAsync(user.UserName, user.Email,
                    callbackUrl);
                if (sendingResult)
                {
                    //var result = await _usersService.SetUserAsConfirmedAsync(user.Id, confirmationToken);
                    return View("RegistrationSuccess");
                }
            }
            return View("Error");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            var model = new ForgotPasswordViewModel();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = await _usersService.GetUserByEmail(model.Email);
            if (user != null)
            {
                var resetToken = await _usersService.GeneratePasswordResetToken(user);
                var callbackUrl = Url.Action("ResetPassword", new {userId = user.Id, token = resetToken});
                var sendingResult =
                    await _emailService.SendPasswordResetMessageAsync(user.UserName, user.Email, callbackUrl);
                if (sendingResult)
                {
                    return View("ForgotPasswordRequestSuccess");
                }
            }
            return View("Error");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(int userId, string token)
        {
            var user = await _usersService.GetUserById(userId);
            if (user != null)
            {
                var model = new PasswordResetViewModel
                {
                    UserId = userId,
                    Token = token
                };
                return View(model);
            }
            return View("Error");
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(PasswordResetViewModel model)
        {
            var user = await _usersService.GetUserById(model.UserId);
            if (user != null)
            {
                var resetResult = await _usersService.ResetUsersPassword(model.UserId, model.Token, model.NewPassword);
                if (resetResult)
                {
                    //TODO: Add more logic, views and redirects
                    return View("ResetPasswordRequestSuccess");
                    //return RedirectToAction("Login", "Account");
                }
            }
            return View("Error");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Confirm(int userId, string token)
        {
            var result = await _usersService.SetUserAsConfirmedAsync(userId, token);
            if (result)
            {
                var user = await _usersService.GetUserById(userId);
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Dashboard");
            }
            return View("Error");
        }
    }
}
