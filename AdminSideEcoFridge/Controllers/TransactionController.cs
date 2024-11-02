using Microsoft.AspNetCore.Mvc;

namespace AdminSideEcoFridge.Controllers
{
    public class TransactionController : BaseController
    {
        public IActionResult Subscribers()
        {
            return View();
        }
    }
}
