using Microsoft.AspNetCore.Mvc;

namespace HostelSoftware.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
