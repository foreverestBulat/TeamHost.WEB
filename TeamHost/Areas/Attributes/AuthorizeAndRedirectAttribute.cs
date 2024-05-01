using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TeamHost.Areas.Attributes
{
    public class AuthorizeAndRedirectAttribute : TypeFilterAttribute
    {
        public AuthorizeAndRedirectAttribute() : base(typeof(CustomAuthorizeFilter))
        {
        }
    }

    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Console.WriteLine("HIST AUTH?");
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult("/Account/Profile/Login");
                Console.WriteLine("HIST AUTH");
            }
        }
    }
}
