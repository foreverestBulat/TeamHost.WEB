using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;

namespace TeamHost.Areas.Home.Controllers;

[Area("Home")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ChangeLanguage(string culture)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) }
        );

        var returnUrl = Request.Headers["Referer"].ToString();
        if (returnUrl.IsNullOrEmpty())
            return RedirectToAction("Index", "Home");
        return Redirect(returnUrl);
    }
}