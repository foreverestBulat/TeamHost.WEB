using Microsoft.AspNetCore.Mvc;

namespace TeamHost.Areas.Main.Controllers;

[Area("Main")]
public class MembersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}