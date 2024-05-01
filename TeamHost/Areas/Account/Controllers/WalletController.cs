using Microsoft.AspNetCore.Mvc;
using TeamHost.Areas.Attributes;

namespace TeamHost.Areas.Account.Controllers;

[AuthorizeAndRedirect]
[Area("Account")]
public class WalletController : Controller
{ 
    public IActionResult Index()
    {
        return View();
    }
}