using System.Web.Helpers;
using System.Web.Mvc;

namespace NektarPodgorine.Web.Infrastructure
{
    public sealed class ValidacijaTokenaZaPost : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (filterContext.IsChildAction || request.HttpMethod != "POST")
            {
                return;
            }

            var cookie = request.Cookies[AntiForgeryConfig.CookieName];
            AntiForgery.Validate(cookie != null ? cookie.Value : null, request.Form["__RequestVerificationToken"]);
        }
    }
}
