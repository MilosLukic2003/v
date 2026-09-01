using System.Web;
using System.Web.Mvc;
using NektarPodgorine.Web.Infrastructure;

namespace NektarPodgorine.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ValidacijaTokenaZaPost());
        }
    }
}
