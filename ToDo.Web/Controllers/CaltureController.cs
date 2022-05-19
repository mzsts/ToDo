using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ToDo.Web.Controllers;

[Route("[controller]/[action]")]
public class CaltureController : Controller
{
    public IActionResult SetCalture(string calture, string redirectUri)
    {
        if(calture is not null)
        {
            HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(calture)));
        }

        return LocalRedirect(redirectUri);
    }
}
