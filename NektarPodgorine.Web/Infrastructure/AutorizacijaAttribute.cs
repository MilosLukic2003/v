using System.Net;
using System.Web.Mvc;

namespace NektarPodgorine.Web.Infrastructure
{
    public class AutorizacijaAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                return;
            }

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}
