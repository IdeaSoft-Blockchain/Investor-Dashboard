using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoinDriveICO.BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using CoinDriveICO.Web.Models;

namespace CoinDriveICO.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAdapterApiService _adapterApiService;

        public HomeController(IAdapterApiService adapterApiService)
        {
            _adapterApiService = adapterApiService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _adapterApiService.GetAdapterInfoAsync("BTC"));
        }

        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "Your application description page.";

            return View(await _adapterApiService.GenerateRefillAddressAsync("BTC",0,"testTag",true));
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
