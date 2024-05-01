using Microsoft.AspNetCore.Mvc;
using TeamHost.Areas.Attributes;

namespace TeamHost.Areas.Account.Controllers;

[Area("Account")]
public class FavouritesController : Controller
{
    [AuthorizeAndRedirect]
    public IActionResult Index()
    {
        return View();
    }
}