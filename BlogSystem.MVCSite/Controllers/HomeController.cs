using BlogSystem.BLL;
using BlogSystem.MVCSite.Filters;
using BlogSystem.MVCSite.Models.UserViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BlogSystem.MVCSite.Controllers
{
    public class HomeController : Controller
    {
        IBLL.IUserManager userManager = new UserManager();
        [BlogSystemAuth]
        public ActionResult Index()
        {
            return View();
        }
        [BlogSystemAuth]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [BlogSystemAuth]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Models.UserViewModels.RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await userManager.RegisterAsync(model.Email, model.Password);
            return Content("注册成功    ");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid userId;
                if (userManager.LoginAsync(model.LoginName, model.LoginPwd, out userId))
                {
                    if (model.RememberMe)
                    {
                        Response.Cookies.Add(new System.Web.HttpCookie(name: "loginName")
                        {

                            Value = model.LoginName,
                            Expires = DateTime.Now.AddDays(7)
                        });
                        //为了获取到userId，方便传输使用
                        Response.Cookies.Add(new System.Web.HttpCookie(name: "userId")
                        {

                            Value = userId.ToString(),
                            Expires = DateTime.Now.AddDays(7)
                        });
                    }
                    else
                    {
                        Session["loginName"] = model.LoginName;
                        Session["userId"] = userId.ToString();
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ModelState.AddModelError("", "您的账号信息有误");
            }
            return View(model);
        }
    }
}