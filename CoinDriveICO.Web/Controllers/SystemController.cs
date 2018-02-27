using System.Collections;
using System.Net;
using System.Threading.Tasks;
using CoinDriveICO.BusinessLayer;
using CoinDriveICO.Framework.SettingsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoinDriveICO.Web.Controllers
{
    public class SystemController : Controller
    {
        private readonly ITransactionWorkerService _worker;
        private readonly string _masterWorkerKey;
        public SystemController(ITransactionWorkerService transactionWorkerService, IOptionsSnapshot<MainSettings> mainSettings)
        {
            _worker = transactionWorkerService;
            _masterWorkerKey = mainSettings.Value.MasterWorkerKey;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RunTransactionsWorker(string masterWorkerKey)
        {
            if (masterWorkerKey != _masterWorkerKey)
            {
                Response.StatusCode = 401;
                return new JsonResult(new {status = 401, message = "Unathorized"});
            }
            var result = await _worker.ProcessTransactionsAsync();
            return new JsonResult(result);
        }
    }
}