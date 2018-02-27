using System;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using CoinDriveICO.BusinessLayer.Services;
using CoinDriveICO.Framework.SettingsModels;
using CoinDriveICO.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoinDriveICO.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAdapterApiService _adapterApiService;
        private readonly IUsersService _userService;
        private readonly IEmailService _emailService;
        private readonly MainSettings _mainSettings;
        public DashboardController(IAdapterApiService adapterApiService, 
            IUsersService usersService, 
            IEmailService emailService,
            IOptionsSnapshot<MainSettings> mainSettings)
        {
            _adapterApiService = adapterApiService;
            _userService = usersService;
            _emailService = emailService;
            _mainSettings = mainSettings.Value;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await ConstructViewModel());
        }

        [Authorize]
        public async Task<IActionResult> GetInvestAddress(string adapterName)
        {
            var userId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var refillAddress = await _adapterApiService.GenerateRefillAddressAsync(adapterName, userId, "", false);
            return new JsonResult(refillAddress?.Value?.Address);
        }

        //[AllowAnonymous]
        //public async Task<IActionResult> SendEmailTest()
        //{
        //    var callbackUrl = Url.Action("Confirm","Account",new {userId = User.FindFirst(ClaimTypes.NameIdentifier), confirmationToken = _userService.})
        //    await _emailService.SendRegisterConfirmationMessageAsync("someusername", "oleksandr.torbiievskyi@gmail.com",
        //        callbackUrl);
        //    return new JsonResult("Success");
        //}

        [Authorize]
        public async Task<IActionResult> GetAffiliateLink()
        {
            var actionlink = Url.Action("Register", "Account", new {affiliateId = User.FindFirst(ClaimTypes.NameIdentifier).Value}, Request.Scheme);
            return new JsonResult(actionlink);
        }

        private async Task<DashboardViewModel> ConstructViewModel()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _userService.GetUserById(userId);
            var overallIcoBalance = await _userService.GetOverallBalance();
            var stringFormat = "0.######";
            var viewModel = new DashboardViewModel
            {
                Balance = user.Balance.ToString(stringFormat),
                BtcToTokenRate = _mainSettings.BtcToTokenRate.ToString(stringFormat),
                TokenToBtcRate = (1 / _mainSettings.BtcToTokenRate).ToString(stringFormat),
                Email = user.Email,
                EthToTokenRate = _mainSettings.EthToTokenRate.ToString(stringFormat),
                TokenToEthRate = (1 / _mainSettings.EthToTokenRate).ToString(stringFormat),
                FullName = user.FullName,
                TokenMultiplier = _mainSettings.TokenMultiplier.ToString(stringFormat),
                UserId = userId,
                UserName = user.UserName,
                OverallBalance = overallIcoBalance.ToString(stringFormat),
                MaxGoal = 100000.ToString(stringFormat),
                PercentsGot = Convert.ToInt32(overallIcoBalance / 100000)
            };
            return viewModel;
        }
    }
}