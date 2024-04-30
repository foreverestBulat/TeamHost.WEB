using System.Diagnostics;
using Microsoft.AspNetCore.Localization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TeamHost.Models;

namespace TeamHost.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IStringLocalizer<HomeController> _localizer;

    public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
    {
        _logger = logger;
        _localizer = localizer;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ChangeLanguage(string culture)
    {
        Console.WriteLine(culture);
        //Response.Cookies.Append(
        //    CookieRequestCultureProvider.DefaultCookieName,
        //    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        //    new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) }
        //);

        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
