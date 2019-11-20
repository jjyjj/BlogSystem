using System.Web.Mvc;
using System.Web.Routing;

namespace BlogSystem.MVCSite.Filters
{
    public class BlogSystemAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //当用户信息储存在cookie中且session为空时，把cookie的数据传到session中
            if (filterContext.HttpContext.Request.Cookies["loginName"] != null&& filterContext.HttpContext.Session["loginName"] == null)
            {
                filterContext.HttpContext.Session["loginName"] = filterContext.HttpContext.Request.Cookies["loginName"];
                filterContext.HttpContext.Session["userId"] = filterContext.HttpContext.Request.Cookies["userId"];
            }



           // base.OnAuthorization(filterContext);
            if (!(filterContext.HttpContext.Session["loginName"] != null || filterContext.HttpContext.Request.Cookies["loginName"] != null))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary()
                {
                    { "controller","Home"},
                    { "action","Login"}
                });

            }
        }
    }
}