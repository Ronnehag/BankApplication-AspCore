using System.Diagnostics;
using System.Threading.Tasks;
using Bank.Application.Bank.Queries.GetBankStatistics;
using Microsoft.AspNetCore.Mvc;
using Bank.WebUI.Models;

namespace Bank.WebUI.Controllers
{
    public class HomeController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            return View(await Mediator.Send(new GetBankStatisticsQuery()));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
